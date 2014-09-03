
Shader "sin/Advanced/CookTorrance" {
	Properties 
	{
		_DiffuseTexture ("Diffuse Texture", 2D) = "white" {}
		_DiffuseTint  ( "Diffuse Tint", Color) = (1, 1, 1, 1)
        _SpecularTint ( "Specular Tint", Color) = (1, 1, 1, 1)
        _FresnelCoef  ( "Fresnel Coef", Float) = 1
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

			#pragma vertex CookTorranceVS
			#pragma fragment CookTorrancePS
			#pragma multi_compile_fwdbase
			#define UNITY_PASS_FORWARDBASE

			#include "CookTorrance.cginc"

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

			#pragma vertex CookTorranceVS
			#pragma fragment CookTorrancePS
			#pragma multi_compile_fwdadd
			#define UNITY_PASS_FORWARDADD

			#include "CookTorrance.cginc"

			ENDCG
		}		
	} 
	FallBack "Diffuse"
}
