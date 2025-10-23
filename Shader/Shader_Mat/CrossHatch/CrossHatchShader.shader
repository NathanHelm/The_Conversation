Shader "Unlit/CrossHatchShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _Hatch0 ("Hatch_0", 2D) = "white" {}
        _Hatch1 ("Hatch_1", 2D) = "white" {}
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
                float3 norm : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 nrm : NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            sampler2D _Hatch0;
            sampler2D _Hatch1;
            float4 _LightColor0;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
                o.nrm = mul(float4(v.norm, 0.0), unity_WorldToObject).xyz;
                return o;
            }

            fixed3 Hatching(float2 _uv, half _intensity)
            {
                half3 hatch0 = tex2D(_Hatch0, _uv).rgb;
                half3 hatch1 = tex2D(_Hatch1, _uv).rgb;

                half3 overbright = max(0, _intensity - 1.0);

                half3 weightsA = saturate((_intensity * 6.0) + half3(-0, -1, -2));
                half3 weightsB = saturate((_intensity * 6.0) + half3(-3, -4, -5));

                weightsA.xy -= weightsA.yz;
                weightsA.z -= weightsB.x;
                weightsB.xy -= weightsB.yz;

                hatch0 = hatch0 * weightsA;
                hatch1 = hatch1 * weightsB;

                half3 hatching = overbright + hatch0.r +
                    hatch0.g + hatch0.b +
                    hatch1.r + hatch1.g +
                    hatch1.b;

                return hatching;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                half3 diffuse = col.rgb * _LightColor0.rgb * dot(_WorldSpaceLightPos0, normalize(i.nrm));
                
                half intensity = dot(diffuse, half3(0.2326, 0.7152, 0.0722));

                col.rgb = Hatching(i.uv * 5, intensity);
                
                return col;
            }
            ENDCG
        }
    }
}
