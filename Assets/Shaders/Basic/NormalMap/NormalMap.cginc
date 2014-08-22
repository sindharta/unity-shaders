#ifndef SIN_BASIC_NORMAL_MAP
#define SIN_BASIC_NORMAL_MAP

#include "UnityCG.cginc"
#include "AutoLight.cginc"
#include "Assets/Shaders/SinForwardLight.cginc"

sampler2D _NormalMap;
float4 _DiffuseTint;
float4 _LightColor0;


//----------------------------------------------------------------------------------------------------------------------

struct PS_IN
{
	float4 pos : SV_POSITION;
	half3 wsLightDir : TEXCOORD0;
	half3 wsNormal : TEXCOORD1;
    half3 wsTangent : TEXCOORD2;
	float2 uv : TEXCOORD3;
	LIGHTING_COORDS(4, 5)
    
#ifdef UNITY_PASS_FORWARDBASE 
    half3 ambientColor : TEXCOORD5; //for non per-pixel lights
#endif
};


//----------------------------------------------------------------------------------------------------------------------
PS_IN NormalMapVS(appdata_tan v)
{
	PS_IN o;

	o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
	o.uv = v.texcoord;
	o.wsLightDir = normalize(WorldSpaceLightDir(v.vertex));

    //[Note] Won't work for non-uniform scaling because we are not using inverse transpose
	o.wsNormal = mul(float3x3(_Object2World), v.normal.xyz);
    o.wsTangent = mul(float3x3(_Object2World), v.tangent.xyz);

    //vertex lighting for additional lights
    const float4 ws_pos = mul(_Object2World, v.vertex);   

#ifdef UNITY_PASS_FORWARDBASE
    //(UNITY_LIGHTMODEL_AMBIENT.rgb * 2) already included in SH ?
    o.ambientColor = ShadeSH9 (half4(o.wsNormal.xyz,1.0));
#endif

	TRANSFER_VERTEX_TO_FRAGMENT(o);

	return o; 
}

//----------------------------------------------------------------------------------------------------------------------

float4 NormalMapPS(PS_IN input) : COLOR
{
    const half3 l = normalize(input.wsLightDir);

    //construct the tbn matrix
	const half3 v_normal = normalize(input.wsNormal);
    const half3 t = normalize(input.wsTangent);
    const half3 b = cross(t,v_normal);
    const half3x3 tbn = half3x3(t,b,v_normal);

    //Calculate the perturbed normal in world space
    const float4 ts_normal = ((tex2D(_NormalMap, float2(input.uv.x, input.uv.y)) * 2.0) - 1.0) * float4(0,0,0,1);

    return float4 (ts_normal);

    const half3 n = mul(tbn,ts_normal.xyz);


    //calculation
	const float attenuation = LIGHT_ATTENUATION(input) * 2;
	const float n_dot_l = saturate(dot(n, l));
	const float3 diffuse_term = n_dot_l * _LightColor0.rgb * _DiffuseTint.rgb * attenuation;

    //final
	float4 final_color = float4(0,0,0,1);
	final_color = float4(diffuse_term.rgb,1);

#ifdef UNITY_PASS_FORWARDBASE
    final_color.rgb += input.ambientColor.rgb;
#endif

	return final_color;
}

//----------------------------------------------------------------------------------------------------------------------



#endif
