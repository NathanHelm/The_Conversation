Shader "Unlit/depth_shader_facebg"
{
    Properties
    {
        _Color("output color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _ImageAmount ("Face Texture Amount", int) = 3
        //TODO add 2DArray
        //_FaceTextures ("Face textures", 2DArray) = "" {}
        _Face1("f1", 2D) = "white" {}
        _Face2("f2", 2D)= "white" {}
        _Face3("f3",2D)= "white" {}
        _NoiseTex_1("N1", 2D) = "white" {}
        _NoiseTex_2("N2,", 2D) = "white" {}
        _ColorWheel("color wheel", 2D) = "white" {}
        _Range("Range", Vector) = (1.,1.,0, 0.)
        _Amplitude("Amp", Float) = 1.0
        _Freq("Freq", Float) = 1.0
        _Scale("n1 Scale", Float) = 1.0
        _Scale1("n2 Scale", Float) = 1.0
        _Strength("Color wheel strength", Float) = .5
        _COORD("coordinate", Vector) = (0, 0, 0, 0)
        _SliceAmount("slice amount", int) = 10
   

        //==========================================
        _Hue1("hue", Float) = 0
        _Sat1("saturation", Float) = 0
        _Bright1("brightness", Float) = 0

        _Hue2("hue", Float) = 0
        _Sat2("saturation", Float) = 0
        _Bright2("brightness", Float) = 0
       
        _Hue3("hue", Float) = 0
        _Sat3("saturation", Float) = 0
        _Bright3("brightness", Float) = 0


        _HSBStep("HSB steps", Vector) = (16, 16, 16, 16)
     
        //==========================================
        
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        Cull Front
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        ZTest Always
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.5
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 worldPos : TEXCOORD2;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 worldPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            sampler2D _Face1; 
            sampler2D _Face2;
            sampler2D _Face3;

            sampler2D _NoiseTex_1;
            sampler2D _NoiseTex_2;

            float4 _NoiseTex_1_TexelSize;
            float4 _NoiseTex_2_TexelSize;
            float4 _Range;

            sampler2D _ColorWheel; 
            float _ScaleAmount;
            int _SliceAmount;
            
            float4 _MainTex_TexelSize;

            float4 _MainTex_ST;
            sampler2D _CameraDepthTexture;

            float _Scale;
            float _Scale1;
            float _Strength;
            float _Amplitude;
            float _Freq;
            int _ImageAmount;
            float4 _COORD;
            float4 _HSBStep;
            //
            float _Hue1;
            float _Sat1;
            float _Bright1;
            //
            float _Hue2;
            float _Sat2;
            float _Bright2;
            //
            float _Hue3;
            float _Sat3;
            float _Bright3;

            float4 _Color;
            float4 _LightColor0;


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;//o.vertex.xy / o.vertex.w * 0.5 + 0.5;
                
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                
                return o;
            }
            //--
            float Posterization(float In, float Steps)
            {
               return round(In * Steps) / Steps;
            }

            float3 RGBtoHSV(float3 rgb)
            {
                float3 hsv;
                float cmax = max(rgb.r, max(rgb.g, rgb.b));
                float cmin = min(rgb.r, min(rgb.g, rgb.b));
                float delta = cmax - cmin;

                // Value (Brightness)
                hsv.z = cmax;

                // Saturation
                if (cmax > 0.0)
                    hsv.y = delta / cmax;
                else
                    hsv.y = 0.0;

                // Hue
                if (delta == 0.0)
                    hsv.x = 0.0; // achromatic
                else if (cmax == rgb.r)
                    hsv.x = fmod((rgb.g - rgb.b) / delta, 6.0);
                else if (cmax == rgb.g)
                    hsv.x = ((rgb.b - rgb.r) / delta) + 2.0;
                else
                    hsv.x = ((rgb.r - rgb.g) / delta) + 4.0;

                hsv.x /= 6.0; // Normalize hue to 0-1 range
                if (hsv.x < 0.0)
                    hsv.x += 1.0;

                return hsv;
            }
            float3 HSBtoRGB(float3 hsb) 
            {

            float h = hsb.x;
            float s = hsb.y;
            float v = hsb.z;

            float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
            float3 p = abs(frac(h + K.xyz) * 6.0 - K.w);
            return v * lerp(K.xxx, saturate(p - K.x), s);

            }
            float3 ChangePosterizeWithHSB(float3 texCol, float Hue, float Sat, float Bright)
            {
               //TODO float3 lum = getLum(HSBtoRGB(texCol));

              return float3(
                
                Posterization(texCol.r * Hue, _HSBStep.x),
                Posterization(texCol.g * Sat, _HSBStep.y),
                Posterization(texCol.b * Bright, _HSBStep.z)

               );
            }
            //--

            float3 PostColor(float3 tex_col, float H, float S, float B)
            {
                float3 col = RGBtoHSV(tex_col.rgb);
                col = ChangePosterizeWithHSB(col, H, S, B);
                col = HSBtoRGB(col.rgb);
                return col;
            }

            float3 DitherColor(float3 tex_col, float2 uv, float2 noiseUV)
            {
                float3 noise = tex2D(_NoiseTex_1, noiseUV * 1);
                float thresh = dot(noise, float3(0.299f, 0.587f, 0.114f));
                return thresh > 0.5? tex_col : tex_col - float3(0.3,0.3,0.3); 
            }
          
            float3 GetImage(float2 uv, float2 noiseUV)
            {
                float2 gridUV = uv * _SliceAmount;
                

                // Get integer cell coordinates
                int x = (int)floor(gridUV.x);
                int y = (int)floor(gridUV.y);

                // Calculate which texture to use: cycle through 0, 1, 2
                int textureIndex = fmod((x + y) ,_ImageAmount);

                // Get UV inside the cell (0-1 range)
                float2 cellUV = frac(gridUV);

                
               if (textureIndex == 0)
                {
                    return DitherColor(PostColor(tex2D(_Face1, cellUV), _Hue1, _Sat1, _Bright1),cellUV,noiseUV / cellUV);
                }
                else if (textureIndex == 1)
                {
                    return DitherColor(PostColor(tex2D(_Face2, cellUV), _Hue2, _Sat2, _Bright2),cellUV,noiseUV / cellUV);
              
                }
                else
                {
                    return DitherColor(PostColor(tex2D(_Face3, cellUV), _Hue3, _Sat3, _Bright3),cellUV,noiseUV / cellUV);
              
                }
            }


            fixed4 frag (v2f i) : SV_Target
            {
                
                float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv); 
                float sceneDepth = Linear01Depth(depth); //getting scene depth.


                float3 colorWheelDepth = tex2D(_ColorWheel, float2(sceneDepth * _Strength, .5)); //the depth of the shader is decided by a texure's color (likely a gradient)
 
                float3 col = tex2D(_MainTex, i.uv).xyz; //get screen texture

                float lum = dot(col, float3(0.299f, 0.587f, 0.114f)); //get luminence of pixel color

                
                float2 noiseUV = i.uv * _NoiseTex_1_TexelSize.xy * _MainTex_TexelSize.zw; //scale the noise of both textures...
                float2 noiseUV2 = i.uv * _NoiseTex_2_TexelSize.xy * _MainTex_TexelSize.zw;

                float3 threshold = tex2D(_NoiseTex_1, noiseUV * _Scale); 
                float3 threshold2 = tex2D(_NoiseTex_2, noiseUV2 * _Scale1);

                float thresholdLum = dot(threshold, float3(0.299f, 0.587f, 0.114f));
                float thresholdLum2 = dot(threshold2, float3(0.299f, 0.587f, 0.114f));

                lum *= pow(sin(_Time * _Freq) * _Amplitude, 2.0); //interpolate between .8-1 based on a sin wave (for a interesting lighting effect)
                
                float3 facebg = GetImage(i.uv + pow(sin(_Time.x * 2),2)*_Amplitude, noiseUV);
                float3 sceneDepthFixed = sceneDepth > .99? float3(1,1,1) : colorWheelDepth; //set the background to black. 

                float threshOnDepth = lerp(thresholdLum, thresholdLum2, clamp(0, .9, sceneDepthFixed)); //interpolate between noise textures based on object's depth
                
                float3 rgb = facebg; //is the noise texture darker than the sin wave's value? 
                
                half3 diffuse = rgb.rgb * _LightColor0.rgb * dot(_WorldSpaceLightPos0, normalize(i.worldPos));
                
                half intensity = dot(diffuse, half3(0.2326, 0.7152, 0.0722)) * 0.1;

              //  float3 rgb = float3(v,v,v);
              //  rgb -= sceneDepth - 1;
                return float4(rgb,1.) * _Color;//* intensity;
           
            }
            ENDCG
        }
    }
}
