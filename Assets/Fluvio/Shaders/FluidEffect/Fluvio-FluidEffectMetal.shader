Shader "Fluvio/Fluid Effect"
{
    Properties
    {
        // Diffuse/alpha
        _Color ("Color", Color) = (0.75,0.75,0.75,0.5)
        _MainTex ("Albedo", 2D) = "white" {}
        _Cutoff ("Alpha Cutoff", Range(0.001,1)) = 0.5
        _Cutoff2 ("Shadow/Depth Alpha Cutoff", Range(0.001, 1.0)) = 0.15
        _InvFade("Soft Particles Factor", Range(0.01,3.0)) = 1.0

        // Smoothness/metallic
        _Glossiness ("Smoothness", Range(0.0, 1.0)) = 0.5
        [Gamma] _Metallic ("Metallic", Range(0.0, 1.0)) = 0.8
        _MetallicGlossMap ("Metallic", 2D) = "white" {}

        // Normal
        _BumpScale ("Scale", Float) = 0.5
        _BumpMap ("Normal Map", 2D) = "bump" {}

        // Emission
        _EmissionColor ("Color", Color) = (0,0,0,0)
        _EmissionMap ("Emission", 2D) = "white" {}

        // Vertex colors
        [KeywordEnum(None, Albedo, Specular, Emission)] _VertexColorMode ("Vertex color mode", Float) = 1

        // UI-only data
        [HideInInspector] _EmissionScaleUI ("Scale", Float) = 0.0
        [HideInInspector] _EmissionColorUI ("Color", Color) = (0,0,0,0)

        // Image effect (passed to composite shader)
        [HideInInspector] _DownsampleFactorUI ("Downsample Factor", Float) = 1.0
        [HideInInspector] _FluidBlurUI ("Blur Fluid", Range(0,1)) = 0.25
        [HideInInspector] _FluidBlurNormalsUI("Blur Normals", Range(0,1)) = 0.125
        [HideInInspector] _FluidBlurBackgroundUI ("Blur Background", Range(0,1)) = 0
        [HideInInspector] _FluidRefractionUI("Refraction", Range(1.0, 5.0)) = 1.3
        [HideInInspector] _FluidSpecularScaleUI ("Fake Specular Effect", Range(0.0, 1.0)) = 0
        [HideInInspector] _FluidTintUI ("Composite Tint Color", Color) = (1,1,1,1)
    }

    CGINCLUDE
        #define _GLOSSYENV 1
        #define FLUVIO_SETUP_BRDF_INPUT FluvioMetallicSetup
    ENDCG

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Fluvio" "PerformanceChecks"="False" }

        // ------------------------------------------------------------------
        //  Depth pass
        Pass
        {
            ZWrite On ZTest LEqual ColorMask 0

            CGPROGRAM
                #pragma multi_compile _VERTEXCOLORMODE_NONE _VERTEXCOLORMODE_ALBEDO _VERTEXCOLORMODE_SPECULAR _VERTEXCOLORMODE_EMISSION
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
                    #ifndef _VERTEXCOLORMODE_NONE
                    float4 color	: COLOR;
                    #endif
                    float2 texcoord : TEXCOORD0;
                };

                struct v2f
                {
                    float4 vertex   : SV_POSITION;
                    #ifndef _VERTEXCOLORMODE_NONE
                    float4 color	: COLOR;
                    #endif
                    half2 texcoord  : TEXCOORD0;
                };

                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.vertex = ObjectToClipPos(v.vertex);
                    #ifndef _VERTEXCOLORMODE_NONE
                    o.color = v.color;
                    #endif
                    o.texcoord = v.texcoord;
                    return o;
                }

                fixed4 frag(v2f i) : COLOR
                {
                    fixed c = tex2D(_MainTex, i.texcoord).a * _Color.a;
                    #ifndef _VERTEXCOLORMODE_NONE
                    c *= i.color.a;
                    #endif
                    clip(c - _Cutoff2);
                    return c;
                }
            ENDCG

        }
        // ------------------------------------------------------------------
        //  Shadow rendering pass
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            ZWrite On ZTest LEqual

            CGPROGRAM
            #pragma target 3.0
            // TEMPORARY: GLES2.0 temporarily disabled to prevent errors spam on devices without textureCubeLodEXT
            #pragma exclude_renderers gles

            // -------------------------------------

            #pragma multi_compile _VERTEXCOLORMODE_NONE _VERTEXCOLORMODE_ALBEDO _VERTEXCOLORMODE_SPECULAR _VERTEXCOLORMODE_EMISSION

            #pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON
            #pragma multi_compile_shadowcaster

            #pragma vertex vertShadowCaster
            #pragma fragment fragShadowCaster

            #include "FluidEffectShadow.cginc"

            ENDCG
        }
    }
    CustomEditor "Thinksquirrel.FluvioEditor.Inspectors.FluidEffectShaderInspector"
}

