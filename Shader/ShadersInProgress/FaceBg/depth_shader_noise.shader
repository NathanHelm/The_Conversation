Shader "Unlit/depth_shader_noise"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CameraOpaqueTexture("Camera Opaque Texture", 2D) = "white" {}

        _ImageAmount ("Face Texture Amount", int) = 3
        //TODO add 2DArray
        //_FaceTextures ("Face textures", 2DArray) = "" {}
        _Face1("f1", 2D) = "white" {}
        _Face2("f2", 2D)= "white" {}
        _Face3("f3",2D)= "white" {}

        _NoiseTex_1("N1", 2D) = "white" {}
        _NoiseTex_2("N2,", 2D) = "white" {}

        _ColorWheel("color wheel", 2D) = "white" {}
        _Range("Range", Vector) = (1.,1.,0, 0.)
        _Amplitude("Amp", Float) = 1.0
        _Freq("Freq", Float) = 1.0
        _Scale("n1 Scale", Float) = 1.0

        _Scale1("n2 Scale", Float) = 1.0
        _Strength("Color wheel strength", Float) = .5
        _COORD("coordinate", Vector) = (0, 0, 0, 0)
        
   

        //==========================================
    
        //==========================================
        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        ZTest Always Cull Off ZWrite On
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 4.5
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
            sampler2D _CameraOpaqueTexture;
            sampler2D _MainTex;
            sampler2D _Face1; 
            sampler2D _Face2;
            sampler2D _Face3;

            sampler2D _NoiseTex_1;
            sampler2D _NoiseTex_2;

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
            float _Amplitude;
            float _Freq;
            float _SliceAmount;
            int _ImageAmount;
            float4 _COORD;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;//o.vertex.xy / o.vertex.w * 0.5 + 0.5;
                
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                
                return o;
            }

          
            fixed4 frag (v2f i) : SV_Target
            {
                
                float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv); 
                float sceneDepth = Linear01Depth(depth); //getting scene depth.
                
                float3 opaqueTex = tex2D(_CameraOpaqueTexture, i.uv);
                float3 colorWheelDepth = tex2D(_ColorWheel, float2(sceneDepth * _Strength, .5)); //the depth of the shader is decided by a texure's color (likely a gradient)
 
                float3 col = tex2D(_MainTex, i.uv).xyz; //get screen texture

                float lum = dot(col, float3(0.299f, 0.587f, 0.114f)); //get luminence of pixel color       
                float2 noiseUV = i.uv * _NoiseTex_1_TexelSize.xy * _MainTex_TexelSize.zw; //scale the noise of both textures...
                float2 noiseUV2 = i.uv * _NoiseTex_2_TexelSize.xy * _MainTex_TexelSize.zw;
                float3 threshold = tex2D(_NoiseTex_1, noiseUV * _Scale); 
                float3 threshold2 = tex2D(_NoiseTex_2, noiseUV2 * _Scale1);
                float thresholdLum = dot(threshold, float3(0.299f, 0.587f, 0.114f));
                float thresholdLum2 = dot(threshold2, float3(0.299f, 0.587f, 0.114f));
                lum *= pow(sin(_Time * _Freq) * _Amplitude, 2.0); //interpolate between .8-1 based on a sin wave (for a interesting lighting effect)
                float3 sceneDepthFixed = sceneDepth > .99? float3(1,1,1) : colorWheelDepth; //set the background to black. 

                float threshOnDepth = lerp(thresholdLum, thresholdLum2, clamp(0, .9, sceneDepthFixed)); //interpolate between noise textures based on object's depth
                
                float4 rgba = lum < threshOnDepth? float4(1, 1, 1, 1) : float4(opaqueTex,1.); //is the noise texture darker than the sin wave's value? 
                
              //  float3 rgb = float3(v,v,v);
              // return float4(sceneDepth,sceneDepth,sceneDepth,1.);
                return rgba;
           
            }
            ENDCG
        }
    }
}
