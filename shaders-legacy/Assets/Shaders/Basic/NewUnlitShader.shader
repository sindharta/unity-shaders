Shader "Tutorial/046_Partial_Derivatives/testing"{
	Properties{
		_Factor("Factor", Range(0, 1000)) = 1
	}

	SubShader{
		Tags{ "RenderType"="Opaque" "Queue"="Geometry"}
		
		Cull Off

		Pass{
			CGPROGRAM

			//include useful shader functions
			#include "UnityCG.cginc"

			//define vertex and fragment shader
			#pragma vertex vert
			#pragma fragment frag

			float _Factor;

			struct appdata{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f{
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

//--------------------------------------------------------------------------------------------------------------------------------------------------------------			
			v2f vert(appdata v){
				v2f o;
				o.position = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			float4 frag(v2f i) : SV_TARGET{
                //zoom in  -> bigger area (smaller derivative value) -> darker
                //zoom out -> smaller area (higher derivative value) -> brighter
			    float derivative = fwidth(i.uv.x) * _Factor;
				
				float4 col = float4(derivative.xxx , 1);
				return col;
			}

			ENDCG
		}
	}
}