Shader "Unlit/DreamShader_INGAME"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MapThreshold ("Map_Threshold", Float) = 1.0
        _StrokeWidth("edge width", Float) = 1.0
        _Intensity("intensity", Float) = 1.0
        _Threshold("col palette threshold", Float) = .3333
        _Palette1 ("col", Color) = (0,0,0,1) 
        _Palette2  ("Texture", 2D) = "white" {}
        _Palette3  ("Texture", 2D) = "white" {} 
        _Palette4  ("Texture", 2D) = "white" {}
        _Palette5  ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise", 2D) = "white" {}

        
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
                
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //_OutlineThickness = 3;
                float3 offset = float3((1.0 / _ScreenParams.x), (1.0 / _ScreenParams.y), 0.0) * _OutlineThickness;
                float3 sceneColor = tex2D(_MainTex, i.uv.xy).rgb;
                float noise = 1;//commented this out--> SampleNoise(i.uv * 5);
                float sobelDepth = SobelSampleDepth(i.uv.xy, offset, noise * _NoiseStrength);
                //for transparency purposes...
                float3 outlineColor = lerp(sceneColor, _OutlineColor.rgb, _OutlineColor.a);
                float3 color = lerp(sceneColor, outlineColor, sobelDepth);
               // _NoiseStrength = 1;
              //  color = float3(_NoiseStrength, _NoiseStrength, _NoiseStrength);
                return float4(color, 1.);
            }
            ENDCG
        }

        GrabPass { "_GrabTexture0" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            #define LUM(c) ((c).r*.3 + (c).g*.59 + (c).b*.11)

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;

            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD1;
                float3 wPos : TEXCOORD2;
            };
            sampler2D _CameraDepthTexture;
            sampler2D _GrabTexture0;
            float4 _GrabTexture0_ST;
            float4 _Col;
            float _Intensity;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _GrabTexture0);
                o.normal = UnityObjectToWorldNormal( v.normal );
                o.wPos = mul( unity_ObjectToWorld, v.vertex );
                return o;
            }
            float growFloatByTime()
            {
               return sin(_Time) * 0.05;
            }
            float inverselerp(float a, float b, float t)
            {
                return (1.0 - t) * a + b * t;
            }
            inline float unity_noise_randomValue (float2 uv){
            return frac(sin(dot(uv, float2(12.9898, 78.233)))*43758.5453);
            }
            inline float unity_noise_interpolate (float a, float b, float t)
            {
                return (1.0-t)*a + (t*b);
            }
            float GetDepth(float2 uv)
            {
                float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv);
                return LinearEyeDepth(depth);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                
                float depth = 1 - pow(GetDepth(i.uv), .75) * _Intensity;
                if(depth < 0.001)
                {
                   // depth = 0;
                }
                float chaos = ((i.uv.x * 2 - 1) * (i.uv.y * 2 - 1)) * depth * 0.05;

                float chaosSides = chaos + (sin(_Time * 10) * 0.09) * 10;
                //return float4(depth, depth, depth, 1.);
                float3 check_color = tex2D(_GrabTexture0, i.uv);
                if(LUM(check_color) <= 0.01)
                {
                    return float4(0 , 0 , 0, 1);
                }
                float3 color = tex2D(_GrabTexture0, ((lerp(-1, 1, unity_noise_randomValue(i.uv)) * chaos) + i.uv)).rgb;
                return float4(color.r, color.g, color.b, 1.);
            }
             
            ENDCG
        }
        GrabPass { "_GrabTexture" }
        Pass 
        {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
             #define LUM(c) ((c).r*.3 + (c).g*.59 + (c).b*.11)
            #include "UnityCG.cginc"

            sampler2D _GrabTexture;


            float _Threshold;
            float4 _Palette1;
            sampler2D  _Palette2;
            sampler2D  _Palette3;
            sampler2D  _Palette4;
            sampler2D  _Palette5;
            sampler2D _NoiseTex;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv.y = v.uv.y;
                o.uv.x = v.uv.x;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                
                float3 col = tex2D(_GrabTexture, i.uv).rgb;
                float3 noise = tex2D(_NoiseTex,float2(i.uv.x + _Time.x * 0.1, i.uv.y) * 2);
                if(LUM(col) <= 0.01)
                {
                    return float4(0, 0, 0, 1);
                }
                float3 weight = float3(1, -.5, -.5);
                float avg = LUM(col);
                float threshold = _Threshold;
                if(avg < threshold * 1.5)
                {
                    col = float3(_Palette1.r, _Palette1.g, _Palette1.b);
                }
                else if(avg < threshold * 2.5)
                {
                    noise = noise.r > 0.5? float3(1,1,1) : float3(.1,0,0); 
                    col = tex2D(_Palette3, i.uv).rgb +  weight;
                    col = col * noise;
                }
                else if(avg < threshold * 4.5)
                {
                    col = tex2D(_Palette2, (i.uv + float2(0,_Time.x * .5)) * 1).rgb + weight;
                }
                else if(avg < threshold * 5)
                {
                    col = tex2D(_Palette4, i.uv).rgb + weight;
                }
                else 
                {
                   
                    col = float3(1,avg * 1.5 ,avg * 1.5);
                }
                return float4(col, 1.);
            }
            ENDCG
        
        }
            
    }
}
