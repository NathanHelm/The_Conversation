Shader "Unlit/InterviewFace"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PageTex ("paper background", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue"="Opaque" }
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
            sampler2D _PageTex;
            sampler2D _MainTex;
            float4 _MainTex_ST;

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
                float3 color = col.a <= 0.001? tex2D(_PageTex, 1 - i.uv) : col ;
                return float4(color,1.);
            }
            ENDCG
        }
    }
}
