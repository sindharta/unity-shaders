
Shader "sin/Basic/SimpleDiffuse" {
	Properties 
	{
		_DiffuseTexture ("Diffuse Texture", 2D) = "white" {}
		_DiffuseTint ( "Diffuse Tint", Color) = (1, 1, 1, 1)
	}

	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		pass
		{		
			Tags { "LightMode"="ForwardBase"}
			cull back

			CGPROGRAM

			#pragma target 3.0
			#pragma fragmentoption ARB_precision_hint_fastest

			#pragma vertex SimpleDiffuseVS
			#pragma fragment SimpleDiffusePS
			#pragma multi_compile_fwdbase
			#define UNITY_PASS_FORWARDBASE

			#include "SimpleDiffuse.cginc"

			ENDCG
		}		

		pass
		{		
			Tags { "LightMode"="ForwardAdd"}
			Blend One One
			cull back
			CGPROGRAM

			#pragma target 3.0
			#pragma fragmentoption ARB_precision_hint_fastest

			#pragma vertex SimpleDiffuseVS
			#pragma fragment SimpleDiffusePS
			#pragma multi_compile_fwdadd
			#define UNITY_PASS_FORWARDADD

			#include "SimpleDiffuse.cginc"

			ENDCG
		}		
	} 
	FallBack "Diffuse"
}
