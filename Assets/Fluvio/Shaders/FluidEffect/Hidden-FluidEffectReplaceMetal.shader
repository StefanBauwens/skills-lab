Shader "Hidden/Fluvio/FluidEffectReplace"
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
    }

    CGINCLUDE
        #define _GLOSSYENV 1
        #define FLUVIO_SETUP_BRDF_INPUT FluvioMetallicSetup
    ENDCG

    SubShader
    {
        Tags { "Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="Fluvio" "PerformanceChecks"="False" }

        // ------------------------------------------------------------------
        // Base forward pass (directional light, emission, lightmaps, ...)
        Pass
        {
            Name "FORWARD"
            Tags { "LightMode" = "ForwardBase" }

            Blend SrcAlpha OneMinusSrcAlpha, One One
            ZWrite Off ZTest LEqual Cull Off

            CGPROGRAM
            #pragma target 3.0
            // GLES2.0 disabled to prevent errors spam on devices without textureCubeLodEXT
            #pragma exclude_renderers gles

            #pragma multi_compile _VERTEXCOLORMODE_NONE _VERTEXCOLORMODE_ALBEDO _VERTEXCOLORMODE_SPECULAR _VERTEXCOLORMODE_EMISSION
            #pragma multi_compile _ _OVERRIDENORMALS

            #pragma multi_compile _ _NORMALMAP
            #pragma multi_compile _ _EMISSIONMAP
            #pragma multi_compile _ _METALLICGLOSSMAP

            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog

            #pragma vertex FluvioVertForwardBase
            #pragma fragment FluvioFragForwardBase

            #include "FluidEffect.cginc"

            ENDCG
        }
        // ------------------------------------------------------------------
        //  Additive forward pass (one light per pass)
        Pass
        {
            Name "FORWARD_DELTA"
            Tags { "LightMode" = "ForwardAdd" }
            Blend SrcAlpha One
            Fog { Color (0,0,0,0) } // in additive pass fog should be black
            ZWrite Off ZTest LEqual Cull Off

            CGPROGRAM
            #pragma target 3.0
            // GLES2.0 disabled to prevent errors spam on devices without textureCubeLodEXT
            #pragma exclude_renderers gles

            #pragma multi_compile _VERTEXCOLORMODE_NONE _VERTEXCOLORMODE_ALBEDO _VERTEXCOLORMODE_SPECULAR _VERTEXCOLORMODE_EMISSION
            #pragma multi_compile _ _OVERRIDENORMALS

            #pragma multi_compile _ _NORMALMAP
            #pragma multi_compile _ _EMISSIONMAP
            #pragma multi_compile _ _METALLICGLOSSMAP

            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog

            #pragma vertex FluvioVertForwardAdd
            #pragma fragment FluvioFragForwardAdd

            #include "FluidEffect.cginc"

            ENDCG
        }
    }
}