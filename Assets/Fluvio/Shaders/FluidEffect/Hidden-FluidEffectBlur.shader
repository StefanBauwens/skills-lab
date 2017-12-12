Shader "Hidden/Fluvio/FluidEffectBlur" {
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }

    CGINCLUDE
    #pragma target 3.0
    #pragma multi_compile _BLUR_COLORS _BLUR_NORMALS
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

    struct v2f {
        float4 pos : POSITION;
        #if SHADER_API_D3D11_9X
        float4 blurCoordinates[7] : TEXCOORD0;
        #else
        float2 screenPos : TEXCOORD0;
        float4 blurCoordinates[7] : TEXCOORD1;
        #endif
    };

    // Change this to adjust the overall max strength of the blur
    #define BLUR_MULTIPLIER 1.0

    uniform float _BlurStrength;
    sampler2D _MainTex;
    float4 _MainTex_TexelSize;

    v2f vert_h( appdata_img v ) {
        v2f o;
        o.pos = ObjectToClipPos(v.vertex);
        float2 screenPos = v.texcoord;
        #ifdef UNITY_HALF_TEXEL_OFFSET
            screenPos.y += _MainTex_TexelSize.y;
        #endif
        #if UNITY_UV_STARTS_AT_TOP
        if (_MainTex_TexelSize.y < 0)
            screenPos.y = 1-screenPos.y;
        #endif
        #if !SHADER_API_D3D11_9X
        o.screenPos = screenPos;
        #endif
        float blurAmount = _BlurStrength * BLUR_MULTIPLIER * (_MainTex_TexelSize.w / _MainTex_TexelSize.z);
        o.blurCoordinates[0].xy = screenPos + float2(-0.028, 0.0) * blurAmount;
        o.blurCoordinates[1].xy = screenPos + float2(-0.024, 0.0) * blurAmount;
        o.blurCoordinates[2].xy = screenPos + float2(-0.020, 0.0) * blurAmount;
        o.blurCoordinates[3].xy = screenPos + float2(-0.016, 0.0) * blurAmount;
        o.blurCoordinates[4].xy = screenPos + float2(-0.012, 0.0) * blurAmount;
        o.blurCoordinates[5].xy = screenPos + float2(-0.008, 0.0) * blurAmount;
        o.blurCoordinates[6].xy = screenPos + float2(-0.004, 0.0) * blurAmount;
        o.blurCoordinates[0].zw = screenPos + float2(0.004, 0.0) * blurAmount;
        o.blurCoordinates[1].zw = screenPos + float2(0.008, 0.0) * blurAmount;
        o.blurCoordinates[2].zw = screenPos + float2(0.012, 0.0) * blurAmount;
        o.blurCoordinates[3].zw = screenPos + float2(0.016, 0.0) * blurAmount;
        o.blurCoordinates[4].zw = screenPos + float2(0.020, 0.0) * blurAmount;
        o.blurCoordinates[5].zw = screenPos + float2(0.024, 0.0) * blurAmount;
        o.blurCoordinates[6].zw = screenPos + float2(0.028, 0.0) * blurAmount;
        return o;
    }
    v2f vert_v( appdata_img v ) {
        v2f o;
        o.pos = ObjectToClipPos(v.vertex);
        float2 screenPos = v.texcoord;
        #ifdef UNITY_HALF_TEXEL_OFFSET
            screenPos.y += _MainTex_TexelSize.y;
        #endif
        #if UNITY_UV_STARTS_AT_TOP
        if (_MainTex_TexelSize.y < 0)
            screenPos.y = 1-screenPos.y;
        #endif
        #if !SHADER_API_D3D11_9X
        o.screenPos = screenPos;
        #endif
        float blurAmount = _BlurStrength * BLUR_MULTIPLIER;
        o.blurCoordinates[0].xy = screenPos + float2(0.0, -0.028) * blurAmount;
        o.blurCoordinates[1].xy = screenPos + float2(0.0, -0.024) * blurAmount;
        o.blurCoordinates[2].xy = screenPos + float2(0.0, -0.020) * blurAmount;
        o.blurCoordinates[3].xy = screenPos + float2(0.0, -0.016) * blurAmount;
        o.blurCoordinates[4].xy = screenPos + float2(0.0, -0.012) * blurAmount;
        o.blurCoordinates[5].xy = screenPos + float2(0.0, -0.008) * blurAmount;
        o.blurCoordinates[6].xy = screenPos + float2(0.0, -0.004) * blurAmount;
        o.blurCoordinates[0].zw = screenPos + float2(0.0, 0.004) * blurAmount;
        o.blurCoordinates[1].zw = screenPos + float2(0.0, 0.008) * blurAmount;
        o.blurCoordinates[2].zw = screenPos + float2(0.0, 0.012) * blurAmount;
        o.blurCoordinates[3].zw = screenPos + float2(0.0, 0.016) * blurAmount;
        o.blurCoordinates[4].zw = screenPos + float2(0.0, 0.020) * blurAmount;
        o.blurCoordinates[5].zw = screenPos + float2(0.0, 0.024) * blurAmount;
        o.blurCoordinates[6].zw = screenPos + float2(0.0, 0.028) * blurAmount;
        return o;
    }
    inline float4 Sample(float2 uv)
    {
        float4 tex = tex2D(_MainTex, uv);
        #if _BLUR_NORMALS
        float depth;
        float3 normal;
        DecodeDepthNormal(tex2D(_MainTex, uv), depth, normal);
        tex.rgb = normal;
        tex.a = depth;
        #endif
        return tex;
    }
    inline float4 GetOutput(float4 color)
    {
        #if _BLUR_NORMALS
        return EncodeDepthNormal(color.a, color.rgb);
        #else
        return color;
        #endif
    }
    float4 frag(v2f i) : COLOR {

        float4 color = float4(0,0,0,0);
        #if SHADER_API_D3D11_9X
        // Maybe find a way to not compute screen pos in fragment shader on d3d11_9x?
        float blurAmount = _BlurStrength * BLUR_MULTIPLIER;
        float2 screenPos = i.blurCoordinates[0].xy - float2(0.0, -0.028) * blurAmount;
        float4 tex = Sample(screenPos);
        #else
        float4 tex = Sample(i.screenPos);
        #endif
        color += Sample(i.blurCoordinates[0].xy)*0.0044299121055113265;
        color += Sample(i.blurCoordinates[1].xy)*0.00895781211794;
        color += Sample(i.blurCoordinates[2].xy)*0.0215963866053;
        color += Sample(i.blurCoordinates[3].xy)*0.0443683338718;
        color += Sample(i.blurCoordinates[4].xy)*0.0776744219933;
        color += Sample(i.blurCoordinates[5].xy)*0.115876621105;
        color += Sample(i.blurCoordinates[6].xy)*0.147308056121;
        color += tex*0.159576912161;
        color += Sample(i.blurCoordinates[0].zw)*0.147308056121;
        color += Sample(i.blurCoordinates[1].zw)*0.115876621105;
        color += Sample(i.blurCoordinates[2].zw)*0.0776744219933;
        color += Sample(i.blurCoordinates[3].zw)*0.0443683338718;
        color += Sample(i.blurCoordinates[4].zw)*0.0215963866053;
        color += Sample(i.blurCoordinates[5].zw)*0.00895781211794;
        color += Sample(i.blurCoordinates[6].zw)*0.0044299121055113265;

        #if !_BLUR_NORMALS
            // Keep alpha at edges
            color.a = tex.a;
        #endif

        return GetOutput(color);
    }
    ENDCG

    SubShader {
         Pass {
              ZTest Always Cull Off ZWrite Off
              Fog { Mode off }

              CGPROGRAM
              #pragma fragmentoption ARB_precision_hint_fastest
              #pragma vertex vert_h
              #pragma fragment frag
              ENDCG
          }
          Pass {
              ZTest Always Cull Off ZWrite Off
              Fog { Mode off }

              CGPROGRAM
              #pragma fragmentoption ARB_precision_hint_fastest
              #pragma vertex vert_v
              #pragma fragment frag
              ENDCG
          }
    }
    Fallback Off
}
