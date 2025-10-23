Shader "Unlit/InterviewIcon"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Val ("val", Float) = 0
    }
    SubShader
    {
       Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
      

        Pass
        {
            Blend SrcAlpha 
            OneMinusSrcAlpha
            ZWrite Off
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
            float4 _MainTex_ST;
            float _Val;

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
                float4 rgb = lerp(float4(0, 0, 0, 0),float4(col.rgba), _Val);
                return rgb;
            }
            ENDCG
        }
    }
}
