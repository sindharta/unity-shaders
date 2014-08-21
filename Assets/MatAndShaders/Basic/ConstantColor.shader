Shader "sin/Basic/ConstantColor" {
	Properties 	{
		_Color ( "Color", Color) = (1, 1, 1, 1)
	}

	SubShader {
		Tags { "RenderType"="Opaque" }
		pass {		
			Tags { "LightMode"="ForwardBase"}
			cull back

			CGPROGRAM

			#pragma target 3.0
			#pragma fragmentoption ARB_precision_hint_fastest           
            #pragma only_renderers d3d9 opengl gles

			#pragma vertex ConstantColorVS
			#pragma fragment ConstantColorPS
			#pragma multi_compile_fwdbase
			#define UNITY_PASS_FORWARDBASE

            #include "UnityCG.cginc"
            #include "AutoLight.cginc"

            float4 _Color;
            
            struct PS_IN  {
                float4 pos : SV_POSITION;
            };


            PS_IN ConstantColorVS(appdata_base v) {
                PS_IN o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                return o;
            }


            float4 ConstantColorPS(PS_IN input) : COLOR {
                return float4(_Color);
            }

			ENDCG
		}		

	}
	FallBack "Diffuse"
}
