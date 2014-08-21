
Shader "sin/Basic/PhongSpecular" {
	Properties 
	{
		_DiffuseTexture ("Diffuse Texture", 2D) = "white" {}
		_DiffuseTint ( "Diffuse Tint", Color) = (1, 1, 1, 1)
		_SpecularTint ( "Specular Tint", Color) = (1, 1, 1, 1)
	    _SpecularPower ("Specular Power", Float) = 10
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

			#pragma vertex PhongSpecularVS
			#pragma fragment PhongSpecularPS
			#pragma multi_compile_fwdbase
			#define UNITY_PASS_FORWARDBASE

			#include "PhongSpecular.cginc"

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

			#pragma vertex PhongSpecularVS
			#pragma fragment PhongSpecularPS
			#pragma multi_compile_fwdadd
			#define UNITY_PASS_FORWARDADD

			#include "PhongSpecular.cginc"

			ENDCG
		}		
	} 
	FallBack "Diffuse"
}
