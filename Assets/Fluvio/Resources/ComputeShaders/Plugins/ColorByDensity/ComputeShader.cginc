#include "../../Includes/FluvioLanguageSupport.cginc"
#define FLUVIO_PLUGIN_DATA_0 GradientColorKey
#define FLUVIO_PLUGIN_DATA_1 GradientAlphaKey
#define FLUVIO_PLUGIN_DATA_2 float4
#include "../../Includes/FluvioCompute.cginc"
FLUVIO_KERNEL(OnUpdatePlugin){int particleIndex = get_global_id(0);if (FluvioShouldUpdatePlugin(particleIndex)){FLUVIO_BUFFER(GradientColorKey) colorKeys = FluvioGetPluginBuffer(0);FLUVIO_BUFFER(GradientAlphaKey) alphaKeys = FluvioGetPluginBuffer(1);float4 rangeSmoothing = FluvioGetPluginValue(2);uint seed = particleIndex;float density = solverData_GetDensity(particleIndex);float d = invlerp(rangeSmoothing.x, rangeSmoothing.y, density);solverData_SetColor(particleIndex, lerp4(solverData_GetColor(particleIndex), EvaluateMinMaxGradient(colorKeys, alphaKeys, d, seed), rangeSmoothing.z));}}