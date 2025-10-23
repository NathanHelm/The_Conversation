Shader "Unlit/Interview_NPC"
{
    Properties
    {
        
        _MainTex ("Texture", 2D) = "white" {}
        _PageTex("Page", 2D) = "white" {}
        _OffsetDrawing("Drawing Offset", Vector) = (0, 0, 0, 0)
        _ScaleDrawing("Scale Drawing", Vector) = (0, 0, 0, 0)
        _Val ("val", Float) = 0
           
       
    }
    SubShader
    {
        Tags { "Queue" = "AlphaTest" "RenderType" = "TransparentCutout" }
        LOD 100
        ZWrite On
        Pass
        {
           // Blend SrcAlpha 
           // OneMinusSrcAlpha
          
            ZTest LEqual
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
            sampler2D _PageTex;
            float4 _MainTex_ST;
            float4 _OffsetDrawing;
            float4 _ScaleDrawing;
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
                float2 uvPos = i.uv + _OffsetDrawing.xy;

                uvPos *= _ScaleDrawing;

                fixed4 col = tex2D(_MainTex, 1 - uvPos);

                fixed4 pageCol = tex2D(_PageTex, 1 - i.uv);
                
                if(col.a < 0.01)
                {
                    col = pageCol;
                }
                if(pageCol.a < 1)
                {
                    col.a = 0;
                }

                float4 rgb = lerp(float4(0, 0, 0, 0),float4(col.rgba), _Val);

                return float4(rgb.rgba);
            }
            
            ENDCG
            
        }
       
    }
}
