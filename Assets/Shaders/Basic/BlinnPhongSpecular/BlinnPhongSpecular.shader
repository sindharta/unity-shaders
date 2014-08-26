
Shader "sin/Basic/BlinnPhongSpecular" {
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

			#pragma vertex BlinnPhongSpecularVS
			#pragma fragment BlinnPhongSpecularPS
			#pragma multi_compile_fwdbase
			#define UNITY_PASS_FORWARDBASE

			#include "BlinnPhongSpecular.cginc"

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

			#pragma vertex BlinnPhongSpecularVS
			#pragma fragment BlinnPhongSpecularPS
			#pragma multi_compile_fwdadd
			#define UNITY_PASS_FORWARDADD

			#include "BlinnPhongSpecular.cginc"

			ENDCG
		}		
	} 
	FallBack "Diffuse"
}
