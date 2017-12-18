#ifndef UNITY_STANDARD_SHADOW_INCLUDED
#define UNITY_STANDARD_SHADOW_INCLUDED
#include "UnityCG.cginc"
#include "UnityShaderVariables.cginc"
#include "UnityStandardConfig.cginc"
#if defined(_ALPHABLEND_ON) && !((SHADER_TARGET < 30) || defined (SHADER_API_MOBILE) || defined(SHADER_API_D3D11_9X) || defined (SHADER_API_PSP2) || defined (SHADER_API_PSM))
#define UNITY_STANDARD_USE_DITHER_MASK 1
#endif
#if defined(_ALPHATEST_ON) || defined(_ALPHABLEND_ON)
#define UNITY_STANDARD_USE_SHADOW_UVS 1
#endif
#if !defined(V2F_SHADOW_CASTER_NOPOS_IS_EMPTY) || defined(UNITY_STANDARD_USE_SHADOW_UVS)
#define UNITY_STANDARD_USE_SHADOW_OUTPUT_STRUCT 1
#endif
half4  _Color;half  _Cutoff2;sampler2D _MainTex;float4  _MainTex_ST;
#ifdef UNITY_STANDARD_USE_DITHER_MASK
sampler3D _DitherMaskLOD;
#endif
struct VertexInput{float4 vertex : POSITION;float3 normal : NORMAL;float2 uv0  : TEXCOORD0;};
#ifdef UNITY_STANDARD_USE_SHADOW_OUTPUT_STRUCT
struct VertexOutputShadowCaster{V2F_SHADOW_CASTER_NOPOS
#if defined(UNITY_STANDARD_USE_SHADOW_UVS)
float2 tex : TEXCOORD1;
#endif
};
#endif
void vertShadowCaster (VertexInput v,
#ifdef UNITY_STANDARD_USE_SHADOW_OUTPUT_STRUCT
out VertexOutputShadowCaster o,
#endif
out float4 opos : SV_POSITION){TRANSFER_SHADOW_CASTER_NOPOS(o, opos)
#if defined(UNITY_STANDARD_USE_SHADOW_UVS)
o.tex = TRANSFORM_TEX(v.uv0, _MainTex);
#endif
}half4 fragShadowCaster (
#ifdef UNITY_STANDARD_USE_SHADOW_OUTPUT_STRUCT
VertexOutputShadowCaster i
#endif
#ifdef UNITY_STANDARD_USE_DITHER_MASK
, UNITY_VPOS_TYPE vpos : VPOS
#endif
) : SV_Target{
#if defined(UNITY_STANDARD_USE_SHADOW_UVS)
half alpha = tex2D(_MainTex, i.tex).a;half alphaTest = _Cutoff2;clip (alpha - alphaTest);
#if defined(_ALPHABLEND_ON)
#ifdef UNITY_STANDARD_USE_DITHER_MASK
half alphaRef = tex3D(_DitherMaskLOD, float3(vpos.xy*0.25,alpha*_Color.a*0.9375)).a;clip (alphaRef - alphaTest);
#endif
#endif
#endif
SHADOW_CASTER_FRAGMENT(i)}
#endif