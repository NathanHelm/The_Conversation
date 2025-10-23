Shader "Unlit/Posterization"
{
    Properties
    {
        _PosterizeTex ("Texture", 2D) = "white" {}
        _Steps("posterize step", Float) = 16
        _Hue("hue", Float) = 0
        _Sat("saturation", Float) = 0
        _Bright("brightness", Float) = 0
        _HSBStep("hsb steps", Vector) = (16, 16, 16, 0)
         
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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };



            sampler2D _PosterizeTex;
            float4 _PosterizeTex_ST;
            float _Steps;
            float4 _PosterizeColor;
            float _Hue;
            float _Sat;
            float _Bright; 
            float4 _HSBStep;
           
 
            /*
            void Unity_Posterize_float4(float4 In, float4 Steps, out float4 Out)
            {
                Out = floor(In / (1 / Steps)) * (1 / Steps);
            }
            */
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
            float3 HSBtoRGB(float3 hsb) {
            float h = hsb.x;
            float s = hsb.y;
            float v = hsb.z;

            float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
            float3 p = abs(frac(h + K.xyz) * 6.0 - K.w);
            return v * lerp(K.xxx, saturate(p - K.x), s);
            }
            float3 ChangePosterizeWithHSB(float3 texCol)
            {
               //TODO float3 lum = getLum(HSBtoRGB(texCol));

              return float3(
                
                Posterization(texCol.r * _Hue, _HSBStep.x),
                Posterization(texCol.g * _Sat, _HSBStep.y),
                Posterization(texCol.b * _Bright, _HSBStep.z)

               );
            }



            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _PosterizeTex);
              //  o.uv = 1-v.uv;
                return o;
            }
          
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 tex_col = tex2D(_PosterizeTex, i.uv);
                float3 col = RGBtoHSV(tex_col.rgb);
                col = ChangePosterizeWithHSB(col);
                col = HSBtoRGB(col.rgb);
                
                return float4(col.rgb,1.);
            }
            ENDCG
        }
    }
}
