Shader "Unlit/hand"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BackTex ("BackTexture", 2D) = "white" {}
        _Steps("Steps", Float) = 1.
        _Color("col", Color) = (0, 0, 0, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Cull Off
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

            sampler2D _MainTex;
            sampler2D _BackTex;
            float4 _MainTex_ST;
            float4 _BackTex_ST;
            float4 _Color;
            float _Steps;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            float luminance(float3 color) {
            float3 magic = float3(0.2125, 0.7154, 0.0721);
            return dot(magic, color);
            }

            float Posterization(float In, float Steps)
            {
               return round(In * Steps) / Steps;
            }

            half4 frag(v2f i, bool facing : SV_ISFRONTFACE) : SV_Target
            {
                // sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 col;
                if (!facing)
                {
                    // Front side, use the front texture
                    col = tex2D(_BackTex, i.uv);

                }
                else
                {
                    col = tex2D(_MainTex, i.uv);

                }
                
                if (col.a < 1)
                {
                discard; // If alpha is too low, discard the fragment (makes it fully transparent)
                }

                float lum = luminance(col);
              //  col.rgb += float3(.8, .65, .5) * .9;

              //  col.rgb -= float3(0.7, 0.7, 0.7);
            
                return float4(col.rgb, col.a) * _Color;
            }
            ENDCG
        }
    }
}
