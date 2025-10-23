Shader "Custom/write_shader_tst"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise_Texture", 2D) = "white" {}
        _Val ("Value", Float) = 0.0
         _Color("Color", Color) = (1, 1, 1, 1)
        _Speed("Speed", Float) = 1.0
        _V("uv_v", Float) = 0.0
       
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            CGPROGRAM
 
            #pragma vertex vert
            #pragma fragment frag
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

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _Val;
            float _Speed;
            float _V;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv = 1 - o.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                //get luminence noise
                fixed4 col2 = tex2D(_NoiseTex, i.uv.xy * 0.1);
                float3 W = float3(0.2126, 0.7152, 0.0722);
                float lum = saturate(dot(col2.rgb, W));

                float x = cos((_Time.x * 2 + lum * _Speed));
                float y = sin((_Time.x * 2 + lum * _Speed));

                col2 = tex2D(_NoiseTex, i.uv.xy + float2(x, y));
                
                lum = max(0.1, dot(col2.rgb, W));
                lum += i.uv.x;

               //..float lrpV = lerp(lum, 1,min(1,_Val));
                if(_Val < 0.01)
                {
                    return float4(0, 0, 0, 0);
                }

                float4 lrpCol = lerp(col2, col.rgba , min(1,_Val));

                float col_lum = max(0.1, dot(col.rgb, W));

               
 
                return lrpCol * _Color; 
                
            }
            ENDCG
        }
    }
}
