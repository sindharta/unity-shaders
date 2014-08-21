//Perform lighting in linear space. Unity setting should be set to gamma setting 

#ifndef SIN_BASIC_SIMPLE_DIFFUSE
#define SIN_BASIC_SIMPLE_DIFFUSE

#include "UnityCG.cginc"
#include "AutoLight.cginc"
#include "Assets/MatAndShaders/SinForwardLight.cginc"

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
	SIN_LIGHTING_COORDS(3, 4)
    
#ifdef UNITY_PASS_FORWARDBASE 
    half3 ambientColor : TEXCOORD5; //for non per-pixel lights
#endif
};


//----------------------------------------------------------------------------------------------------------------------
PS_IN SimpleDiffuseVS(appdata_base v)
{
	PS_IN o;

	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
	o.uv = v.texcoord;
	o.wsLightDir = normalize(WorldSpaceLightDir(v.vertex));
	o.wsNormal = mul(float3x3(_Object2World), v.normal.xyz);

    //vertex lighting for additional lights
    const float4 ws_pos = mul(_Object2World, v.vertex);   

#ifdef UNITY_PASS_FORWARDBASE
    //(UNITY_LIGHTMODEL_AMBIENT.rgb * 2) already included in SH ?
    o.ambientColor = ShadeSH9 (half4(o.wsNormal.xyz,1.0));
#endif

	SIN_TRANSFER_VERTEX_TO_FRAGMENT(o);

	return o; 
}

//----------------------------------------------------------------------------------------------------------------------

float4 SimpleDiffusePS(PS_IN input) : COLOR
{					
	half3 l = normalize(input.wsLightDir);
	half3 n = normalize(input.wsNormal);	 

    //calculation
	const float attenuation = SIN_LIGHT_ATTENUATION(input) * 2;
	const float n_dot_l = saturate(dot(n, l));	
	const float3 diffuse_term = n_dot_l * _LightColor0.rgb * _DiffuseTint.rgb * attenuation;
	const float4 diffuse_tex = tex2D(_DiffuseTexture, input.uv);
	const float3 albedo = pow(diffuse_tex.rgb,GAMMA);

    //final
	float4 final_color;
	final_color.rgb = pow( diffuse_term * albedo,INV_GAMMA);
//    final_color.rgb = diffuse_term * albedo;
    final_color.a = diffuse_tex.a;
    
#ifdef UNITY_PASS_FORWARDBASE
    final_color.rgb += input.ambientColor.rgb;
#endif

	return final_color;
}

//----------------------------------------------------------------------------------------------------------------------



#endif
