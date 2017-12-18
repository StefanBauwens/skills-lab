Shader "Hidden/Fluvio/FluidEffectReplaceUnlit"
{
    Properties
    {
        // Color/alpha
        _Color ("Color", Color) = (0.75,0.75,0.75,0.5)
        _MainTex ("Albedo", 2D) = "white" {}
        _Cutoff ("Alpha Cutoff", Range(0.001,1)) = 0.5
        _Cutoff2 ("Depth Alpha Cutoff", Range(0.001, 1.0)) = 0.15
        _InvFade("Soft Particles Factor", Range(0.01,3.0)) = 1.0
    }

    SubShader
    {
        Tags { "Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="Fluvio" "PerformanceChecks"="False" }

        // ------------------------------------------------------------------
        //  Simple pass
        Pass
        {
			Name "FORWARD"
			Tags{ "LightMode" = "ForwardBase" }

            Blend SrcAlpha OneMinusSrcAlpha, One One
			ZWrite Off ZTest LEqual Cull Off


            CGPROGRAM
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

                fixed4 _Color;
                sampler2D _MainTex;

                struct appdata_t
                {
                    float4 vertex   : POSITION;
                    float4 color	: COLOR;
                    float2 texcoord : TEXCOORD0;
                };

                struct v2f
                {
                    float4 vertex   : SV_POSITION;
                    float4 color	: COLOR;
                    half2 texcoord  : TEXCOORD0;
                };

                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.vertex = ObjectToClipPos(v.vertex);
                    o.color = v.color;
                    o.texcoord = v.texcoord;
                    return o;
                }

                half4 frag(v2f i) : COLOR
                {
					return tex2D(_MainTex, i.texcoord) * _Color * i.color;
                }
            ENDCG
        }
    }
}