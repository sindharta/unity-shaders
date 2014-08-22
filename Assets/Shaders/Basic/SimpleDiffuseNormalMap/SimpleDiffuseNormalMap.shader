Shader "sin/Basic/SimpleDiffuseNormalMap" {
    Properties 
    {
        _DiffuseTexture ("Diffuse Texture", 2D) = "white" {}
        _NormalTexture ("Normal Texture", 2D) = "blue" {}
        _DiffuseTint ( "Diffuse Tint", Color) = (1, 1, 1, 1)
    }

    SubShader 
    {
        Tags { "RenderType"="Opaque" }
        pass {      
            Tags { "LightMode"="ForwardBase"}
//            cull on

            CGPROGRAM

            #pragma target 3.0
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma only_renderers d3d9 opengl gles

            #pragma vertex VS
            #pragma fragment PS
            #pragma multi_compile_fwdbase
            #define UNITY_PASS_FORWARDBASE

            #include "SimpleDiffuseNormalMap.cginc"

            ENDCG
        }       

        pass {      
            Tags { "LightMode"="ForwardAdd"}
            Blend One One
            CGPROGRAM

            #pragma target 3.0
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma only_renderers d3d9 opengl gles

            #pragma vertex VS
            #pragma fragment PS
            #pragma multi_compile_fwdadd
            #define UNITY_PASS_FORWARDADD

            #include "SimpleDiffuseNormalMap.cginc"

            ENDCG
        }       
    } 
    FallBack "Diffuse"
}
