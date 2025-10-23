Shader "Unlit/SpriteInterpolate"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Scroll("Scroll", Float) = 1.
        _Sprite("Sprite", 2D) = "white" {}
        _FrameAmount("Amount of Frames", Float) = 1.
    }

    SubShader
    {
        Blend SrcAlpha OneMinusSrcAlpha
ZWrite Off
        Tags { "RenderType"="Transparent" }
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

            sampler2D _MainTex;
            sampler2D _Sprite;
            float4 _MainTex_ST;
            float _Scroll;
            float _FrameAmount;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            float3 GetSprite(float2 uv, float max, float index)
            {
            float spriteWidth = 1.0 / max;
            float2 sliceUV = float2(uv.x / max + index * spriteWidth, uv.y);
            float3 col = tex2D(_Sprite, sliceUV);
            return col;
            }
            float4 GetLerpSprite(float2 uv)
            {
                float4 colA = float4(GetSprite(uv, _FrameAmount, 0), 1);
                float4 colB = float4(GetSprite(uv, _FrameAmount, 1), 1);
                return lerp(colA, colB, _Scroll);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float4 col = GetLerpSprite(i.uv);
                if(col.r == 0)
                {
                    discard;
                }
               // _Scroll += abs(sin(_Time * 10));
               
                return col;
            }
            ENDCG
        }
    }
}
