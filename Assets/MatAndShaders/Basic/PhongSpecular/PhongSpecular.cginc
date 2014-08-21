//Perform lighting in linear space. Unity setting should be set to gamma setting

#ifndef SIN_BASIC_PHONG_SPECULAR
#define SIN_BASIC_PHONG_SPECULAR

#include "UnityCG.cginc"
#include "AutoLight.cginc"
#include "Assets/MatAndShaders/SinForwardLight.cginc"

sampler2D _DiffuseTexture;
float4 _DiffuseTint;
float4 _SpecularTint;
float  _SpecularPower;
float4 _LightColor0;


//----------------------------------------------------------------------------------------------------------------------

struct PS_IN {
	float4 pos : SV_POSITION;
	half3 wsLightDir : TEXCOORD0;
	half3 wsNormal  : TEXCOORD1;
	half3 wsViewDir : TEXCOORD2;
	float2 uv : TEXCOORD3;
	LIGHTING_COORDS(4, 5)

#ifdef UNITY_PASS_FORWARDBASE
    half3 ambientColor : TEXCOORD6; //for non per-pixel lights
#endif
};

//----------------------------------------------------------------------------------------------------------------------

PS_IN PhongSpecularVS(appdata_base v) {
	PS_IN o;
    
    const float4 ws_pos = mul(_Object2World, v.vertex);

	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
	o.uv = v.texcoord;
	o.wsLightDir = normalize(WorldSpaceLightDir(v.vertex));
	o.wsNormal  = mul(float3x3(_Object2World), v.normal.xyz);
	o.wsViewDir = normalize(_WorldSpaceCameraPos.xyz - ws_pos.xyz);

//vertex lighting for additional lights
#ifdef UNITY_PASS_FORWARDBASE
    //(UNITY_LIGHTMODEL_AMBIENT.rgb * 2) already included in SH ?
    o.ambientColor = ShadeSH9 (half4(o.wsNormal.xyz,1.0));
#endif

	TRANSFER_VERTEX_TO_FRAGMENT(o);

	return o;
}

//----------------------------------------------------------------------------------------------------------------------

float4 PhongSpecularPS(PS_IN input) : COLOR {
	const half3 l = normalize(input.wsLightDir);
	const half3 n = normalize(input.wsNormal);
	const half3 v = normalize(input.wsViewDir);

    //diffuse
	const float attenuation = LIGHT_ATTENUATION(input) * 2;
	const float n_dot_l = dot(n, l);
	const float3 diffuse_term = saturate(n_dot_l) * _LightColor0.rgb * _DiffuseTint.rgb * attenuation;
	const float4 diffuse_tex = tex2D(_DiffuseTexture, input.uv);
	const float3 albedo = pow(diffuse_tex.rgb,GAMMA);

	//specular
	const half3 r = normalize((2 * n * n_dot_l) - l);
    const float r_dot_v = dot(r,v);
    const float3 spec_term = pow(saturate(r_dot_v),_SpecularPower);

    //final
	float4 final_color;
	final_color.rgb = diffuse_term * albedo + spec_term * _SpecularTint;
    final_color.a = diffuse_tex.a;

#ifdef UNITY_PASS_FORWARDBASE
    final_color.rgb += input.ambientColor.rgb;
#endif

	//back to gamma space
	final_color.rgb = pow( final_color.rgb,INV_GAMMA);

	return final_color;
}

//----------------------------------------------------------------------------------------------------------------------

#endif