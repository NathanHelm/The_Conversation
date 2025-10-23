Shader "Unlit/EdgeDetectionPaper"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex("Noise Texture", 2D) = "white" {}
        _LNoiseTex("Light Noise", 2D) = "white" {}
        _BackgroundTex("Background Texture", 2D) = "white"{}
        _Val ("Value", Float) = 0.0
        _Color("Color", Color) = (1, 1, 1, 1)
        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Pass
        {
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
            sampler2D _LNoiseTex;
            sampler2D _MainTex;
            sampler2D _BackgroundTex;
            sampler2D _CameraDepthTexture;
            sampler2D _CameraNormal;
            sampler2D _CameraDepthNormalsTexture;
            sampler2D _CameraGBufferTexture2;
            sampler2D _NoiseTex; 
            float _OutlineThickness;
            float _OutlineDepthMultiplier;
            float _OutlineDepthBias;
            float _OutlineNormalMultiplier;
            float _OutlineNormalBias;
            float _NoiseStrength;
            float _WiggleFrequency;
            float _WiggleAmplitude;
            float _Val;
            float _Color;



            float4 _OutlineColor;
            
            float Sobel(float4 l, float4 r, float4 u, float4 d, float4 c)
            {
                return  abs(l - c) +
                abs(r - c) +
                abs(u - c) +
                abs(d - c);
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

            float4 SobelSample(sampler2D t, float2 uv, float3 offset, float depthOffset)
            {
                uv *= depthOffset;
                float4 pixelCenter = tex2D(t, uv);
                float4 pixelLeft   = tex2D(t, uv - offset.xz);
                float4 pixelRight  = tex2D(t, uv + offset.xz);
                float4 pixelUp     = tex2D(t, uv + offset.zy);
                float4 pixelDown   = tex2D(t, uv - offset.zy);

                return abs(pixelLeft - pixelCenter)  +
                       abs(pixelRight - pixelCenter) +
                       abs(pixelUp - pixelCenter)    +
                       abs(pixelDown - pixelCenter);
            }
            float SampleNoise(float2 uv)
            {
                return tex2D(_NoiseTex, uv).r;
            }
            float Noise (float2 uv) {
               return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
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
                o.uv = 1 - v.uv;
                return o;
            }
            float hash(float2 p){
            float3 p3  = frac(float3(p.xyx) * .1031);
            p3 += dot(p3, p3.yzx + 33.33);
            return frac((p3.x + p3.y) * p3.z);
            }

            float luminance(float3 color) {
             float3 magic = float3(0.2125, 0.7154, 0.0721);
            return dot(magic, color);
            }
           

            fixed4 frag (v2f i) : SV_Target
            {
                //i.uv = 1/_ScreenParams.xy;
                float3 offset = float3((1.0 / _ScreenParams.x), (1.0 / _ScreenParams.y), 0.0) * _OutlineThickness;
                float3 sceneColor = tex2D(_MainTex, i.uv.xy).rgb;
                float3 backgroundImage = tex2D(_BackgroundTex, i.uv.xy).rgb;
          
                float noise = SampleNoise(float2(i.uv.x, i.uv.y));

               // _WiggleFrequency += noise * 6;
                //_WiggleAmplitude *= noise;

                float2 wave = float2(sin(i.uv.y * _WiggleFrequency), cos(i.uv.x * _WiggleFrequency)) * _WiggleAmplitude;
                float3 sobelNormalVec = SobelSample(_CameraDepthNormalsTexture, i.uv + wave, offset, 1).rgb;

                float sobelNormal = sobelNormalVec.x + sobelNormalVec.y + sobelNormalVec.z;
                sobelNormal = pow(sobelNormal * _OutlineNormalMultiplier, _OutlineNormalBias);
                float sobelDepth = SobelSampleDepth(i.uv + wave, offset, _NoiseStrength);
                float sobelOutline = saturate(max(sobelDepth, sobelNormal));
                


                //for transparency purposes...
                float3 outlineColor = lerp(sceneColor, _OutlineColor.rgb, _OutlineColor.a);
               // return float4(sobelDepth,sobelDepth,sobelDepth,1.);
                
                if(sobelOutline < 0.5)
                {
                    discard;
                    return float4(0, 0, 0, 0);
                   
                }
            
                
                
                float3 color = lerp(sceneColor, outlineColor, sobelOutline);
                return float4(color.rgb, 1.);
            }
            ENDCG
        }
}
}

    
        

