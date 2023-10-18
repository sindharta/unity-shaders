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

			//the object data that's put into the vertex shader
			struct appdata{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			//the data that's used to generate fragments and can be read by the fragment shader
			struct v2f{
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			//the vertex shader
			v2f vert(appdata v){
				v2f o;
				o.position = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			//the fragment shader
			fixed4 frag(v2f i) : SV_TARGET{
                //calculate the change of the uv coordinate to the next pixel
			    float derivative = fwidth(i.uv.x) * _Factor;
			    //transform derivative to greyscale color
				fixed4 col = float4(derivative.xxx , 1);
				return col;
			}

			ENDCG
		}
	}
}