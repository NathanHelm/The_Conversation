Shader "Custom/FrontBackPageShader"
{
    Properties{
        _FrontSideTex ("Front Side Texture", 2D) = "white" {}
        _FrontSideInsideTex ("Texture At Center/FrontSide", 2D) = "white" {}
        _BackSideTex ("Back Side Texture", 2D) = "white" {}
        _BackSideInsideTex ("Texture At Back Side Texture", 2D) = "white" {}
      
        _OffsetBack("Back offset xy, z = scale", Vector) = (0, 0, 1, 0)
        _OffsetFront("Front offset xy, z = scale", Vector) = (0, 0, 1, 0)
        _FrontCanvas("Front Canvas xy: minx max miny maxy", Vector) = (0, 0, 0, 0)
        _BackCanvas("Back Canvas xy:  minx max miny maxy", Vector) = (0, 0, 0, 0)
        _Boldness("Writing Darkness", Range(0,1)) = .7
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite On
            Cull Off
            CGPROGRAM
            #pragma target 4.0
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #define LUM(c) ((c).r*.3 + (c).g*.59 + (c).b*.11)

            sampler2D _FrontSideTex; 
            sampler2D _FrontSideInsideTex; 
            sampler2D _BackSideTex;
            sampler2D _BackSideInsideTex; 

            float4 _OffsetBack;
            float4 _OffsetFront;
            float4 _BackCanvas;
            float4 _FrontCanvas;
            float _Boldness;



            struct appdata 
            { 
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            struct v2f     
            { 

            float4 pos : SV_POSITION; 
            float3 normal : NORMAL; 
            float2 uv : TEXCOORD0;

            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.normal = UnityObjectToWorldNormal(float3(0,0,1));
                
                return o;
            }
            fixed4 PaperBlend(in fixed3 paperTex, in fixed3 insideTex, in float t)
            {
                return fixed4(lerp(paperTex, insideTex, t), 1.);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float facing = dot(i.normal, normalize(_WorldSpaceCameraPos - mul(unity_ObjectToWorld, float4(0,0,0,1)).xyz));
                fixed4 FrontTex = tex2D(_FrontSideTex, 1 - i.uv);
                fixed4 BackTex = tex2D(_BackSideTex, i.uv);
                float2 uvIBack = i.uv * _OffsetBack.z + float2(_OffsetBack.x, _OffsetBack.y);
                float2 uvIFront = 1 - i.uv * _OffsetFront.z + float2(_OffsetFront.x, _OffsetFront.y);
                fixed4 BackTexInside = tex2D(_BackSideInsideTex, uvIBack);
                fixed4 FrontTexInside = tex2D(_FrontSideInsideTex, uvIFront);
                if(facing > 0)
                {
                    return BackTex;
                }
                else
                {
                    FrontTexInside = PaperBlend( FrontTex, FrontTexInside - _Boldness, FrontTexInside.a);
                    float4 dec;
                    dec = (i.uv.x < _FrontCanvas.x || i.uv.x > _FrontCanvas.y || i.uv.y < _FrontCanvas.z || i.uv.y > _FrontCanvas.w)? FrontTex : FrontTexInside;
                    return dec;
                }
            }

            ENDCG
        }
    }
}
