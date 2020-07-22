#ifndef SIN_BASIC_SCROLLING_TEX
#define SIN_BASIC_SCROLLING_TEX

#include "UnityCG.cginc"
#include "AutoLight.cginc"
#include "Assets/Shaders/SinForwardLight.cginc"

sampler2D _MainTex;
float _Tint;
float _XScrollSpeed;
float _YScrollSpeed;

//----------------------------------------------------------------------------------------------------------------------

struct PS_IN
{
	float4 pos : SV_POSITION;
	float2 uv : TEXCOORD2;
	LIGHTING_COORDS(3, 4)
    SIN_SH_LIGHT_COORD(5)
};


//----------------------------------------------------------------------------------------------------------------------
PS_IN VS(appdata_base v)
{
	PS_IN o;

	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
	o.uv = v.texcoord;

    const float3 world_normal = mul(float3x3(_Object2World), v.normal.xyz);

	TRANSFER_VERTEX_TO_FRAGMENT(o);
    SIN_TRANSFER_SH_LIGHT_TO_FRAGMENT(o,world_normal);

	return o; 
}

//----------------------------------------------------------------------------------------------------------------------

float4 PS(PS_IN input) : COLOR
{					
    const float2 uv = input.uv + float2(_XScrollSpeed, _YScrollSpeed) * _Time;

	const float4 diffuse_tex = tex2D(_MainTex, uv);
	float4 final_color = float4(pow(diffuse_tex.rgb,GAMMA) * _Tint,diffuse_tex.a);
   
    SIN_SH_LIGHT(final_color,input);

    //back to gamma space
    final_color.rgb = pow( final_color.rgb,INV_GAMMA);

	return final_color;
}

//----------------------------------------------------------------------------------------------------------------------



#endif
