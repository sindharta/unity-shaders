// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "sin/Basic/ColorArray" {
    Properties
    {
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
            #pragma only_renderers d3d9 d3d11 opengl gles
            #pragma vertex VS
            #pragma fragment PS
            #pragma multi_compile_fwdbase
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"

            float4  g_colors[4];
            int     g_numColors;
//----------------------------------------------------------------------------------------------------------------------
			struct VS_IN
            {
			    float4 pos : POSITION;
			    fixed4 col : COLOR;
			};

//----------------------------------------------------------------------------------------------------------------------

			struct PS_IN
			{
				float4 pos : SV_POSITION;
				float4 col : COLOR;
			};

//----------------------------------------------------------------------------------------------------------------------
			PS_IN VS(VS_IN v)
			{
				PS_IN o;

				o.pos = UnityObjectToClipPos(v.pos);
                float t = v.col.r * g_numColors;
                int index = int(t);
                o.col = g_colors[index];
				return o;
			}

//----------------------------------------------------------------------------------------------------------------------

			float4 PS(PS_IN input) : COLOR
			{
                return input.col;
			}

//----------------------------------------------------------------------------------------------------------------------

            ENDCG
        }
    }
    FallBack "Diffuse"
}