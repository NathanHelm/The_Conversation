Shader "Unlit/depth_shader_ledger"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex_1("N1", 2D) = "white" {}
        _DarknessTex("DarknessTex", 2D) = "white" {}

        _ColorWheel("color wheel", 2D) = "white" {}
        _Range("Range", Vector) = (1.,1.,0, 0.)
        _Scale("n1 Scale", Float) = 1.0
        _Scale1("n2 Scale", Float) = 1.0
        _Strength("Color wheel strength", Float) = .5
        _COORD("coordinate", Vector) = (0, 0, 0, 0)
        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Cull Off ZWrite Off ZTest Always
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
                float4 worldPos : TEXCOORD2;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 worldPos : TEXCOORD2;
            };

            sampler2D _MainTex;

            sampler2D _NoiseTex_1;
            sampler2D _NoiseTex_2;
            sampler2D _DarknessTex;

            float4 _NoiseTex_1_TexelSize;
            float4 _NoiseTex_2_TexelSize;
            float4 _Range;

            sampler2D _ColorWheel; 
            
            float4 _MainTex_TexelSize;

            float4 _MainTex_ST;
            sampler2D _CameraDepthTexture;

            float _Scale;
            float _Scale1;
            float _Strength;
            float4 _COORD;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;//o.vertex.xy / o.vertex.w * 0.5 + 0.5;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                
                return o;
            }
            float Unity_InverseLerp_float4(float A, float B, float T)
            {
               return (T - A)/(B - A);
            }


            fixed4 frag (v2f i) : SV_Target
            {
                
                float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv); 
                float sceneDepth = Linear01Depth(depth); //getting scene depth.


                float3 colorWheelDepth = tex2D(_ColorWheel, float2(sceneDepth * _Strength, .5)); //the depth of the shader is decided by a texure's color (likely a gradient)
 
                float3 col = tex2D(_MainTex, i.uv).xyz; //get screen texture

                float lum = dot(col, float3(0.299f, 0.587f, 0.114f)); //get luminence of pixel color
                float3 drkCol = tex2D(_DarknessTex, float2(i.uv.x + _Time.x * 0.5, i.uv.y)).xyz;
                
                drkCol += float3(0.1,0.1,0.1);

                float3 drkLum = dot(drkCol, float3(0.299f, 0.587f, 0.114f));
                

                if(col.r > 0.99 && col.g < 0.01 && col.b < 0.01)
                {
                    //clip;
                   discard;
                }
                
                lum *= drkLum.x;

                
                float2 noiseUV = i.uv * _NoiseTex_1_TexelSize.xy * _MainTex_TexelSize.zw; //scale the noise of both textures...

                float3 threshold = tex2D(_NoiseTex_1, noiseUV * _Scale);

                float thresholdLum = dot(threshold, float3(0.299f, 0.587f, 0.114f));

                float3 r_col = thresholdLum > lum? col * 0.5 : col;

                return float4(r_col.rgb, 1.);

               // lum *= lerp(.8, 1, sin(_Time * 10) * 1); //interpolate between .8-1 based on a sin wave (for a interesting lighting effect)


               // float3 sceneDepthFixed = sceneDepth > .99? float3(1, 1, 1) : colorWheelDepth; //set the background to black. 

                //float v = lum < threshhold? 0 : 1.; //is the noise texture darker than the sin wave's value? 
                
                //float3 rgb = float3(v,v,v);
                
                //return float4(rgb, 1.0f);
           
            }
            ENDCG
        }
    }
}
