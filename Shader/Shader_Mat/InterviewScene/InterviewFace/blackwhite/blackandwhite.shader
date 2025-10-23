Shader "Unlit/blackandwhite"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _M ("drkness", Float) = 0
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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _M;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, 1 - i.uv);
                float lum = dot(col.rgb, float3(0.2125, 0.7154, 0.0721));
                if(lum < _M)
                {
                    return float4(0, 0, 0, 1);
                }
                else
                {
                 
                    return float4(1, 1, 1, 1);
                }
               
            }
            ENDCG
        }
    }
}
