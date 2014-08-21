#ifndef SIN_BASIC_SCROLLING_TEX
#define SIN_BASIC_SCROLLING_TEX

#include "UnityCG.cginc"
#include "AutoLight.cginc"
#include "Assets/MatAndShaders/SinForwardLight.cginc"

sampler2D _MainTex;
float _Tint;
float _XScrollSpeed;
float _YScrollSpeed;

//----------------------------------------------------------------------------------------------------------------------

struct PS_IN
{
	float4 pos : SV_POSITION;
	float2 uv : TEXCOORD2;
	SIN_LIGHTING_COORDS(3, 4)
    
#ifdef UNITY_PASS_FORWARDBASE 
    half3 ambientColor : TEXCOORD5; //for non per-pixel lights
#endif
};


//----------------------------------------------------------------------------------------------------------------------
PS_IN VS(appdata_base v)
{
	PS_IN o;

	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
	o.uv = v.texcoord;

#ifdef UNITY_PASS_FORWARDBASE
    //(UNITY_LIGHTMODEL_AMBIENT.rgb * 2) already included in SH ?
    float3 world_normal = mul(float3x3(_Object2World), v.normal.xyz);
    o.ambientColor = ShadeSH9 (half4(world_normal,1.0));
#endif

	SIN_TRANSFER_VERTEX_TO_FRAGMENT(o);

	return o; 
}

//----------------------------------------------------------------------------------------------------------------------

float4 PS(PS_IN input) : COLOR
{					
    const float2 uv = input.uv + float2(_XScrollSpeed, _YScrollSpeed) * _Time;

	const float4 diffuse_tex = tex2D(_MainTex, uv) * _Tint;
	float4 final_color = diffuse_tex; 
   
    
#ifdef UNITY_PASS_FORWARDBASE
    final_color.rgb += input.ambientColor.rgb;
#endif
    
	return final_color;
}

//----------------------------------------------------------------------------------------------------------------------



#endif
