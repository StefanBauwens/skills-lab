Shader "Hidden/Fluvio/FluidEffectReplaceDepth"
{
    Properties
    {
        // Diffuse/alpha
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo", 2D) = "white" {}
        _Cutoff2 ("Shadow/Depth Alpha Cutoff", Range(0.001, 1.0)) = 0.15
        _InvFade("Soft Particles Factor", Range(0.01,3.0)) = 1.0

        // Normal
        _BumpScale("Scale", Float) = 1.0
        _BumpMap ("Normal Map", 2D) = "bump" {}
    }

    SubShader
    {
        Tags { "Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="Fluvio" "PerformanceChecks"="False" }

        Pass
        {
            Cull Off

            CGPROGRAM
			#pragma multi_compile _ _NORMALMAP
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            #define UNITY_SHADER_NO_UPGRADE
            #if UNITY_VERSION >= 540
            #define _Object2World unity_ObjectToWorld
                inline float4 ObjectToClipPos(in float3 pos)
                {
                    return UnityObjectToClipPos(pos);
                }
            #else
                inline float4 ObjectToClipPos(in float3 pos)
                {
                    return mul(UNITY_MATRIX_MVP, float4(pos, 1.0));
                }
            #endif

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 nz : TEXCOORD1;
                float4 color : COLOR;
            };
            uniform float4 _MainTex_ST;
            v2f vert(appdata_full v)
            {
                v2f o;
                o.pos = ObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.nz.xyz = COMPUTE_VIEW_NORMAL;
                o.nz.w = COMPUTE_DEPTH_01;
                o.color = v.color;
                return o;
            }
            uniform sampler2D _MainTex;
            uniform fixed _Cutoff2;
            uniform float4 _Color;

#if _NORMALMAP
			uniform sampler2D _BumpMap;
			uniform float _BumpScale;
#endif

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 c = tex2D(_MainTex, i.uv);
                clip(c.a - _Cutoff2);

				float3 normal = i.nz.xyz;
#if _NORMALMAP
				normal += (UnpackNormal(tex2D(_BumpMap, i.uv)) *_BumpScale);
#endif
                float depth = i.nz.w;

                return EncodeDepthNormal(depth, normal);
            }
            ENDCG
        }
    }
}
