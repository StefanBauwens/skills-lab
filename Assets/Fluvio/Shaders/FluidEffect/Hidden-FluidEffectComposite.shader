Shader "Hidden/Fluvio/FluidEffectComposite"
{
    Properties
    {
        _MainTex ("Base (RGBA)", 2D) = "white" {}
    }
    SubShader
    {
        Pass
        {
            ZTest Always
            Cull Off
            ZWrite Off
            Fog { Mode Off }
            Tags {"Queue" = "Overlay" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ FLUVIO_CAMERA_DEPTHNORMALS
            #pragma multi_compile _ FLUVIO_DEPTH_MODE_CONSTANT FLUVIO_DEPTH_MODE_TEXTURE
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

            #if FLUVIO_DEPTH_MODE_CONSTANT || FLUVIO_DEPTH_MODE_TEXTURE
                #if FLUVIO_CAMERA_DEPTHNORMALS
                    #define FLUVIO_CAMERA_DEPTH_TEX_NAME _CameraDepthNormalsTexture
                #else
                    #define FLUVIO_CAMERA_DEPTH_TEX_NAME _CameraDepthTexture
                #endif
            #endif

            uniform sampler2D FLUVIO_CAMERA_DEPTH_TEX_NAME;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST; uniform half4 _MainTex_TexelSize;
            uniform sampler2D _FluidTex; uniform float4 _FluidTex_ST; uniform half4 _FluidTex_TexelSize;
            uniform sampler2D _BGCameraTex; uniform half4 _BGCameraTex_TexelSize;

            #if FLUVIO_DEPTH_MODE_TEXTURE
                uniform sampler2D _FluidDepthTex; uniform half4 _FluidDepthTex_TexelSize;
                uniform float _FluidRefraction;
            #elif FLUVIO_DEPTH_MODE_CONSTANT
                uniform float _FluidDepth;
            #endif

            float _FluidThreshold;
            float _FluidSpecular;
            float _FluidSpecularScale;
            float _FluidOpacity;
            float4 _FluidTint;
            float _FluidFade;

            uniform float _CameraOrtho;

            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
            };

            // Inverse lerp
            inline float invlerp(float from, float to, float value)
            {
                if (from < to)
                {
                    if (value < from)
                        return 0.0f;
                    if (value > to)
                        return 1.0f;
                    value -= from;
                    value /= to - from;
                    return value;
                }
                else
                {
                  if (from <= to)
                      return 0.0f;
                  if (value < to)
                      return 1.0f;
                  if (value > from)
                      return 0.0f;
                  else
                      return 1.0 - (value - to) / (from - to);
                }
            }

            v2f vert( appdata_img v )
            {
                v2f o;
                o.pos = ObjectToClipPos(v.vertex);
                float2 uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                float2 uv2 = TRANSFORM_TEX(v.texcoord, _FluidTex);

                #ifdef UNITY_HALF_TEXEL_OFFSET
                    uv.y += _MainTex_TexelSize.y;
                    uv2.y += _FluidTex_TexelSize.y;
                #endif
                #if UNITY_UV_STARTS_AT_TOP
                    if (_MainTex_TexelSize.y < 0)
                        uv.y = 1.0 - uv.y;
                    if (_FluidTex_TexelSize.y < 0)
                        uv2.y = 1.0 - uv2.y;
                #endif

                o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, uv);
                o.uv2 = MultiplyUV(UNITY_MATRIX_TEXTURE0, uv2);

                return o;
            }

            float4 frag (v2f i) : COLOR
            {
                #if FLUVIO_DEPTH_MODE_CONSTANT || FLUVIO_DEPTH_MODE_TEXTURE
                    // Get scene depth texture information
                    float sceneDepth;
                    float4 sceneDepthTex = tex2D(FLUVIO_CAMERA_DEPTH_TEX_NAME, i.uv);

                    #if FLUVIO_CAMERA_DEPTHNORMALS
                        float3 sceneNormal;
                        DecodeDepthNormal(sceneDepthTex, sceneDepth, sceneNormal);
                    #else
                        sceneDepth = lerp(Linear01Depth(sceneDepthTex.r), sceneDepthTex.r, _CameraOrtho);
                    #endif

                    // Get fluid depth texture information
                    float fluidDepth;

                    #if FLUVIO_DEPTH_MODE_TEXTURE
                        float4 fluidDepthTex = tex2D(_FluidDepthTex, i.uv);
                        float3 fluidNormal;
                        DecodeDepthNormal(fluidDepthTex, fluidDepth, fluidNormal);
                    #else
                        fluidDepth = _FluidDepth;
                    #endif
                #endif

                // Get fluid color and alpha
                float4 color = tex2D(_FluidTex, i.uv2) * float4(_FluidTint.rgb,1);
                color.a = invlerp(min(_FluidThreshold, .999), 1, color.a);

                #if FLUVIO_DEPTH_MODE_CONSTANT || FLUVIO_DEPTH_MODE_TEXTURE
                    float d = sceneDepth - fluidDepth;
                    color.a *= saturate(d * _FluidFade);
                #endif

                // Texture behind fluid
                float4 sceneColorTex = tex2D(_MainTex, i.uv);

                #if FLUVIO_DEPTH_MODE_TEXTURE
                    float4 bgColorTex = tex2D(_BGCameraTex, i.uv + fluidNormal.xy * _FluidRefraction);
                #else
                    float4 bgColorTex = tex2D(_BGCameraTex, i.uv);
                #endif

                bgColorTex = lerp(sceneColorTex, bgColorTex, color.a);

                // Fake Specular
                float4 spec = pow(color, _FluidSpecular/max(color.a, 0.01)) * _FluidSpecularScale * _FluidTint.a;

                // Final color
                return lerp(color, bgColorTex, (1.0f - color.a * (1.0f - _FluidOpacity))) + spec;
            }
            ENDCG
        }
    }
Fallback Off
}
