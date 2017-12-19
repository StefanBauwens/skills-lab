#include "UnityCG.cginc"
#define UNITY_SHADER_NO_UPGRADE
#if UNITY_VERSION >= 540
#define _Object2World unity_ObjectToWorld
inline float4 ObjectToClipPos(in float3 pos){return UnityObjectToClipPos(pos);}
#else 
inline float4 ObjectToClipPos(in float3 pos){return mul(UNITY_MATRIX_MVP, float4(pos, 1.0));}
#endif
uniform sampler2D _MainTex;uniform float4 _MainTex_ST;uniform half _InvFade;
#ifdef SOFTPARTICLES_ON
sampler2D _CameraDepthTexture;
#endif
struct v2fB {float4 pos : SV_POSITION;half2 uv : TEXCOORD0;half4 uvScreen : TEXCOORD1;half4 color : COLOR0;};v2fB vertB (appdata_full v){v2fB o;o.uv.xy = TRANSFORM_TEX(v.texcoord.xy, _MainTex);o.pos = ObjectToClipPos(v.vertex);o.uvScreen = ComputeGrabScreenPos (o.pos);
#ifdef SOFTPARTICLES_ON
COMPUTE_EYEDEPTH(o.uvScreen.z);
#endif
o.color = v.color;return o;}