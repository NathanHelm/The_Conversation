Shader "Custom/Double_Page_Texture_First"
{
    Properties
    {
        _FrontTexture ("Front Texture", 2D) = "white" {}
        _BackTexture ("Back Texture", 2D) = "white" {}
        _MainTex ("Base Texture", 2D) = "white" {}
        _Color ("color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        ZWrite Off
        ZTest Always
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {
            Cull Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            // Textures
            sampler2D _FrontTexture;
            sampler2D _BackTexture;
            float4 _FrontTexture_ST;
            float4 _BackTexture_ST;
            float4 _Color;
            
            // Vertex shader
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
            };
            
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.normal = v.normal;
                return o;
            }
            
            // Fragment shader
            half4 frag(v2f i, bool facing : SV_ISFRONTFACE) : SV_Target
            { 
                half4 color;
                if (facing)
                {
                    // Front side, use the front texture
                    color = tex2D(_FrontTexture, i.uv);
                }
                else
                {
                    // Back side, use the back texture
                    color = tex2D(_BackTexture, i.uv);
                }
                if (color.a < 1)
                {
                discard; // If alpha is too low, discard the fragment (makes it fully transparent)
                }
                return color * _Color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}


