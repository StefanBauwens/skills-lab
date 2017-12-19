#include "../../Includes/FluvioLanguageSupport.cginc"
#define FLUVIO_PLUGIN_DATA_0 Keyframe
#define FLUVIO_PLUGIN_DATA_1 float4
#include "../../Includes/FluvioCompute.cginc"
FLUVIO_KERNEL(OnUpdatePlugin){int particleIndex = get_global_id(0);if (FluvioShouldUpdatePlugin(particleIndex)){FLUVIO_BUFFER(Keyframe) size = FluvioGetPluginBuffer(0);float4 rangeSmoothing = FluvioGetPluginValue(1);float density = solverData_GetDensity(particleIndex);float d = invlerp(rangeSmoothing.x, rangeSmoothing.y, density);uint seed = particleIndex;solverData_SetSize(particleIndex, max(0.0f, lerp(solverData_GetSize(particleIndex), EvaluateMinMaxCurve(size, d, seed), rangeSmoothing.z)));}}