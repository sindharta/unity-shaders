#ifndef SINDHARTA_BASIC_SIMPLE_DIFFUESE
#define SINDHARTA_BASIC_SIMPLE_DIFFUESE

#include "UnityCG.cginc"
#include "AutoLight.cginc"

sampler2D _DiffuseTexture;
float4 _DiffuseTint;
float4 _LightColor0;

struct PS_IN
{
	float4 pos : SV_POSITION;
	half3 wsLightDir : TEXCOORD0;
	half3 wsNormal : TEXCOORD1;
	float2 uv : TEXCOORD2;
	LIGHTING_COORDS(3, 4)
};


#define GAMMA 2.2
#define INV_GAMMA 0.45

PS_IN SimpleDiffuseVS(appdata_base v)
{
	PS_IN o;

	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
	o.uv = v.texcoord;
	o.wsLightDir = normalize(WorldSpaceLightDir(v.vertex));
	o.wsNormal = mul(float3x3(_Object2World), v.normal.xyz);

	TRANSFER_VERTEX_TO_FRAGMENT(o);

	return o; 
}

float4 SimpleDiffusePS(PS_IN i) : COLOR
{					
	half3 l = normalize(i.wsLightDir);
	half3 n = normalize(i.wsNormal);	 

	//ambient
	const float3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb * 2;

	//attenuation
	const float attenuation = LIGHT_ATTENUATION(i) * 2;
	const float n_dot_l = saturate(dot(n, l));	
	const float3 diffuse_term = n_dot_l * _LightColor0.rgb * _DiffuseTint.rgb * attenuation;
	const float4 diffuse_tex = tex2D(_DiffuseTexture, i.uv);
	const float3 albedo = pow(diffuse_tex.rgb,INV_GAMMA);

	float4 final_color;
	final_color.rgb = pow((ambient + diffuse_term) * albedo,GAMMA);
	final_color.a = diffuse_tex.a;

	return final_color;
}



#endif
