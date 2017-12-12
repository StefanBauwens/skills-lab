// ---------------------------------------------------------------------------------------
// Custom plugin properties
// ---------------------------------------------------------------------------------------


// Holds output data for emitting particles
typedef struct
{
	float4 emitPosition; // xyz - position, w[0] - mixing distance squared
	float4 emitVelocity; // xyz - velocity, w - particle system
	int fluidB; // only index 0
	int fluidC; // only index 0
	int fluidD; // only index 0
	int mixingFluidsAreTheSame; // only index 0
} FluidMixerData;

#define FLUVIO_PLUGIN_DATA_RW_0 FluidMixerData

// ---------------------------------------------------------------------------------------
// Main include
// ---------------------------------------------------------------------------------------

#include "../../Includes/FluvioCompute.cginc"

// ---------------------------------------------------------------------------------------
// Main plugin
// ---------------------------------------------------------------------------------------

FLUVIO_KERNEL(OnUpdatePlugin)
{
    int particleIndex = get_global_id(0);

    if (!FluvioShouldUpdatePlugin(particleIndex)) return;
    
    for(EachNeighbor(particleIndex, neighborIndex))
    {
        if (!FluvioShouldUpdatePluginNeighbor(neighborIndex)) continue;

		FLUVIO_BUFFER_RW(FluidMixerData) mixerData = FluvioGetPluginBuffer(0);

        int fluid = fluvio_PluginFluidID;
        int fluidB = mixerData[0].fluidB;
        int fluidC = mixerData[0].fluidC;
        int fluidD = mixerData[0].fluidD;
        int mixingFluidsAreTheSame = mixerData[0].mixingFluidsAreTheSame;
        float mixingDistanceSq = mixerData[0].emitPosition.w;
        
        int particleFluid = solverData_GetFluidID(particleIndex);
        int neighborFluid = solverData_GetFluidID(neighborIndex);
     
        bool particleIsA = particleFluid == fluid;
            
        if ((mixingFluidsAreTheSame == 1 || particleFluid != neighborFluid) && // Fluids are not the same, unless A and B are the same
            (particleIsA || particleFluid == fluidB) && // First fluid is A or B
            (neighborFluid == fluid || neighborFluid == fluidB)) // Second fluid is A or B
        {

            float4 position = solverData_GetPosition(particleIndex);
            float4 neighborPosition = solverData_GetPosition(neighborIndex);
            float4 dist = position - neighborPosition;

            // Make sure the particles are actually close
            if (dot(dist,dist) > mixingDistanceSq)
                return;

            float4 velocity = solverData_GetVelocity(particleIndex);
            float4 invMass = 1.0f/solverData_GetMass(particleIndex);

            bool emitC = false;
            bool particleIsA = fluid == particleFluid;

            // Despawn the current particle, unless Fluid C is null and the particle is from Fluid A
            if (fluidC != 0 || !particleIsA)
            {
                emitC = true;
                solverData_SetLifetime(particleIndex, 0.0f);
            }

            // Emit a particle. We don't emit from the neighbor position, that gets handled in the opposite pair
            // We handle actual emission on the main thread.
            if (fluidD == 0 || particleIsA)
            {
                if (emitC)
                {
                    // Set the system to Fluid C
                    mixerData[particleIndex].emitVelocity.w = fluidC;
                }
                else
                {
                    mixerData[particleIndex].emitVelocity.w = 0;
                }
            }
            else
            {
                // Set the system to Fluid D
                mixerData[particleIndex].emitVelocity.w = fluidD;
            }

            // Set emit position and velocity. These must be manually integrated since they won't be applied until after the solver has run.
            // The below is a standard Euler explicit integrator.
            float4 acceleration = solverData_GetFluid(fluid).gravity + solverData_GetForce(particleIndex)*invMass;
            float dtIter = fluvio_Time.y;
            int iterations = fluvio_Time.w;

            for (int iter = 0; iter < iterations; ++iter)
            {
                float4 t = dtIter*acceleration;
                    
                // Ignore very large velocity changes
                if (any(isnan(t) || isinf(t) || dot(t,t) > FLUVIO_MAX_SQR_VELOCITY_CHANGE * fluvio_KernelSize.w * fluvio_KernelSize.w))
				{
                    t = 0;
			    }
                    
                velocity += t;
            }

			mixerData[particleIndex].emitVelocity.xyz = velocity.xyz;
			mixerData[particleIndex].emitPosition.xyz = position.xyz + velocity.xyz * fluvio_Time.x;
        }
    }
}
