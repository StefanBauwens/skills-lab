Shader "Fluvio/Particles/VertexLit Blended" {
    Properties {
        _TintColor("Tint Color", Color) = (1, 1, 1, 1)
        _Intensity ("Intensity", Float) = 2.2
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _BumpMap ("Distort (in RG channels)", 2D) = "blue" {}
        _Distort ("Distort", Float) = 0.1
        _Power ("Light Power", Range(0.0,5)) = 1
        _WaveMod ("Wave Speed (XY) Wave Strength (ZW)", Vector) = (2, 6, .02, .025)
        _InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
        _Cutoff ("Shadow Alpha cutoff", Range(0,1)) = 0.5
    }

    CGINCLUDE

        #include "FluidParticleShader-Lighting.cginc"
        #include "UnityCG.cginc"

        uniform half4 _TintColor;
        uniform half _Intensity;
        uniform sampler2D _BumpMap;
        uniform half _Distort;
        uniform half4 _WaveMod;

        sampler2D _GrabTex;

        half4 fragB ( v2fB i ) : COLOR
        {
            #ifdef SOFTPARTICLES_ON
            float sceneZ = LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.uvScreen))));
            float partZ = i.uvScreen.z;
            float fade = saturate (_InvFade * (sceneZ-partZ));
            #endif

            half4 off = half4(sin(_Time.y * _WaveMod.x) * _WaveMod.z, cos(_Time.y * _WaveMod.y) * _WaveMod.w, 0.5, 0.5);
            half4 normal = tex2D (_BumpMap, i.uv) - off - 0.5;
            i.uvScreen.xy += normal.xy * _Distort;

            half4 mainTex = tex2D (_MainTex, i.uv);
            half4 tint = mainTex * _TintColor;
            half3 tex = (tex2Dproj(_GrabTex, UNITY_PROJ_COORD (i.uvScreen)) * (1 - tint.a)) + (tint * (tint.a * i.color * i.light * _Intensity));
            half4 screen = half4(tex.xyz,  mainTex.a * i.color.a);

            #ifdef SOFTPARTICLES_ON
            screen.a *= fade;
            #endif

            return screen;
        }

    ENDCG

    SubShader {
        Tags {"Queue"="Transparent+100"  "RenderType"="Transparent"}

        // Pass to render object as a shadow caster
        Pass {
            Name "Caster"
            Tags { "LightMode" = "ShadowCaster" }
            Offset 1, 1

            Fog {Mode Off}
            ZWrite On ZTest LEqual Cull Off

        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #pragma multi_compile_shadowcaster
        #pragma fragmentoption ARB_precision_hint_fastest
        #include "UnityCG.cginc"

        struct v2f {
            V2F_SHADOW_CASTER;
            float2  uv : TEXCOORD1;
        };

        v2f vert( appdata_base v )
        {
            v2f o;
            TRANSFER_SHADOW_CASTER(o)
            o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
            return o;
        }

        uniform fixed _Cutoff;

        float4 frag( v2f i ) : COLOR
        {
            fixed4 texcol = tex2D( _MainTex, i.uv );
            clip( texcol.a - _Cutoff );

            SHADOW_CASTER_FRAGMENT(i)
        }
        ENDCG
        }

        GrabPass { "_GrabTex" }

        Pass {
            Cull Off
            ZWrite Off
            ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha
            Tags {"LightMode"="Vertex"}

            CGPROGRAM

            #pragma vertex vertB
            #pragma fragment fragB
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_particles

            ENDCG

            }

        }
        Fallback "Fluvio/Particles/No GrabPass/VertexLit Blended"
}
