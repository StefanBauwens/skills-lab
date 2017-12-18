Shader "Fluvio/Fluid Effect (Unlit)"
{
    Properties
    {
        // Color/alpha
        _Color ("Color", Color) = (0.75,0.75,0.75,0.5)
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
        _Cutoff ("Alpha Cutoff", Range(0.001,1)) = 0.5
        _Cutoff2 ("Depth Alpha Cutoff", Range(0.001, 1.0)) = 0.15
        _InvFade("Soft Particles Factor", Range(0.01,3.0)) = 1.0

        // UI-only data
        [HideInInspector] _EmissionScaleUI ("Scale", Float) = 0.0
        [HideInInspector] _EmissionColorUI ("Color", Color) = (0,0,0,0)

        // Image effect (passed to composite shader)
        _DownsampleFactorUI ("Downsample Factor", Float) = 1.0
        _FluidBlurUI ("Blur Fluid", Range(0,1)) = 0.25
        _FluidBlurBackgroundUI ("Blur Background", Range(0,1)) = 0
        _FluidRefractionUI("Refraction", Range(1.0, 5.0)) = 1.3
        _FluidSpecularScaleUI ("Fake Specular Effect", Range(0.0, 1.0)) = 0
        _FluidTintUI ("Composite Tint Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Fluvio" "PerformanceChecks"="False" }

        // ------------------------------------------------------------------
        //  Depth pass
        Pass
        {
            ZWrite On ZTest LEqual ColorMask 0

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
                float _Cutoff2;

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

                fixed4 frag(v2f i) : COLOR
                {
                    fixed c = tex2D(_MainTex, i.texcoord).a * _Color.a;
                    c *= i.color.a;
                    clip(c - _Cutoff2);
                    return c;
                }
            ENDCG

        }
    }
    //CustomEditor "Thinksquirrel.FluvioEditor.Inspectors.FluidEffectShaderInspector"
}

