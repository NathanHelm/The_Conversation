    Shader "Unlit/painting_texture"
    {
        Properties
        {
            _MainTex ("Texture", 2D) = "white" {}
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
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;

                v2f vert (appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    return o;
                }

        float InverseLerp(float _A, float B, float T)
        {
            return (T - _A)/(B - _A);
        }
        float rand(float co){

        return frac(sin(co*(91.3458)) * 47453.5453);

        }


       fixed4 frag (v2f i) : SV_Target
       {

        //float2 mouse = iMouse.xy/iResolution.xy;

        float time = _Time.x;

        float2 uv = i.uv;

        float timeSpeed = 3. * rand(1.);

        float speed = 3.;

        float fractAmount = 3.5;

        float2 overlayPos = float2(.5,.5);

        float2 uvMix = float2(cos(time * timeSpeed),sin(time * timeSpeed));



        float t = time * speed;

        float2 centerScale = float2(0.5, 0.5); //note that centerScale as a static value, gives a simple 'centered overlay.' given overlayPos = .5
        float fractPosStuff = frac(lerp(cos(t), sin(t), uv.x * uvMix.x) * lerp(sin(t), cos(t), uv.y * uvMix.y) * fractAmount);
        centerScale = float2(fractPosStuff, fractPosStuff);

      // float2 centerScale = float2(.5, .5);
       

        centerScale *= .5;

        float2 centerXY = overlayPos;



        if(uv.x >= centerXY.x - centerScale.x && uv.x <= centerXY.x + centerScale.x
        && uv.y >= centerXY.y - centerScale.y && uv.y < centerXY.y + centerScale.y )
        {
            //x,y = 0 in normal scale = centerXY - centerScale = lowerbound.
            //x,y = 1 in normal scale = centerXY + centerScale = upperbound.

            float2 upperbound = centerXY + centerScale;
            float2 lowerbound = centerXY - centerScale;

            float2 bounds = float2( InverseLerp(lowerbound.x, upperbound.x, uv.x), InverseLerp(lowerbound.y, upperbound.y, uv.y));

            return tex2D(_MainTex, bounds);

        }
        else
        {
            return tex2D(_MainTex, uv);
        }
        }
           
        ENDCG

           }
        }
    }
