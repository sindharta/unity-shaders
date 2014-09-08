//Perform lighting in linear space. Unity setting should be set to gamma setting 

#ifndef SIN_ADVANCED_COOK_TORRANCE
#define SIN_ADVANCED_COOK_TORRANCE

#include "UnityCG.cginc"
#include "AutoLight.cginc"
#include "Assets/Shaders/SinForwardLight.cginc"
#include "Assets/Shaders/SinShaderUtility.cginc"

sampler2D _DiffuseTexture;
float4 _DiffuseTint;
float4 _LightColor0;
float4 _SpecularTint;
float  _FresnelCoef;
float  _Roughness;

//----------------------------------------------------------------------------------------------------------------------

struct PS_IN
{
	float4 pos : SV_POSITION;
	half3 wsLightDir : TEXCOORD0;
	half3 wsNormal : TEXCOORD1;
    half3 wsViewDir : TEXCOORD2;
	float2 uv : TEXCOORD3;
	LIGHTING_COORDS(4, 5)
    SIN_SH_LIGHT_COORD(6)
};


//----------------------------------------------------------------------------------------------------------------------
PS_IN CookTorranceVS(appdata_base v)
{
	PS_IN o;

    const float4 ws_pos = mul(_Object2World, v.vertex);

	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
	o.uv = v.texcoord.xy;
	o.wsLightDir = normalize(WorldSpaceLightDir(v.vertex));
	o.wsNormal = mul(float3x3(_Object2World), v.normal.xyz);
    o.wsViewDir = normalize(_WorldSpaceCameraPos.xyz - ws_pos.xyz);

    //vertex lighting for additional lights
	TRANSFER_VERTEX_TO_FRAGMENT(o);
    SIN_TRANSFER_SH_LIGHT_TO_FRAGMENT(o,o.wsNormal);

	return o; 
}

//----------------------------------------------------------------------------------------------------------------------
//GPU Version of Beckmann 
float Beckmann(float nDotH, float roughness) {
    const float nDotH_2 = nDotH * nDotH;
    const float roughness_2 = roughness * roughness;
    const float exp_value = (1.0 - nDotH_2) / (roughness_2 * nDotH_2);     
    const float val = exp(-exp_value) / (PI * roughness_2 * nDotH_2 * nDotH_2);
    return val;
}


//----------------------------------------------------------------------------------------------------------------------

float4 CookTorrancePS(PS_IN input) : COLOR
{					
	const half3 l = normalize(input.wsLightDir);
	const half3 n = normalize(input.wsNormal);
    const half3 v = normalize(input.wsViewDir);

    const half3 half_vector = normalize(l + v);
    const float n_dot_h = saturate(dot(n,half_vector));
    const float n_dot_v = saturate(dot(n,v));
    const float v_dot_h = saturate(dot(v,half_vector));
    const float n_dot_l = saturate(dot(n, l));

    //calculation
	const float attenuation = LIGHT_ATTENUATION(input) * 2;
	const float3 diffuse_term = n_dot_l * _LightColor0.rgb * _DiffuseTint.rgb * attenuation;
	const float4 diffuse_tex = tex2D(_DiffuseTexture, input.uv);
	const float3 albedo = pow(diffuse_tex.rgb,GAMMA);

    //Cook Torrance specular model (http://en.wikipedia.org/wiki/Specular_highlight#Cook.E2.80.93Torrance_model)

    //Distribution of having microfacets that do pure specular reflection
    float D = Beckmann(n_dot_h,_Roughness);
   
    //Geometric attenuation
    const float G_const = 2 * n_dot_h / v_dot_h;
    const float G1 = G_const * n_dot_v;
    const float G2 = G_const * n_dot_l;
    const float G = min(1.0,min(G1,G2));

    //Fresnel
    const float F = SchlickFresnel(_FresnelCoef, v_dot_h);

    const float cook_torrance = (D * G * F) / (4.0 * n_dot_v * n_dot_l);
    const float3 specular_term = cook_torrance * _LightColor0.rgb * _SpecularTint.rgb;

    //final
	float4 final_color = float4(diffuse_term.rgb * albedo + specular_term,diffuse_tex.a);
    SIN_SH_LIGHT(final_color,input);

    final_color.rgb = pow( final_color.rgb,INV_GAMMA);

	return final_color;
}

//----------------------------------------------------------------------------------------------------------------------



#endif
