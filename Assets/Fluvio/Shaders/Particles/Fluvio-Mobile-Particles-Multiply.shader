Shader "Fluvio/Mobile/Particles/Multiply" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Intensity ("Intensity", Float) = 2.2
        _BumpMap ("Distort (in RG channels)", 2D) = "blue" {}
        _GrabTex ("Refraction Texture", 2D) = "gray" {}
        _Distort ("Distort", Float) = 0.1
        _WaveMod ("Wave Speed (XY) Wave Strength (ZW)", Vector) = (2, 6, .02, .025)
    }

    CGINCLUDE

        #include "FluidParticleShader-Mobile.cginc"
        #include "UnityCG.cginc"

        uniform fixed _Intensity;
        uniform sampler2D _BumpMap;
        uniform fixed _Distort;
        uniform fixed4 _WaveMod;

        uniform sampler2D _GrabTex;

        fixed4 fragB ( v2fB i ) : COLOR
        {
            fixed4 off = fixed4(sin(_Time.y * _WaveMod.x) * _WaveMod.z, cos(_Time.y * _WaveMod.y) * _WaveMod.w, 0.5, 0.5);
            fixed4 normal = tex2D (_BumpMap, i.uv) - off - 0.5;
            i.uvScreen += normal.xy * _Distort;

            fixed4 mainTex = tex2D (_MainTex, i.uv);
            fixed4 tint = mainTex * i.color;
            fixed4 screen = tex2D(_GrabTex, i.uvScreen) * (tint * _Intensity);
            screen.a = mainTex.a;

            return screen;
        }

    ENDCG

    SubShader {
        Tags {"Queue"="Transparent+100"  "RenderType"="Transparent"}
        Cull Off
        ZWrite Off
        ZTest LEqual
        Blend SrcAlpha OneMinusSrcAlpha
        Lighting Off

    Pass {

        CGPROGRAM

        #pragma vertex vertB
        #pragma fragment fragB
        #pragma fragmentoption ARB_precision_hint_fastest

        ENDCG

        }
    }
}
