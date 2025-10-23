Shader "Unlit/EdgeDetection"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex("Noise Texture", 2D) = "white" {}
        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Name "DepthOnly"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
           
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                /*
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                */
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
               
            };

            sampler2D _MainTex;
            sampler2D _CameraDepthTexture;
            sampler2D _CameraGBufferTexture2;
            sampler2D _NoiseTex; 
            float _OutlineThickness;
            float _OutlineDepthMultiplier;
            float _OutlineDepthBias;
            float _OutlineNormalMultiplier;
            float _OutlineNormalBias;
            float _NoiseStrength;



            float4 _OutlineColor;
            
            float Sobel(float4 l, float4 r, float4 u, float4 d, float4 c)
            {
                return  abs(l - c) +
                abs(r - c) +
                abs(u - c) +
                abs(d -c);
            }

            float SobelSampleDepth(float2 uv, float3 offset, float depthOffset)
            {
                float2 offsetX = offset.xz * depthOffset;
                float2 offsetY = offset.zy * depthOffset;

                float pixelCenter = LinearEyeDepth(tex2D(_CameraDepthTexture, uv).r );
                float pixelLeft   = LinearEyeDepth(tex2D(_CameraDepthTexture, uv - offsetX).r);
                float pixelRight  = LinearEyeDepth(tex2D(_CameraDepthTexture, uv + offsetX).r);
                float pixelUp     = LinearEyeDepth(tex2D(_CameraDepthTexture, uv + offsetY).r);
                float pixelDown   = LinearEyeDepth(tex2D(_CameraDepthTexture, uv - offsetY).r);
                
                return abs(pixelLeft  - pixelCenter) +
                abs(pixelRight - pixelCenter) +
                abs(pixelUp  - pixelCenter) +
                abs(pixelDown  - pixelCenter);
            }

            float4 SobelSample(sampler2D t, float2 uv, float3 offset)
            {
                float4 pixelCenter = tex2D(t, uv);
                float4 pixelLeft   = tex2D(t, uv - offset.xz);
                float4 pixelRight  = tex2D(t, uv + offset.xz);
                float4 pixelUp     = tex2D(t, uv + offset.zy);
                float4 pixelDown   = tex2D(t, uv - offset.zy);

                return Sobel(pixelLeft, pixelRight, pixelUp, pixelDown, pixelCenter);
            }
            float SampleNoise(float2 uv)
            {
                return tex2D(_NoiseTex, uv * 10).r;
            }
         

            v2f vert (appdata v)
            {
                /*
                v2f output;
                output.vertex = float4(v.vertex.xy, 0.0, 1.0);
                output.texcoord = (v.vertex.xy + 1.0) * 0.5;
                // For Direct3D Build
                output.texcoord.y = 1.0 - output.texcoord.y;
                // For Open/WebGL build
                //output.texcoord.y = output.texcoord.y;

                */
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 offset = float3((1.0 / _ScreenParams.x), (1.0 / _ScreenParams.y), 0.0) * _OutlineThickness;
                float3 sceneColor = tex2D(_MainTex, i.uv.xy).rgb;
                float noise = SampleNoise(i.uv * 5);

                float sobelDepth = SobelSampleDepth(i.uv.xy, offset, noise * _NoiseStrength);

                //for transparency purposes...
                float3 outlineColor = lerp(sceneColor, _OutlineColor.rgb, _OutlineColor.a);

                float3 color = lerp(sceneColor, outlineColor, sobelDepth);

                return float4(color, 1.);
            }
            ENDCG
        }
         GrabPass { "_GrabTex" }
      Pass
        {
            Name "OutlinePass"
            Tags { "LightMode" = "Always" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _GrabTex;
            float4 _GrabTex_TexelSize;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv.x = v.uv.x;
                o.uv.y = 1 - v.uv.y;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 col = tex2D(_GrabTex, i.uv);

                
                return col; // green outline
            }
            ENDCG
        }
    }
}
