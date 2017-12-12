#include "UnityStandardCore.cginc"
#define UNITY_SHADER_NO_UPGRADE
#if UNITY_VERSION >= 540
#define _Object2World unity_ObjectToWorld
inline float4 ObjectToClipPos(in float3 pos){return UnityObjectToClipPos(pos);}
#else 
inline float4 ObjectToClipPos(in float3 pos){return mul(UNITY_MATRIX_MVP, float4(pos, 1.0));}
#endif
#if UNITY_VERSION >= 520
#define FLUVIO_FRAGMENTGI(s, occlusion, i_ambientOrLightmapUV, atten, light) FragmentGI(s, occlusion, i_ambientOrLightmapUV, atten, light)
#else 
#define FLUVIO_FRAGMENTGI(s, occlusion, i_ambientOrLightmapUV, atten, light) FragmentGI(s.posWorld, occlusion, i_ambientOrLightmapUV, atten, s.oneMinusRoughness, s.normalWorld, s.eyeVec, light)
#endif
#if UNITY_SPECCUBE_BOX_PROJECTION
#define FLUVIO_IN_WORLDPOS(i) i.posWorld
#else 
#define FLUVIO_IN_WORLDPOS(i) half3(0,0,0)
#endif
#define FLUVIO_FRAGMENT_SETUP(x) FragmentCommonData x = FluvioFragmentSetup(i.tex, i.eyeVec, WorldNormal(i.tangentToWorldAndColor), ExtractTangentToWorldPerPixel(i.tangentToWorldAndColor), FLUVIO_IN_WORLDPOS(i), vertex_color);
#define FLUVIO_FRAGMENT_SETUP_FWDADD(x) FragmentCommonData x = FluvioFragmentSetup(i.tex, i.eyeVec, WorldNormal(i.tangentToWorldAndLightDir), ExtractTangentToWorldPerPixel(i.tangentToWorldAndLightDir), half3(0,0,0), half3(0,0,0));
#ifndef FLUVIO_SETUP_BRDF_INPUT
#define FLUVIO_SETUP_BRDF_INPUT FluvioSpecularSetup
#endif
struct FluvioVertexInput{float4 vertex : POSITION;half3 normal : NORMAL;float4 color : COLOR;float2 uv0  : TEXCOORD0;
#ifdef _TANGENT_TO_WORLD
half4 tangent : TANGENT;
#endif
};float4x4 _Fluvio_FluidToObject;float3 FluvioGetWorldNormal(float3 norm){
#ifdef _OVERRIDENORMALS
norm = mul((float3x3)_Fluvio_FluidToObject, float3(0,0,-1));
#endif
return UnityObjectToWorldNormal(norm);}float4 FluvioGetWorldTangent(float4 dir){
#ifdef _OVERRIDENORMALS
dir = float4(mul((float3x3)_Fluvio_FluidToObject, float3(1,0,0)), -1);
#endif
return float4(UnityObjectToWorldDir(dir.xyz), dir.w);}half FluvioAlpha(float4 texcoords){return tex2D(_MainTex, texcoords).a * texcoords.a;}half3 FluvioEmission(float2 uv, float3 color){
#ifndef _EMISSIONMAP
return _EmissionColor.rgb * color;
#else 
return tex2D(_EmissionMap, uv).rgb * _EmissionColor.rgb * color;
#endif
}half3 FluvioAlbedo(float2 texcoords, float3 color){return _Color.rgb * tex2D (_MainTex, texcoords).rgb * color;}float2 FluvioTexCoords(FluvioVertexInput v){return TRANSFORM_TEX(v.uv0, _MainTex);}half4 FluvioOutputForward (half3 color, half alpha){return half4(color, alpha);}inline FragmentCommonData FluvioSpecularSetup (float4 i_tex, half3 vertex_color){
#ifdef _VERTEXCOLORMODE_ALBEDO
half3 albedo = FluvioAlbedo(i_tex, vertex_color);
#else 
half3 albedo = FluvioAlbedo(i_tex, float3(1,1,1));
#endif
half4 specGloss = SpecularGloss(i_tex);
#ifdef _VERTEXCOLORMODE_SPECULAR
half3 specColor = specGloss.rgb * vertex_color;
#else 
half3 specColor = specGloss.rgb;
#endif
half oneMinusRoughness = specGloss.a;half oneMinusReflectivity;half3 diffColor = EnergyConservationBetweenDiffuseAndSpecular (albedo, specColor,  oneMinusReflectivity);FragmentCommonData o = (FragmentCommonData)0;o.diffColor = diffColor;o.specColor = specColor;o.oneMinusReflectivity = oneMinusReflectivity;
#if UNITY_VERSION >= 550
o.smoothness = oneMinusRoughness;
#else 
o.oneMinusRoughness = oneMinusRoughness;
#endif
return o;}inline FragmentCommonData FluvioMetallicSetup(float4 i_tex, half3 vertex_color){
#ifdef _VERTEXCOLORMODE_ALBEDO
half3 albedo = FluvioAlbedo(i_tex, vertex_color);
#else 
half3 albedo = FluvioAlbedo(i_tex, float3(1, 1, 1));
#endif
half2 metallicGloss = MetallicGloss(i_tex);half metallic = metallicGloss.x;half oneMinusRoughness = metallicGloss.y;half oneMinusReflectivity;half3 specColor;half3 diffColor = DiffuseAndSpecularFromMetallic(albedo, metallic,  specColor,  oneMinusReflectivity);
#ifdef _VERTEXCOLORMODE_SPECULAR
specColor *= vertex_color;
#endif
FragmentCommonData o = (FragmentCommonData)0;o.diffColor = diffColor;o.specColor = specColor;o.oneMinusReflectivity = oneMinusReflectivity;
#if UNITY_VERSION >= 550
o.smoothness = oneMinusRoughness;
#else 
o.oneMinusRoughness = oneMinusRoughness;
#endif
return o;}
#ifdef _NORMALMAP
half3 FluvioNormalInTangentSpace(float2 texcoords){return UnpackScaleNormal(tex2D (_BumpMap, texcoords), _BumpScale);}
#endif
inline FragmentCommonData FluvioFragmentSetup (float4 i_tex, half3 i_eyeVec, half3 i_normalWorld, half3x3 i_tanToWorld, half3 i_posWorld, half3 vertex_color){half alpha = FluvioAlpha(i_tex);
#ifdef _NORMALMAP
half3 normalWorld = NormalizePerPixelNormal(mul(FluvioNormalInTangentSpace(i_tex), i_tanToWorld));
#else 
half3 normalWorld = i_normalWorld;
#endif
half3 eyeVec = i_eyeVec;eyeVec = NormalizePerPixelNormal(eyeVec);FragmentCommonData o = FLUVIO_SETUP_BRDF_INPUT(i_tex, vertex_color);o.normalWorld = normalWorld;o.eyeVec = eyeVec;o.posWorld = i_posWorld;o.alpha = alpha;return o;}inline float invlerp(float from, float to, float value){return (value - from) / (to - from);}struct FluvioVertexOutputForwardBase{float4 pos       : SV_POSITION;float4 tex       : TEXCOORD0;half3 eyeVec       : TEXCOORD1;half4 tangentToWorldAndColor[3]  : TEXCOORD2;half4 ambientOrLightmapUV   : TEXCOORD5;SHADOW_COORDS(6)UNITY_FOG_COORDS(7)
#if UNITY_SPECCUBE_BOX_PROJECTION
float3 posWorld     : TEXCOORD8;
#endif
};FluvioVertexOutputForwardBase FluvioVertForwardBase (FluvioVertexInput v){FluvioVertexOutputForwardBase o;UNITY_INITIALIZE_OUTPUT(FluvioVertexOutputForwardBase, o);float4 posWorld = mul(_Object2World, v.vertex);
#if UNITY_SPECCUBE_BOX_PROJECTION
o.posWorld = posWorld.xyz;
#endif
o.pos = ObjectToClipPos(v.vertex);o.tex.xy = FluvioTexCoords(v);o.eyeVec = NormalizePerVertexNormal(posWorld - _WorldSpaceCameraPos);float3 normalWorld = FluvioGetWorldNormal(v.normal);
#ifdef _TANGENT_TO_WORLD
float4 tangentWorld = FluvioGetWorldTangent(v.tangent);float3x3 tangentToWorld = CreateTangentToWorldPerVertex(normalWorld, tangentWorld.xyz, tangentWorld.w);o.tangentToWorldAndColor[0].xyz = tangentToWorld[0];o.tangentToWorldAndColor[1].xyz = tangentToWorld[1];o.tangentToWorldAndColor[2].xyz = tangentToWorld[2];
#else 
o.tangentToWorldAndColor[0].xyz = 0;o.tangentToWorldAndColor[1].xyz = 0;o.tangentToWorldAndColor[2].xyz = normalWorld;
#endif
#ifdef _VERTEXCOLORMODE_NONE
o.tangentToWorldAndColor[0].w = 1;o.tangentToWorldAndColor[1].w = 1;o.tangentToWorldAndColor[2].w = 1;o.tex.w = 1;
#else 
o.tangentToWorldAndColor[0].w = v.color.r;o.tangentToWorldAndColor[1].w = v.color.g;o.tangentToWorldAndColor[2].w = v.color.b;o.tex.w = v.color.a;
#endif
#if UNITY_SHOULD_SAMPLE_SH
#if UNITY_SAMPLE_FULL_SH_PER_PIXEL
o.ambientOrLightmapUV.rgb = 0;
#elif (SHADER_TARGET < 30)
o.ambientOrLightmapUV.rgb = ShadeSH9(half4(normalWorld, 1.0));
#else 
o.ambientOrLightmapUV.rgb = ShadeSH3Order(half4(normalWorld, 1.0));
#endif
#ifdef VERTEXLIGHT_ON
o.ambientOrLightmapUV.rgb += Shade4PointLights (unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,unity_LightColor[0].rgb, unity_LightColor[1].rgb, unity_LightColor[2].rgb, unity_LightColor[3].rgb,unity_4LightAtten0, posWorld, normalWorld);
#endif
#endif
TRANSFER_SHADOW(o);UNITY_TRANSFER_FOG(o,o.pos);return o;}half4 FluvioFragForwardBase (FluvioVertexOutputForwardBase i) : SV_Target{half3 vertex_color = half3(i.tangentToWorldAndColor[0].w, i.tangentToWorldAndColor[1].w, i.tangentToWorldAndColor[2].w);FLUVIO_FRAGMENT_SETUP(s)
#if UNITY_VERSION >= 550
UnityLight mainLight = MainLight ();
#else 
UnityLight mainLight = MainLight (s.normalWorld);
#endif
half atten = SHADOW_ATTENUATION(i);half occlusion = 1;UnityGI gi = FLUVIO_FRAGMENTGI(s, occlusion, i.ambientOrLightmapUV, atten, mainLight);
#if UNITY_VERSION >= 550
half smoothness = s.smoothness;
#else 
half smoothness = s.oneMinusRoughness;
#endif
half4 c = UNITY_BRDF_PBS(s.diffColor, s.specColor, s.oneMinusReflectivity, smoothness, s.normalWorld, -s.eyeVec, gi.light, gi.indirect);c.rgb += UNITY_BRDF_GI (s.diffColor, s.specColor, s.oneMinusReflectivity, smoothness, s.normalWorld, -s.eyeVec, occlusion, gi);
#ifdef _VERTEXCOLORMODE_EMISSION
c.rgb += FluvioEmission(i.tex, vertex_color);
#else 
c.rgb += FluvioEmission(i.tex, float3(1,1,1));
#endif
UNITY_APPLY_FOG(i.fogCoord, c.rgb);return FluvioOutputForward (c, s.alpha);}struct FluvioVertexOutputForwardAdd{float4 pos       : SV_POSITION;float4 tex       : TEXCOORD0;half3 eyeVec       : TEXCOORD1;half4 tangentToWorldAndLightDir[3] : TEXCOORD2;LIGHTING_COORDS(5,6)UNITY_FOG_COORDS(7)};FluvioVertexOutputForwardAdd FluvioVertForwardAdd (FluvioVertexInput v){FluvioVertexOutputForwardAdd o;UNITY_INITIALIZE_OUTPUT(FluvioVertexOutputForwardAdd, o);float4 posWorld = mul(_Object2World, v.vertex);o.pos = ObjectToClipPos(v.vertex);o.tex.xy = FluvioTexCoords(v);o.eyeVec = NormalizePerVertexNormal(posWorld.xyz - _WorldSpaceCameraPos);float3 normalWorld = FluvioGetWorldNormal(v.normal);
#ifdef _TANGENT_TO_WORLD
float4 tangentWorld = FluvioGetWorldTangent(v.tangent);float3x3 tangentToWorld = CreateTangentToWorldPerVertex(normalWorld, tangentWorld.xyz, tangentWorld.w);o.tangentToWorldAndLightDir[0].xyz = tangentToWorld[0];o.tangentToWorldAndLightDir[1].xyz = tangentToWorld[1];o.tangentToWorldAndLightDir[2].xyz = tangentToWorld[2];
#else 
o.tangentToWorldAndLightDir[0].xyz = 0;o.tangentToWorldAndLightDir[1].xyz = 0;o.tangentToWorldAndLightDir[2].xyz = normalWorld;
#endif
TRANSFER_VERTEX_TO_FRAGMENT(o);float3 lightDir = _WorldSpaceLightPos0.xyz - posWorld.xyz * _WorldSpaceLightPos0.w;
#ifndef USING_DIRECTIONAL_LIGHT
lightDir = NormalizePerVertexNormal(lightDir);
#endif
o.tangentToWorldAndLightDir[0].w = lightDir.x;o.tangentToWorldAndLightDir[1].w = lightDir.y;o.tangentToWorldAndLightDir[2].w = lightDir.z;UNITY_TRANSFER_FOG(o,o.pos);return o;}half4 FluvioFragForwardAdd (FluvioVertexOutputForwardAdd i) : SV_Target{FLUVIO_FRAGMENT_SETUP_FWDADD(s)
#if UNITY_VERSION >= 550
half smoothness = s.smoothness;UnityLight light = AdditiveLight (IN_LIGHTDIR_FWDADD(i), LIGHT_ATTENUATION(i));
#else 
half smoothness = s.oneMinusRoughness;UnityLight light = AdditiveLight (s.normalWorld, IN_LIGHTDIR_FWDADD(i), LIGHT_ATTENUATION(i));
#endif
UnityIndirect noIndirect = ZeroIndirect ();half4 c = UNITY_BRDF_PBS (s.diffColor, s.specColor, s.oneMinusReflectivity, smoothness, s.normalWorld, -s.eyeVec, light, noIndirect);UNITY_APPLY_FOG_COLOR(i.fogCoord, c.rgb, half4(0,0,0,0));return FluvioOutputForward (c, s.alpha);}struct FluvioVertexOutputDeferred{float4 pos       : SV_POSITION;float4 tex       : TEXCOORD0;half3 eyeVec       : TEXCOORD1;half4 tangentToWorldAndColor[3]  : TEXCOORD2;half4 ambientOrLightmapUV   : TEXCOORD5;
#if UNITY_SPECCUBE_BOX_PROJECTION
float3 posWorld     : TEXCOORD6;
#endif
};FluvioVertexOutputDeferred FluvioVertDeferred (FluvioVertexInput v){FluvioVertexOutputDeferred o;UNITY_INITIALIZE_OUTPUT(FluvioVertexOutputDeferred, o);float4 posWorld = mul(_Object2World, v.vertex);
#if UNITY_SPECCUBE_BOX_PROJECTION
o.posWorld = posWorld;
#endif
o.pos = ObjectToClipPos(v.vertex);o.tex.xy = FluvioTexCoords(v);o.eyeVec = NormalizePerVertexNormal(posWorld.xyz - _WorldSpaceCameraPos);float3 normalWorld = FluvioGetWorldNormal(v.normal);
#ifdef _TANGENT_TO_WORLD
float4 tangentWorld = FluvioGetWorldTangent(v.tangent);float3x3 tangentToWorld = CreateTangentToWorldPerVertex(normalWorld, tangentWorld.xyz, tangentWorld.w);o.tangentToWorldAndColor[0].xyz = tangentToWorld[0];o.tangentToWorldAndColor[1].xyz = tangentToWorld[1];o.tangentToWorldAndColor[2].xyz = tangentToWorld[2];
#else 
o.tangentToWorldAndColor[0].xyz = 0;o.tangentToWorldAndColor[1].xyz = 0;o.tangentToWorldAndColor[2].xyz = normalWorld;
#endif
#ifdef _VERTEXCOLORMODE_NONE
o.tangentToWorldAndColor[0].w = 1;o.tangentToWorldAndColor[1].w = 1;o.tangentToWorldAndColor[2].w = 1;o.tex.w = 1;
#else 
o.tangentToWorldAndColor[0].w = v.color.r;o.tangentToWorldAndColor[1].w = v.color.g;o.tangentToWorldAndColor[2].w = v.color.b;o.tex.w = v.color.a;
#endif
#if UNITY_SHOULD_SAMPLE_SH
#if (SHADER_TARGET < 30)
o.ambientOrLightmapUV.rgb = ShadeSH9(half4(normalWorld, 1.0));
#else 
o.ambientOrLightmapUV.rgb = ShadeSH3Order(half4(normalWorld, 1.0));
#endif
#endif
return o;}void FluvioFragDeferred (FluvioVertexOutputDeferred i,out half4 outDiffuse : SV_Target0,out half4 outSpecSmoothness : SV_Target1,out half4 outNormal : SV_Target2,out half4 outEmission : SV_Target3){
#if (SHADER_TARGET < 30)
outDiffuse = 1;outSpecSmoothness = 1;outNormal = 0;outEmission = 0;return;
#endif
half3 vertex_color = half3(i.tangentToWorldAndColor[0].w, i.tangentToWorldAndColor[1].w, i.tangentToWorldAndColor[2].w);FLUVIO_FRAGMENT_SETUP(s)half alpha = FluvioAlpha(i.tex);clip(alpha - _Cutoff);
#if UNITY_VERSION >= 550
UnityLight dummyLight = DummyLight ();half smoothness = s.smoothness;
#else 
UnityLight dummyLight = DummyLight (s.normalWorld);half smoothness = s.oneMinusRoughness;
#endif
half atten = 1;half occlusion = 1;UnityGI gi = FLUVIO_FRAGMENTGI(s, occlusion, i.ambientOrLightmapUV, atten, dummyLight);half3 color = UNITY_BRDF_PBS (s.diffColor, s.specColor, s.oneMinusReflectivity, smoothness, s.normalWorld, -s.eyeVec, gi.light, gi.indirect).rgb;color += UNITY_BRDF_GI (s.diffColor, s.specColor, s.oneMinusReflectivity, smoothness, s.normalWorld, -s.eyeVec, occlusion, gi);
#ifdef _VERTEXCOLORMODE_EMISSION
color += FluvioEmission(i.tex, vertex_color);
#else 
color += FluvioEmission(i.tex, float3(1,1,1));
#endif
#ifndef UNITY_HDR_ON
color.rgb = exp2(-color.rgb);
#endif
outDiffuse = half4(s.diffColor * alpha, 1);outSpecSmoothness = half4(s.specColor * alpha, smoothness);outNormal = half4(s.normalWorld,1);outEmission = half4(color, 1);}