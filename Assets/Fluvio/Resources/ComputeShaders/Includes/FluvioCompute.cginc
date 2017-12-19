#ifndef FLUVIO_COMPUTE_INCLUDED
#define FLUVIO_COMPUTE_INCLUDED

// We include this manually due to how nested includes work currently with the HLSL compiler
#ifndef FLUVIO_LANG_INCLUDED
#define FLUVIO_LANG_INCLUDED

// Define FALSE as 0 and TRUE as non-zero
#define FALSE 0
#define TRUE !FALSE

// OpenCL/HLSL specific syntax
#ifdef FLUVIO_API_OPENCL
    // Buffers
	#define FLUVIO_BUFFER(bufferType) __global const bufferType*
	#define FLUVIO_BUFFER_ARG(bufferType) __global __read_only const bufferType*
	#define FLUVIO_BUFFER_RW(bufferType) __global bufferType*
	#define FLUVIO_TEXTURE(textureName) image2d_t textureName
	#define FLUVIO_SAMPLER(textureName) sampler_t sampler##textureName
	#define FLUVIO_SAMPLE_TEXTURE(tex, samp, uv) read_imagef(tex, samp, uv)
	#define FLUVIO_INITIALIZE(type) ((type)(0))
	#define FLUVIO_BARRIER barrier(CLK_GLOBAL_MEM_FENCE)
	inline float lerp(float a, float b, float w) { return a + w*(b-a); }
    inline float2 lerp2(float2 a, float2 b, float2 w) { return a + w*(b-a); }
    inline float3 lerp3(float3 a, float3 b, float3 w) { return a + w*(b-a); }
    inline float4 lerp4(float4 a, float4 b, float4 w) { return a + w*(b-a); }
    #define mat4x4 float16
    inline float4 mul3x4(mat4x4 M, float4 v)
    {
        float4 r;

        float4 a = M.s048C;
        float4 b = M.s159D;
        float4 c = M.s26AE;

        r.x = dot(a.xyz, v.xyz) + a.w;
        r.y = dot(b.xyz, v.xyz) + b.w;
        r.z = dot(c.xyz, v.xyz) + c.w;
        r.w = v.w;

        return r;
    }
    inline float4 mulv(mat4x4 M, float4 v)
    {
        float4 r;

        float3 a = M.s048;
        float3 b = M.s159;
        float3 c = M.s26A;

        r.x = dot(a, v.xyz);
        r.y = dot(b, v.xyz);
        r.z = dot(c, v.xyz);
        r.w = v.w;

        return r;
    }
#else
    #ifndef FLUVIO_API_UNITYNATIVE
        #define FLUVIO_API_UNITYNATIVE
    #endif
    #define FLUVIO_BUFFER(bufferType) StructuredBuffer<bufferType>
	#define FLUVIO_BUFFER_ARG(bufferType) StructuredBuffer<bufferType>
	#define FLUVIO_BUFFER_RW(bufferType) RWStructuredBuffer<bufferType>
	#define FLUVIO_TEXTURE(textureName) Texture2D<float4> textureName
	#define FLUVIO_SAMPLER(textureName) SamplerState sampler##textureName
	#define FLUVIO_SAMPLE_TEXTURE(tex, samp, uv) tex.SampleLevel(samp, uv, 0)
	#define FLUVIO_INITIALIZE(type) ((type)0)
	#define FLUVIO_BARRIER AllMemoryBarrierWithGroupSync()
	#define get_global_id(i) fluvio_DispatchThreadID[(i)]
	#define get_local_id(i) fluvio_GroupThreadID[(i)]
	#define lerp2 lerp
    #define lerp3 lerp
    #define lerp4 lerp
    #define fabs abs
    #define mat4x4 float4x4
    inline float4 mul3x4(mat4x4 M, float4 v)
    {
        float4 r;

        float4 a = float4(M._m00, M._m01, M._m02, M._m03);
        float4 b = float4(M._m10, M._m11, M._m12, M._m13);
        float4 c = float4(M._m20, M._m21, M._m22, M._m23);

        r.x = dot(a.xyz, v.xyz) + a.w;
        r.y = dot(b.xyz, v.xyz) + b.w;
        r.z = dot(c.xyz, v.xyz) + c.w;
        r.w = v.w;

        return r;
    }
    inline float4 mulv(mat4x4 M, float4 v)
    {
        float4 r;

        float3 a = float3(M._m00, M._m01, M._m02);
        float3 b = float3(M._m10, M._m11, M._m12);
        float3 c = float3(M._m20, M._m21, M._m22);

        r.x = dot(a, v.xyz);
        r.y = dot(b, v.xyz);
        r.z = dot(c, v.xyz);
        r.w = v.w;

        return r;
    }
#endif

// Fluid data struct
typedef struct {
	float4 gravity; // xyz - gravity, w - unused
	float initialDensity;
	float minimumDensity;
	float particleMass;
	float viscosity;
	float turbulence;
	float surfaceTension;
	float gasConstant;
	float buoyancyCoefficient;
} FluidData;

// Fluid particle struct
typedef struct {
	// Particle properties
	float4 position; // xyz - position, w - unused
	float4 velocity; // xyz - velocity, w - particle size
	float4 color; // rgba color
	float4 vorticityTurbulence; // xyz - vorticity, w - turbulence probability
	float4 lifetime; // x - lifetime, yzw - unused
	int4 id; // x - fluid id, y - particle id, z - neighbor count, w - unused

			 // Physical properties
	float4 force; //xyz - force, w - unused
	float4 normal; // xyz - normal, w - prenormalized length
	float4 densityPressure; // x - density, y - self density, z - pressure, w - unused
} FluidParticle;

// Keyframe struct
typedef struct {
    float time;
    float value;
    float inTangent;
    float outTangent;
    int tangentMode;
} Keyframe;

// Gradient color key struct
typedef struct {
    float cR;
    float cG;
    float cB;
    float cA;
    float time;
} GradientColorKey;

// Gradient alpha key struct
typedef struct {
    float alpha;
    float time;
} GradientAlphaKey;

#endif // FLUVIO_LANG_INCLUDED

#define FLUVIO_PI 3.14159265359f

// Grid constants (change in FluvioSettings inspector only!)
#define FLUVIO_USE_INDEX_GRID_ON
#define FLUVIO_MAX_GRID_SIZE 64
#define FLUVIO_GRID_BUCKET_SIZE 64
#define FLUVIO_GRID_LENGTH 16777216

// Simulation constants - these cannot be changed at this time
#define FLUVIO_THREAD_GROUP_SIZE 128 // FluvioSettings.kComputeThreadGroupSize
#define FLUVIO_THREAD_GROUP_SIZE_X 16 // FluvioSettings.kComputeThreadGroupSizeX
#define FLUVIO_THREAD_GROUP_SIZE_Y 16 // FluvioSettings.kComputeThreadGroupSizeY
#define FLUVIO_THREAD_GROUP_SIZE_Z 4 // FluvioSettings.kComputeThreadGroupSizeZ
#define FLUVIO_EPSILON 9.99999944e-11f // FluvioSettings.kEpsilon
#define FLUVIO_MAX_SQR_VELOCITY_CHANGE 100.0f // FluvioSettings.kMaxSqrVelocityChange
#define FLUVIO_TURBULENCE_CONSTANT 10.0f // FluvioSettings.kTurbulenceConstant

// Force mode
#define FLUVIO_FORCE_MODE_FORCE 0
#define FLUVIO_FORCE_MODE_ACCELERATION 1
#define FLUVIO_FORCE_MODE_IMPULSE 2
#define FLUVIO_FORCE_MODE_VELOCITY_CHANGE 3

// Solver read-write only buffers
#ifdef FLUVIO_SOLVER
    #define FLUVIO_BUFFER_SOLVER_RW(bufferType) FLUVIO_BUFFER_RW(bufferType)
#else
    #define FLUVIO_PLUGIN
    #define FLUVIO_BUFFER_SOLVER_RW(bufferType) FLUVIO_BUFFER_ARG(bufferType)
#endif

// Bounds checking
#define FLUVIO_OUT_OF_BOUNDS(index, size) ((index) >= (size))
#define FLUVIO_BOUNDS_CHECK(index, size) { if (FLUVIO_OUT_OF_BOUNDS((index), (size))) return; }

// Kernel entry point mega-macro
// OpenCL passes data in the form of arguments, wheras Unity compute shaders do not.
// This macro takes care of all of that.
// To use this macro, buffer types need to be provided. For example:
//		#define FLUVIO_PLUGIN_DATA_0 float4
//		#define FLUVIO_PLUGIN_DATA_RW_1 float
// Up to 10 buffers are supported (one for each slot). Use the FluvioGetPluginBuffer/FluvioGetPluginValue macros to get plugin data.
// Min-max curves should be defined as the type Keyframe.
// Min-max gradients use two buffers and should be defined as the types GradientColorKey and GradientAlphaKey.
#ifdef FLUVIO_API_OPENCL

    #ifdef FLUVIO_PLUGIN
        #define FLUVIO_PLUGIN_ARG_0
        #define FLUVIO_PLUGIN_ARG_1
        #define FLUVIO_PLUGIN_ARG_2
        #define FLUVIO_PLUGIN_ARG_3
        #define FLUVIO_PLUGIN_ARG_4
        #define FLUVIO_PLUGIN_ARG_5
        #define FLUVIO_PLUGIN_ARG_6
        #define FLUVIO_PLUGIN_ARG_7
        #define FLUVIO_PLUGIN_ARG_8
        #define FLUVIO_PLUGIN_ARG_9

        #ifdef FLUVIO_PLUGIN_DATA_0
            #undef FLUVIO_PLUGIN_ARG_0
            #define FLUVIO_PLUGIN_ARG_0 , FLUVIO_BUFFER(FLUVIO_PLUGIN_DATA_0) fluvio_PluginData0
        #endif
        #ifdef FLUVIO_PLUGIN_DATA_1
            #undef FLUVIO_PLUGIN_ARG_1
            #define FLUVIO_PLUGIN_ARG_1 , FLUVIO_BUFFER(FLUVIO_PLUGIN_DATA_1) fluvio_PluginData1
        #endif
        #ifdef FLUVIO_PLUGIN_DATA_2
            #undef FLUVIO_PLUGIN_ARG_2
            #define FLUVIO_PLUGIN_ARG_2 , FLUVIO_BUFFER(FLUVIO_PLUGIN_DATA_2) fluvio_PluginData2
        #endif
        #ifdef FLUVIO_PLUGIN_DATA_3
            #undef FLUVIO_PLUGIN_ARG_3
            #define FLUVIO_PLUGIN_ARG_3 , FLUVIO_BUFFER(FLUVIO_PLUGIN_DATA_3) fluvio_PluginData3
        #endif
        #ifdef FLUVIO_PLUGIN_DATA_4
            #undef FLUVIO_PLUGIN_ARG_4
            #define FLUVIO_PLUGIN_ARG_4 , FLUVIO_BUFFER(FLUVIO_PLUGIN_DATA_4) fluvio_PluginData4
        #endif
        #ifdef FLUVIO_PLUGIN_DATA_5
            #undef FLUVIO_PLUGIN_ARG_5
            #define FLUVIO_PLUGIN_ARG_5 , FLUVIO_BUFFER(FLUVIO_PLUGIN_DATA_5) fluvio_PluginData5
        #endif
        #ifdef FLUVIO_PLUGIN_DATA_6
            #undef FLUVIO_PLUGIN_ARG_6
            #define FLUVIO_PLUGIN_ARG_6 , FLUVIO_BUFFER(FLUVIO_PLUGIN_DATA_6) fluvio_PluginData6
        #endif
        #ifdef FLUVIO_PLUGIN_DATA_7
            #undef FLUVIO_PLUGIN_ARG_7
            #define FLUVIO_PLUGIN_ARG_7 , FLUVIO_BUFFER(FLUVIO_PLUGIN_DATA_7) fluvio_PluginData7
        #endif
        #ifdef FLUVIO_PLUGIN_DATA_8
            #undef FLUVIO_PLUGIN_ARG_8
            #define FLUVIO_PLUGIN_ARG_8 , FLUVIO_BUFFER(FLUVIO_PLUGIN_DATA_8) fluvio_PluginData8
        #endif
        #ifdef FLUVIO_PLUGIN_DATA_9
            #undef FLUVIO_PLUGIN_ARG_9
            #define FLUVIO_PLUGIN_ARG_9 , FLUVIO_BUFFER(FLUVIO_PLUGIN_DATA_9) fluvio_PluginData9
        #endif

        #ifdef FLUVIO_PLUGIN_DATA_RW_0
            #undef FLUVIO_PLUGIN_ARG_0
            #define FLUVIO_PLUGIN_ARG_0 , FLUVIO_BUFFER_RW(FLUVIO_PLUGIN_DATA_RW_0) fluvio_PluginData0
        #endif
        #ifdef FLUVIO_PLUGIN_DATA_RW_1
            #undef FLUVIO_PLUGIN_ARG_1
            #define FLUVIO_PLUGIN_ARG_1 , FLUVIO_BUFFER_RW(FLUVIO_PLUGIN_DATA_RW_1) fluvio_PluginData1
        #endif
        #ifdef FLUVIO_PLUGIN_DATA_RW_2
            #undef FLUVIO_PLUGIN_ARG_2
            #define FLUVIO_PLUGIN_ARG_2 , FLUVIO_BUFFER_RW(FLUVIO_PLUGIN_DATA_RW_2) fluvio_PluginData2
        #endif
        #ifdef FLUVIO_PLUGIN_DATA_RW_3
            #undef FLUVIO_PLUGIN_ARG_3
            #define FLUVIO_PLUGIN_ARG_3 , FLUVIO_BUFFER_RW(FLUVIO_PLUGIN_DATA_RW_3) fluvio_PluginData3
        #endif
        #ifdef FLUVIO_PLUGIN_DATA_RW_4
            #undef FLUVIO_PLUGIN_ARG_4
            #define FLUVIO_PLUGIN_ARG_4 , FLUVIO_BUFFER_RW(FLUVIO_PLUGIN_DATA_RW_4) fluvio_PluginData4
        #endif
        #ifdef FLUVIO_PLUGIN_DATA_RW_5
            #undef FLUVIO_PLUGIN_ARG_5
            #define FLUVIO_PLUGIN_ARG_5 , FLUVIO_BUFFER_RW(FLUVIO_PLUGIN_DATA_RW_5) fluvio_PluginData5
        #endif
        #ifdef FLUVIO_PLUGIN_DATA_RW_6
            #undef FLUVIO_PLUGIN_ARG_6
            #define FLUVIO_PLUGIN_ARG_6 , FLUVIO_BUFFER_RW(FLUVIO_PLUGIN_DATA_RW_6) fluvio_PluginData6
        #endif
        #ifdef FLUVIO_PLUGIN_DATA_RW_7
            #undef FLUVIO_PLUGIN_ARG_7
            #define FLUVIO_PLUGIN_ARG_7 , FLUVIO_BUFFER_RW(FLUVIO_PLUGIN_DATA_RW_7) fluvio_PluginData7
        #endif
        #ifdef FLUVIO_PLUGIN_DATA_RW_8
            #undef FLUVIO_PLUGIN_ARG_8
            #define FLUVIO_PLUGIN_ARG_8 , FLUVIO_BUFFER(FLUVIO_PLUGIN_DATA_RW_8) fluvio_PluginData8
        #endif
        #ifdef FLUVIO_PLUGIN_DATA_RW_9
            #undef FLUVIO_PLUGIN_ARG_9
            #define FLUVIO_PLUGIN_ARG_9 , FLUVIO_BUFFER(FLUVIO_PLUGIN_DATA_RW_9) fluvio_PluginData9
        #endif
    #endif

    #ifdef FLUVIO_SOLVER
		#ifdef FLUVIO_USE_INDEX_GRID_ON
			#define FLUVIO_KERNEL_INDEX_GRID(kernelName) \
				__kernel \
				void kernelName( \
					int4 fluvio_Count, \
					int fluvio_Stride, \
					float4 fluvio_KernelSize, \
					float4 fluvio_KernelFactors, \
					float4 fluvio_Time, \
					FLUVIO_BUFFER_RW(FluidParticle) fluvio_Particle, \
					FLUVIO_BUFFER_SOLVER_RW(int) fluvio_Neighbors, \
					FLUVIO_BUFFER_SOLVER_RW(FluidParticle) fluvio_IndexGridBoundaryParticle, \
					volatile FLUVIO_BUFFER_SOLVER_RW(uint) fluvio_IndexGrid)
		#else
			#define FLUVIO_KERNEL_INDEX_GRID(kernelName) \
				__kernel \
				void kernelName( \
					int4 fluvio_Count, \
					int fluvio_Stride, \
					float4 fluvio_KernelSize, \
					float4 fluvio_KernelFactors, \
					float4 fluvio_Time, \
					FLUVIO_BUFFER_RW(FluidParticle) fluvio_Particle, \
					FLUVIO_BUFFER_SOLVER_RW(int) fluvio_Neighbors, \
					FLUVIO_BUFFER_SOLVER_RW(FluidParticle) fluvio_IndexGridBoundaryParticle)
		#endif

		#define FLUVIO_KERNEL_BOUNDARY_TEXTURE(kernelName) \
            __kernel \
            void kernelName( \
                int4 fluvio_Count, \
				int fluvio_Stride, \
                float4 fluvio_KernelSize, \
                float4 fluvio_KernelFactors, \
                float4 fluvio_Time, \
                FLUVIO_BUFFER_RW(FluidParticle) fluvio_Particle, \
                FLUVIO_BUFFER_SOLVER_RW(int) fluvio_Neighbors, \
                FLUVIO_BUFFER_ARG(FluidData) fluvio_Fluid, \
                FLUVIO_BUFFER_SOLVER_RW(FluidParticle) fluvio_BoundaryParticle, \
                FLUVIO_TEXTURE(fluvio_BoundaryTexture), \
                FLUVIO_SAMPLER(fluvio_BoundaryTexture), \
                int4 fluvio_BoundaryTexture_Params, \
                float4 fluvio_BoundaryTexture_Bounds, \
                mat4x4 fluvio_BoundaryTexture_WorldToLocal, \
                float4 fluvio_BoundaryTexture_UV, \
                float fluvio_BoundaryTexture_Threshold)

		#define FLUVIO_KERNEL(kernelName) \
            __kernel \
            void kernelName( \
                int4 fluvio_Count, \
				int fluvio_Stride, \
                float4 fluvio_KernelSize, \
                float4 fluvio_KernelFactors, \
                float4 fluvio_Time, \
                FLUVIO_BUFFER_RW(FluidParticle) fluvio_Particle, \
                FLUVIO_BUFFER_SOLVER_RW(int) fluvio_Neighbors, \
                FLUVIO_BUFFER_ARG(FluidData) fluvio_Fluid, \
                FLUVIO_BUFFER_SOLVER_RW(FluidParticle) fluvio_BoundaryParticle)
    #else
        #define FLUVIO_KERNEL(kernelName) \
            __kernel \
            void kernelName( \
                int4 fluvio_Count, \
                int fluvio_Stride, \
                float4 fluvio_KernelSize, \
                float4 fluvio_KernelFactors, \
                float4 fluvio_Time, \
                FLUVIO_BUFFER_RW(FluidParticle) fluvio_Particle, \
                FLUVIO_BUFFER_SOLVER_RW(int) fluvio_Neighbors, \
                FLUVIO_BUFFER_ARG(FluidData) fluvio_Fluid, \
                int fluvio_IncludeFluidGroup, \
                int fluvio_PluginFluidID \
                FLUVIO_PLUGIN_ARG_0 \
                FLUVIO_PLUGIN_ARG_1 \
                FLUVIO_PLUGIN_ARG_2 \
                FLUVIO_PLUGIN_ARG_3 \
                FLUVIO_PLUGIN_ARG_4 \
                FLUVIO_PLUGIN_ARG_5 \
                FLUVIO_PLUGIN_ARG_6 \
                FLUVIO_PLUGIN_ARG_7 \
                FLUVIO_PLUGIN_ARG_8 \
                FLUVIO_PLUGIN_ARG_9)
    #endif
#else
    #ifdef FLUVIO_PLUGIN
        #define FLUVIO_PLUGIN_DECL_0
        #define FLUVIO_PLUGIN_DECL_1
        #define FLUVIO_PLUGIN_DECL_2
        #define FLUVIO_PLUGIN_DECL_3
        #define FLUVIO_PLUGIN_DECL_4
        #define FLUVIO_PLUGIN_DECL_5
        #define FLUVIO_PLUGIN_DECL_6
        #define FLUVIO_PLUGIN_DECL_7
        #define FLUVIO_PLUGIN_DECL_8
        #define FLUVIO_PLUGIN_DECL_9

        #ifdef FLUVIO_PLUGIN_DATA_0
            #undef FLUVIO_PLUGIN_DECL_0
            #define FLUVIO_PLUGIN_DECL_0 FLUVIO_BUFFER(FLUVIO_PLUGIN_DATA_0) fluvio_PluginData0
        #endif
        #ifdef FLUVIO_PLUGIN_DATA_1
            #undef FLUVIO_PLUGIN_DECL_1
            #define FLUVIO_PLUGIN_DECL_1 FLUVIO_BUFFER(FLUVIO_PLUGIN_DATA_1) fluvio_PluginData1
        #endif
        #ifdef FLUVIO_PLUGIN_DATA_2
            #undef FLUVIO_PLUGIN_DECL_2
            #define FLUVIO_PLUGIN_DECL_2 FLUVIO_BUFFER(FLUVIO_PLUGIN_DATA_2) fluvio_PluginData2
        #endif
        #ifdef FLUVIO_PLUGIN_DATA_3
            #undef FLUVIO_PLUGIN_DECL_3
            #define FLUVIO_PLUGIN_DECL_3 FLUVIO_BUFFER(FLUVIO_PLUGIN_DATA_3) fluvio_PluginData3
        #endif
        #ifdef FLUVIO_PLUGIN_DATA_4
            #undef FLUVIO_PLUGIN_DECL_4
            #define FLUVIO_PLUGIN_DECL_4 FLUVIO_BUFFER(FLUVIO_PLUGIN_DATA_4) fluvio_PluginData4
        #endif
        #ifdef FLUVIO_PLUGIN_DATA_5
            #undef FLUVIO_PLUGIN_DECL_5
            #define FLUVIO_PLUGIN_DECL_5 FLUVIO_BUFFER(FLUVIO_PLUGIN_DATA_5) fluvio_PluginData5
        #endif
        #ifdef FLUVIO_PLUGIN_DATA_6
            #undef FLUVIO_PLUGIN_DECL_6
            #define FLUVIO_PLUGIN_DECL_6 FLUVIO_BUFFER(FLUVIO_PLUGIN_DATA_6) fluvio_PluginData6
        #endif
        #ifdef FLUVIO_PLUGIN_DATA_7
            #undef FLUVIO_PLUGIN_DECL_7
            #define FLUVIO_PLUGIN_DECL_7 FLUVIO_BUFFER(FLUVIO_PLUGIN_DATA_7) fluvio_PluginData7
        #endif
        #ifdef FLUVIO_PLUGIN_DATA_8
            #undef FLUVIO_PLUGIN_DECL_8
            #define FLUVIO_PLUGIN_DECL_8 FLUVIO_BUFFER(FLUVIO_PLUGIN_DATA_8) fluvio_PluginData8
        #endif
        #ifdef FLUVIO_PLUGIN_DATA_9
            #undef FLUVIO_PLUGIN_DECL_9
            #define FLUVIO_PLUGIN_DECL_9 FLUVIO_BUFFER(FLUVIO_PLUGIN_DATA_9) fluvio_PluginData9
        #endif

        #ifdef FLUVIO_PLUGIN_DATA_RW_0
            #undef FLUVIO_PLUGIN_DECL_0
            #define FLUVIO_PLUGIN_DECL_0 FLUVIO_BUFFER_RW(FLUVIO_PLUGIN_DATA_RW_0) fluvio_PluginData0
        #endif
        #ifdef FLUVIO_PLUGIN_DATA_RW_1
            #undef FLUVIO_PLUGIN_DECL_1
            #define FLUVIO_PLUGIN_DECL_1 FLUVIO_BUFFER_RW(FLUVIO_PLUGIN_DATA_RW_1) fluvio_PluginData1
        #endif
        #ifdef FLUVIO_PLUGIN_DATA_RW_2
            #undef FLUVIO_PLUGIN_DECL_2
            #define FLUVIO_PLUGIN_DECL_2 FLUVIO_BUFFER_RW(FLUVIO_PLUGIN_DATA_RW_2) fluvio_PluginData2
        #endif
        #ifdef FLUVIO_PLUGIN_DATA_RW_3
            #undef FLUVIO_PLUGIN_DECL_3
            #define FLUVIO_PLUGIN_DECL_3 FLUVIO_BUFFER_RW(FLUVIO_PLUGIN_DATA_RW_3) fluvio_PluginData3
        #endif
        #ifdef FLUVIO_PLUGIN_DATA_RW_4
            #undef FLUVIO_PLUGIN_DECL_4
            #define FLUVIO_PLUGIN_DECL_4 FLUVIO_BUFFER_RW(FLUVIO_PLUGIN_DATA_RW_4) fluvio_PluginData4
        #endif
        #ifdef FLUVIO_PLUGIN_DATA_RW_5
            #undef FLUVIO_PLUGIN_DECL_5
            #define FLUVIO_PLUGIN_DECL_5 FLUVIO_BUFFER_RW(FLUVIO_PLUGIN_DATA_RW_5) fluvio_PluginData5
        #endif
        #ifdef FLUVIO_PLUGIN_DATA_RW_6
            #undef FLUVIO_PLUGIN_DECL_6
            #define FLUVIO_PLUGIN_DECL_6 FLUVIO_BUFFER_RW(FLUVIO_PLUGIN_DATA_RW_6) fluvio_PluginData6
        #endif
        #ifdef FLUVIO_PLUGIN_DATA_RW_7
            #undef FLUVIO_PLUGIN_DECL_7
            #define FLUVIO_PLUGIN_DECL_7 FLUVIO_BUFFER_RW(FLUVIO_PLUGIN_DATA_RW_7) fluvio_PluginData7
        #endif
        #ifdef FLUVIO_PLUGIN_DATA_RW_8
            #undef FLUVIO_PLUGIN_DECL_8
            #define FLUVIO_PLUGIN_DECL_8 FLUVIO_BUFFER(FLUVIO_PLUGIN_DATA_RW_8) fluvio_PluginData8
        #endif
        #ifdef FLUVIO_PLUGIN_DATA_RW_9
            #undef FLUVIO_PLUGIN_DECL_9
            #define FLUVIO_PLUGIN_DECL_9 FLUVIO_BUFFER(FLUVIO_PLUGIN_DATA_RW_9) fluvio_PluginData9
        #endif
    #endif

    // Variable declaration
    int4 fluvio_Count;
	int fluvio_Stride;
    float4 fluvio_KernelSize;
    float4 fluvio_KernelFactors;
    float4 fluvio_Time;
    FLUVIO_BUFFER_RW(FluidParticle) fluvio_Particle;
	FLUVIO_BUFFER_SOLVER_RW(int) fluvio_Neighbors;
	FLUVIO_BUFFER(FluidData) fluvio_Fluid;
	FLUVIO_BUFFER_SOLVER_RW(FluidParticle) fluvio_BoundaryParticle;
	FLUVIO_BUFFER_SOLVER_RW(FluidParticle) fluvio_IndexGridBoundaryParticle;
	FLUVIO_TEXTURE(fluvio_BoundaryTexture);
	FLUVIO_SAMPLER(fluvio_BoundaryTexture);
	int4 fluvio_BoundaryTexture_Params;
	float4 fluvio_BoundaryTexture_Bounds;
	mat4x4 fluvio_BoundaryTexture_WorldToLocal;
	float4 fluvio_BoundaryTexture_UV;
	float fluvio_BoundaryTexture_Threshold;
#ifdef FLUVIO_USE_INDEX_GRID_ON
    FLUVIO_BUFFER_SOLVER_RW(uint) fluvio_IndexGrid;
#endif

    #ifdef FLUVIO_PLUGIN
        int fluvio_IncludeFluidGroup;
        int fluvio_PluginFluidID;
        FLUVIO_PLUGIN_DECL_0;
        FLUVIO_PLUGIN_DECL_1;
        FLUVIO_PLUGIN_DECL_2;
        FLUVIO_PLUGIN_DECL_3;
        FLUVIO_PLUGIN_DECL_4;
        FLUVIO_PLUGIN_DECL_5;
        FLUVIO_PLUGIN_DECL_6;
        FLUVIO_PLUGIN_DECL_7;
        FLUVIO_PLUGIN_DECL_8;
        FLUVIO_PLUGIN_DECL_9;
    #endif

    #define _FLUVIO_KERNEL(kernelName, xSize, ySize, zSize) \
        [numthreads(xSize,ySize,zSize)]\
        void kernelName(int3 fluvio_DispatchThreadID : SV_DispatchThreadID, int3 fluvio_GroupThreadID : SV_GroupThreadID)
	#define FLUVIO_KERNEL(kernelName) _FLUVIO_KERNEL(kernelName, FLUVIO_THREAD_GROUP_SIZE, 1, 1)
    #define FLUVIO_KERNEL_INDEX_GRID(kernelName) FLUVIO_KERNEL(kernelName)
	#define FLUVIO_KERNEL_BOUNDARY_TEXTURE(kernelName) FLUVIO_KERNEL(kernelName)
#endif

// ---------------------------------------------------------------------------------------
// Helper macros
// ---------------------------------------------------------------------------------------

// Get the plugin data at the specified index
#define FluvioGetPluginBuffer(index) fluvio_PluginData##index
#define FluvioGetPluginValue(index) fluvio_PluginData##index[0]

// Macro to iterate through particle neighbors
#define EachNeighbor(particleIndex, neighborIndex) int fluvio_neighborIndexP = particleIndex*fluvio_Stride, neighborIndex = fluvio_Neighbors[fluvio_neighborIndexP]; fluvio_neighborIndexP < particleIndex*fluvio_Stride + fluvio_Particle[particleIndex].id.z; ++fluvio_neighborIndexP, neighborIndex = fluvio_Neighbors[fluvio_neighborIndexP]

// Returns true if the current particle is alive
#define FluvioParticleIsAlive(particleIndex) (fluvio_Particle[(particleIndex)].lifetime.x > 0.0f)

// Returns true if the plugin should be updated for this particle
#define FluvioShouldUpdatePlugin(particleIndex) (!FLUVIO_OUT_OF_BOUNDS(particleIndex, fluvio_Count.z) && FluvioParticleIsAlive((particleIndex)) && (fluvio_IncludeFluidGroup == 1 || fluvio_PluginFluidID == fluvio_Particle[(particleIndex)].id.x))

// Returns true if the plugin should be updated for this particle neighbor
#define FluvioShouldUpdatePluginNeighbor(neighborIndex) (FluvioParticleIsAlive((neighborIndex)) && (fluvio_IncludeFluidGroup == 1 || fluvio_PluginFluidID == fluvio_Particle[(neighborIndex)].id.x))

// ---------------------------------------------------------------------------------------
// Helper functions
// ---------------------------------------------------------------------------------------

// Gets the neighbor index at the specified offset (from 0 to stride)
inline int FluvioGetNeighborIndex(FLUVIO_BUFFER_SOLVER_RW(int) neighbors, int particleIndex, int stride, int offset)
{
    return neighbors[particleIndex * stride + offset];
}

#define FluvioPositive(x) 0.5f*(x + max(x,-x))
#define FluvioRenorm(x, upper, lower) 1.0f-FluvioPositive(1.0f-FluvioPositive((x-lower)/(upper-lower)))

// Performs a mathematical branch:
//      if (x > y) return a;
//      else return b;
// Assumes y is greater than 0.001.
inline float FluvioRenormBranch(float x, float y, float a, float b)
{
    return lerp(b, a, FluvioRenorm(x, y + 0.001f, y - 0.001f));
}

// Clamp length
inline float3 clamp_len(float3 v, float len)
{
    return (dot(v, v) > (len * len)) ? (normalize(v) * len) : v;
}

// Inverse lerp
inline float invlerp(float from, float to, float value)
{
    if (from < to)
    {
        if (value < from)
            return 0.0f;
        if (value > to)
            return 1.0f;
        value -= from;
        value /= to - from;
        return value;
    }
    else
    {
        if (from <= to)
            return 0.0f;
        if (value < to)
            return 1.0f;
        if (value > from)
            return 0.0f;
        else
            return 1.0 - (value - to) / (from - to);
    }
}
inline float2 invlerp2(float2 from, float2 to, float2 value)
{
	return 1 - (value - to) / (from - to);
}
inline float4 invlerp4(float4 from, float4 to, float4 value)
{
	return 1 - (value - to) / (from - to);
}

// Random numbers
inline uint WangHash(uint seed)
{
    seed = (seed ^ 61) ^ (seed >> 16);
    seed *= 9;
    seed = seed ^ (seed >> 4);
    seed *= 0x27d4eb2d;
    seed = seed ^ (seed >> 15);
    return seed;
}

inline float RandomFloat(uint seed)
{
    float f = (float)WangHash(seed);
    return f * (1.0f / 4294967295.0f); // 2^32 - 1
}


// Animation curve evaluation
inline float DoEvaluateCurve(float t, Keyframe keyframe0, Keyframe keyframe1)
{
    float dt = keyframe1.time - keyframe0.time;

    float m0 = keyframe0.outTangent * dt;
    float m1 = keyframe1.inTangent * dt;

    float t2 = t * t;
    float t3 = t2 * t;

    float a = 2 * t3 - 3 * t2 + 1;
    float b = t3 - 2 * t2 + t;
    float c = t3 - t2;
    float d = -2 * t3 + 3 * t2;

    return a * keyframe0.value + b * m0 + c * m1 + d * keyframe1.value;
}

inline float EvaluateCurve(FLUVIO_BUFFER_ARG(Keyframe) curve, int startIndex, int endIndex, float time)
{
    Keyframe start, end;

    for(int i = startIndex; i < endIndex; ++i)
    {
        start = curve[i];
        end = curve[i+1];

        if (time < end.time)
            break;
    }

    float t = invlerp(start.time, end.time, time);
    return DoEvaluateCurve(t, start, end);
}

inline float EvaluateMinMaxCurve(FLUVIO_BUFFER_ARG(Keyframe) curve, float time, uint seed)
{
    Keyframe curveInfo = curve[0];

    int maxCurveStartIndex = 1;
    int maxCurveEndIndex = (int)curveInfo.time;
    int minCurveStartIndex = (int)curveInfo.value;
    int minCurveEndIndex = curveInfo.inTangent;

    float scalar = curveInfo.outTangent;

    float maxCurve = EvaluateCurve(curve, maxCurveStartIndex, maxCurveEndIndex, time);
    float minCurve = EvaluateCurve(curve, minCurveStartIndex, minCurveEndIndex, time);

    return scalar * lerp(minCurve, maxCurve, RandomFloat(seed));
}

// Gradient evaluation
inline float4 DoEvaluateGradient(float cT, float aT, GradientColorKey colorKey0, GradientColorKey colorKey1, GradientAlphaKey alphaKey0, GradientAlphaKey alphaKey1)
{
    float3 col0 = FLUVIO_INITIALIZE(float3);
    float3 col1 = FLUVIO_INITIALIZE(float3);

    col0.x = colorKey0.cR;
    col0.y = colorKey0.cG;
    col0.z = colorKey0.cB;

    col1.x = colorKey1.cR;
    col1.y = colorKey1.cG;
    col1.z = colorKey1.cB;

    float4 c = FLUVIO_INITIALIZE(float4);
    c.xyz = lerp3(col0, col1, cT);
    c.w = lerp(alphaKey0.alpha, alphaKey1.alpha, aT);
    return c;
}

inline float4 EvaluateGradient(FLUVIO_BUFFER_ARG(GradientColorKey) colorKeys, FLUVIO_BUFFER_ARG(GradientAlphaKey) alphaKeys, int colorKeyStartIndex, int colorKeyEndIndex, int alphaKeyStartIndex, int alphaKeyEndIndex, float time)
{
    GradientColorKey startColor, endColor;
    GradientAlphaKey startAlpha, endAlpha;

    for(int i = colorKeyStartIndex; i < colorKeyEndIndex; ++i)
    {
        startColor = colorKeys[i];
        endColor = colorKeys[i+1];

        if (time < endColor.time)
            break;
    }

    if (alphaKeyEndIndex - alphaKeyStartIndex == 1)
    {
        startAlpha = alphaKeys[alphaKeyStartIndex];
        endAlpha = alphaKeys[alphaKeyStartIndex+1];
    }
    else
    {
        for(int j = alphaKeyStartIndex; j < alphaKeyEndIndex; ++j)
        {
            startAlpha = alphaKeys[j];
            endAlpha = alphaKeys[j+1];

           if (time < endAlpha.time)
               break;
        }
    }

    float cT = invlerp(startColor.time, endColor.time, time);
    float aT = invlerp(startAlpha.time, endAlpha.time, time);

    return DoEvaluateGradient(cT, aT, startColor, endColor, startAlpha, endAlpha);
}

inline float4 EvaluateMinMaxGradient(FLUVIO_BUFFER_ARG(GradientColorKey) colorKeys, FLUVIO_BUFFER_ARG(GradientAlphaKey) alphaKeys, float time, uint seed)
{
    GradientColorKey colorInfo = colorKeys[0];
    GradientAlphaKey alphaInfo = alphaKeys[0];

    int maxColorKeyStartIndex = 1;
    int maxColorKeyEndIndex = colorInfo.cR;
    int minColorKeyStartIndex = colorInfo.cG;
    int minColorKeyEndIndex = colorInfo.cB;

    int maxAlphaKeyStartIndex = 1;
    int maxAlphaKeyEndIndex = colorInfo.cA;
    int minAlphaKeyStartIndex = colorInfo.time;
    int minAlphaKeyEndIndex = alphaInfo.alpha;

    float4 maxGradient = EvaluateGradient(colorKeys, alphaKeys, maxColorKeyStartIndex, maxColorKeyEndIndex, maxAlphaKeyStartIndex, maxAlphaKeyEndIndex, time);
    float4 minGradient = EvaluateGradient(colorKeys, alphaKeys, minColorKeyStartIndex, minColorKeyEndIndex, minAlphaKeyStartIndex, minAlphaKeyEndIndex, time);

    return lerp4(minGradient, maxGradient, RandomFloat(seed));
}

// ---------------------------------------------------------------------------------------
// Solver data macros
// ---------------------------------------------------------------------------------------

// The following macros will only work within the main kernel function on OpenCL.

///     Adds the specified force to the particle, using an optional force mode.
///			ForceMode.Force: f0 += f1
///			ForceMode.Acceleration: f0 += f1 * invMass
///			ForceMode.Impulse: f0 += f1 / dt
///			ForceMode.VelocityChange: f0 += (f1 * invMass) / dt
#define solverData_AddForce(particleIndex, forceAmount, forceMode) \
{ \
    float deltaTime = fluvio_Time.x; \
    switch ((forceMode)) \
    { \
        case FLUVIO_FORCE_MODE_FORCE: \
            fluvio_Particle[(particleIndex)].force += (forceAmount); \
            break; \
        case FLUVIO_FORCE_MODE_ACCELERATION: \
            fluvio_Particle[(particleIndex)].force += (forceAmount) * (1.0f / fluvio_Fluid[fluvio_Particle[(particleIndex)].id.x].particleMass); \
            break; \
        case FLUVIO_FORCE_MODE_IMPULSE: \
            fluvio_Particle[(particleIndex)].force += (forceAmount) / deltaTime; \
            break; \
        case FLUVIO_FORCE_MODE_VELOCITY_CHANGE: \
            fluvio_Particle[(particleIndex)].force += ((forceAmount) * (1.0f / fluvio_Fluid[fluvio_Particle[(particleIndex)].id.x].particleMass)) / deltaTime; \
            break; \
    } \
}

///     Gets the color of the particle at the specified index in the solver data.
#define solverData_GetColor(particleIndex) (fluvio_Particle[(particleIndex)].color)

///     Gets the fluid that is associated with the particle at the specified index in the solver data.
#define solverData_GetFluid(particleIndex) (fluvio_Fluid[fluvio_Particle[(particleIndex)].id.x])

///     Gets the fluid ID that is associated with the particle at the specified index in the solver data.
#define solverData_GetFluidID(particleIndex) (fluvio_Particle[(particleIndex)].id.x)

///     Gets the density of the particle at the specified index in the solver data.
#define solverData_GetDensity(particleIndex) (fluvio_Particle[(particleIndex)].densityPressure.x)

///     Gets the unapplied force of the particle at the specified index in the solver data.
#define solverData_GetForce(particleIndex) (fluvio_Particle[(particleIndex)].force)

///     Gets the mass of the particle at the specified index in the solver data.
#define solverData_GetMass(particleIndex) (fluvio_Fluid[fluvio_Particle[(particleIndex)].id.x].particleMass)

///     Gets the lifetime of the particle at the specified index in the solver data.
#define solverData_GetLifetime(particleIndex) (fluvio_Particle[(particleIndex)].lifetime.x)

///     Gets the position of the particle at the specified index in the solver data.
#define solverData_GetPosition(particleIndex) (fluvio_Particle[(particleIndex)].position)

///     Gets the pressure of the particle at the specified index in the solver data.
#define solverData_GetPressure(particleIndex) (fluvio_Particle[(particleIndex)].densityPressure.z)

///     Gets the random seed of the particle at the specified index in the solver data.
#define solverData_GetRandomSeed(particleIndex) ((uint)(fluvio_Particle[(particleIndex)].id.z))

///     Gets the size of the particle at the specified index in the solver data.
#define solverData_GetSize(particleIndex) (fluvio_Particle[(particleIndex)].velocity.w)

///     Gets the surface normal and normal length of the particle at the specified index in the solver data.
#define solverData_GetSurfaceNormal(particleIndex) (fluvio_Particle[(particleIndex)].normal)

///     Gets the turbulence probability of the particle at the specified index in the solver data.
#define solverData_GetTurbulence(particleIndex) (fluvio_Particle[(particleIndex)].vorticityTurbulence.w)

///     Gets the current velocity of the particle at the specified index in the solver data.
#define solverData_GetVelocity(particleIndex) (fluvio_Particle[(particleIndex)].velocity)

///     Gets the vorticity of the particle at the specified index in the solver data.
#define solverData_GetVorticity(particleIndex) (fluvio_Particle[(particleIndex)].vorticityTurbulence)

///     Sets the color of the particle at the specified index in the solver data.
#define solverData_SetColor(particleIndex, pColor) (fluvio_Particle[(particleIndex)].color = (pColor))

///     Sets the unapplied force of the particle at the specified index in the solver data.
#define solverData_SetForce(particleIndex, pForce) (fluvio_Particle[(particleIndex)].force = (pForce))

///     Sets the lifetime of the particle at the specified index in the solver data.
#define solverData_SetLifetime(particleIndex, pLifetime) (fluvio_Particle[(particleIndex)].lifetime.x = (pLifetime))

///     Sets the size of the particle at the specified index in the solver data.
#define solverData_SetSize(particleIndex, pSize) (fluvio_Particle[(particleIndex)].velocity.w = (pSize))

///     Sets the turbulence probability of the particle at the specified index in the solver data.
#define solverData_SetTurbulence(particleIndex, pTurbulence) (fluvio_Particle[(particleIndex)].vorticityTurbulence.w = (pTurbulence))

///     Sets the vorticity of the particle at the specified index in the solver data.
#define solverData_SetVorticity(particleIndex, pVorticity) (fluvio_Particle[(particleIndex)].vorticityTurbulence.xyz = (pVorticity.xyz))

#endif // FLUVIO_COMPUTE_INCLUDED
