//Perform lighting in linear space. Unity setting should be set to gamma setting 

#ifndef SIN_BASIC_SIMPLE_DIFFUSE
#define SIN_BASIC_SIMPLE_DIFFUSE

#include "UnityCG.cginc"
#include "AutoLight.cginc"
#include "Assets/Shaders/SinForwardLight.cginc"

sampler2D _DiffuseTexture;
float4 _DiffuseTint;
float4 _LightColor0;

//----------------------------------------------------------------------------------------------------------------------

struct PS_IN
{
	float4 pos : SV_POSITION;
	half3 wsLightDir : TEXCOORD0;
	half3 wsNormal : TEXCOORD1;
	float2 uv : TEXCOORD2;
	LIGHTING_COORDS(3, 4)
    SIN_SH_LIGHT_COORD(5)
};


//----------------------------------------------------------------------------------------------------------------------
PS_IN SimpleDiffuseVS(appdata_base v)
{
	PS_IN o;

	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
	o.uv = v.texcoord.xy;
	o.wsLightDir = normalize(WorldSpaceLightDir(v.vertex));
	o.wsNormal = mul(float3x3(_Object2World), v.normal.xyz);

    //vertex lighting for additional lights
	TRANSFER_VERTEX_TO_FRAGMENT(o);
    SIN_TRANSFER_SH_LIGHT_TO_FRAGMENT(o,o.wsNormal);

	return o; 
}

//----------------------------------------------------------------------------------------------------------------------

float4 SimpleDiffusePS(PS_IN input) : COLOR
{					
	half3 l = normalize(input.wsLightDir);
	half3 n = normalize(input.wsNormal);	 

    //calculation
	const float attenuation = LIGHT_ATTENUATION(input) * 2;
	const float n_dot_l = saturate(dot(n, l));	
	const float3 diffuse_term = n_dot_l * _LightColor0.rgb * _DiffuseTint.rgb * attenuation;
	const float4 diffuse_tex = tex2D(_DiffuseTexture, input.uv);
	const float3 albedo = pow(diffuse_tex.rgb,GAMMA);

    //final
	float4 final_color = float4(diffuse_term.rgb * albedo,diffuse_tex.a);
    SIN_SH_LIGHT(final_color,input);

    final_color.rgb = pow( final_color.rgb,INV_GAMMA);

	return final_color;
}

//----------------------------------------------------------------------------------------------------------------------



#endif
