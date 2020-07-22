
Shader "sin/Basic/SimpleTexture" {
	Properties 
	{
		_MainTexture ("Main Texture", 2D) = "white" {}
	}

	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		pass {		
			Tags { "LightMode"="ForwardBase"}
			cull back

			CGPROGRAM

			#pragma target 3.0
			#pragma fragmentoption ARB_precision_hint_fastest           
            #pragma only_renderers d3d9 opengl gles

			#pragma vertex SimpleTextureVS
			#pragma fragment SimpleTexturePS
			#pragma multi_compile_fwdbase
			#define UNITY_PASS_FORWARDBASE

            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Assets/Shaders/SinForwardLight.cginc"


            sampler2D _MainTexture;
            
            struct PS_IN
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD2;
                SIN_LIGHTING_COORDS(3, 4)
            };


            PS_IN SimpleTextureVS(appdata_base v) {
                PS_IN o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.uv = v.texcoord;
                return o;
            }


            float4 SimpleTexturePS(PS_IN input) : COLOR {
                return float4(tex2D(_MainTexture, input.uv).rgb,1.0);
            }

			ENDCG
		}		

	}
	FallBack "Diffuse"
}
