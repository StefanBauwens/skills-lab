// ---------------------------------------------------------------------------------------
// Custom plugin properties
// ---------------------------------------------------------------------------------------

#define FLUVIO_PLUGIN_DATA_0 Keyframe // acceleration
#define FLUVIO_PLUGIN_DATA_1 float4 // buffer with xyz - touchPoints, w[0] - radius, w[1] - touch points count

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

	if (FluvioShouldUpdatePlugin(particleIndex))
    {
        FLUVIO_BUFFER(Keyframe) acceleration = FluvioGetPluginBuffer(0);
		FLUVIO_BUFFER(float4) touchPoints = FluvioGetPluginBuffer(1);
		float radius = touchPoints[0].w;
		int touchPointsCount = (int)touchPoints[1].w;
        uint seed = solverData_GetRandomSeed(particleIndex);

        for(int i = 0, l = touchPointsCount; i < l; ++i)
        {
            float4 pt = touchPoints[i];
			pt.w = 0;
            float4 worldPosition = solverData_GetPosition(particleIndex);
			worldPosition.w = 0;
            float4 d = pt - worldPosition;
            float len = length(d);
            if (len < radius) solverData_AddForce(particleIndex, (d / len) * EvaluateMinMaxCurve(acceleration, len / radius, seed), FLUVIO_FORCE_MODE_ACCELERATION);
        }
    }
}
