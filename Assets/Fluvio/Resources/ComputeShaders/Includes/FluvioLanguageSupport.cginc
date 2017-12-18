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
    inline float2 lerp2(float2 a, float2 b, float w) { return a + w*(b-a); }
    inline float3 lerp3(float3 a, float3 b, float w) { return a + w*(b-a); }
    inline float4 lerp4(float4 a, float4 b, float w) { return a + w*(b-a); }
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
	#define InterlockedAdd(dest, val) atomic_add(&dest, val)
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
