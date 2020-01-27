
Shader "sin/Basic/NormalMap" {
	Properties 
	{
		_NormalMap ("NormalMap", 2D) = "blue" {}
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

			#pragma vertex NormalMapVS
			#pragma fragment NormalMapPS
			#pragma multi_compile_fwdbase
			#define UNITY_PASS_FORWARDBASE

			#include "NormalMap.cginc"

			ENDCG
		}		

		pass {		
			Tags { "LightMode"="ForwardAdd"}
			Blend One One
			cull back
			CGPROGRAM

			#pragma target 3.0
			#pragma fragmentoption ARB_precision_hint_fastest
            #pragma only_renderers d3d9 opengl gles

			#pragma vertex NormalMapVS
			#pragma fragment NormalMapPS
			#pragma multi_compile_fwdadd
			#define UNITY_PASS_FORWARDADD

			#include "NormalMap.cginc"

			ENDCG
		}		
	} 
	FallBack "Diffuse"
}
