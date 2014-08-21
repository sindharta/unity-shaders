Shader "sin/Basic/ScrollingTex" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
        _Tint  (" Tint", Range(0,1)) = 1        
        _XScrollSpeed (" X Scroll Speed", Range(0,5)) = 1
        _YScrollSpeed (" Y Scroll Speed", Range(0,5)) = 1
	}
    SubShader 
    {
        Tags { "RenderType"="Opaque" }
        pass  {       
            Tags { "LightMode"="ForwardBase"}
            cull back

            CGPROGRAM

            #pragma target 3.0
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma only_renderers d3d9 opengl gles

            #pragma vertex VS
            #pragma fragment PS
            #pragma multi_compile_fwdbase
            #define UNITY_PASS_FORWARDBASE

            #include "ScrollingTex.cginc"

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

            #pragma vertex VS
            #pragma fragment PS
            #pragma multi_compile_fwdadd
            #define UNITY_PASS_FORWARDADD

            #include "ScrollingTex.cginc"

            ENDCG
        }       
    } 
	FallBack "Diffuse"
}
