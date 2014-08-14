Shader "sin/Basic/Checker" {
	Properties {
		_FirstColor  ("First Color", Color) = (1,1,1,1)
        _SecondColor ("Second Color", Color) = (1,1,1,1)
        _NumHorizontalTiles  ("Num Horizontal Tiles", Float) = 5
        _NumVerticalTiles ("Num Vertical Tiles ", Float) = 5
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

			#pragma vertex CheckerVS
			#pragma fragment CheckerPS
			#pragma multi_compile_fwdbase
			#define UNITY_PASS_FORWARDBASE

            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Assets/MatAndShaders/SinForwardLight.cginc"

            float4 _FirstColor;
            float4 _SecondColor;
            float _NumHorizontalTiles;
            float _NumVerticalTiles;

            struct PS_IN {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD2;
            };


            PS_IN CheckerVS(appdata_base v) {
                PS_IN o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.uv = v.texcoord;
                return o;
            }


            float4 CheckerPS(PS_IN input) : COLOR {
                int2 floor_uv = int2(floor(input.uv * float2(_NumHorizontalTiles, _NumVerticalTiles)));

                float lerp_factor = (floor_uv.x % 2 + (1 - (floor_uv.y % 2))) % 2;
                float4 result = lerp(_FirstColor,_SecondColor,lerp_factor);
                return result;

            }

			ENDCG
		}		

	}
	FallBack "Diffuse"
}
