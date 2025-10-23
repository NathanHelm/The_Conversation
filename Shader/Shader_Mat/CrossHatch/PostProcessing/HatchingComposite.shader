// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/HatchingComposite"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Hatch0("Hatch 0 (light)", 2D) = "white" {}
		_Hatch1("Hatch 1", 2D) = "white" {}
		_Background("Background", 2D) = "white" {}
		_T("t", Float) = 0

	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uvFlipY : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.uvFlipY = o.uv;
 
				return o;
			}
			sampler2D _Hatch0;
			sampler2D _Hatch1;
			float _T;

			fixed3 Hatching(float2 _uv, half _intensity)
			{
				half3 hatch0 = tex2D(_Hatch0, _uv).rgb;
				half3 hatch1 = tex2D(_Hatch1, _uv).rgb;

				half3 overbright = max(0, _intensity - 1.0);

				half3 weightsA = saturate((_intensity * 6.0) + half3(-0, -1, -2));
				half3 weightsB = saturate((_intensity * 6.0) + half3(-3, -4, -5));

				weightsA.xy -= weightsA.yz;
				weightsA.z	-= weightsB.x;
				weightsB.xy -= weightsB.yz;

				hatch0 = hatch0 * weightsA;
				hatch1 = hatch1 * weightsB;

				half3 hatching = overbright + hatch0.r +
								 hatch0.g	+ hatch0.b +
								 hatch1.r	+ hatch1.g +
								 hatch1.b;

				return hatching;

			}

			sampler2D _MainTex;
			sampler2D _UVBuffer;
			sampler2D _Background;
			
			sampler2D _HatchTex1;
			sampler2D _HatchTex2;
			sampler2D _CameraDepthTexture;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 bgCol = tex2D(_Background, i.uv);

				float4 uv = tex2D(_UVBuffer, i.uvFlipY);

				half intensity = dot(col.rgb, float3(0.2326, 0.7152, 0.0722));
		
				float depth = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, i.uv));
				depth = pow(Linear01Depth(depth), 0.75);


				/*

				if(depth > 0.02)
				{
					float4 depthBg = float4(bgCol.xyz, 1);
					float4 lrpCol = lerp(depthBg, col, _T); //yes, I enjoy lerping quite often...
					return lrpCol;

				}
					*/

				intensity = depth > .99? 0 : 1;

				fixed3 hatch = Hatching(uv.xy * 10, intensity);

				if(uv.x > 0)
				{
					
					return float4(hatch.xyz, 1);
				}
				else
				{
					return float4(1, 1, 1, 1);
				}

				
				
				//col.rgb = hatch;
				float hatchLum = saturate(dot(hatch.rgb, float3(0.2326, 0.7152, 0.0722)));
				
				/*
				if(hatchLum > 0.4)
				{
					float4 lrpCol = lerp(float4(1, 1, 1, 1), col, _T); //yes, I enjoy lerping quite often...
					return float4(1, 1, 1, 1);
				}
					*/

				float3 lrpCol3 = lerp(hatch.rgb, col.rgb, _T);
				return float4(lrpCol3,1);
			}
				
			ENDCG
		}
	}
}
