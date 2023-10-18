// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

//Will output the color as it is without going back to the gamma space
#ifndef SIN_BASIC_NORMAL_MAP
#define SIN_BASIC_NORMAL_MAP

#include "UnityCG.cginc"
#include "AutoLight.cginc"
#include "Assets/Shaders/SinForwardLight.cginc"
#include "Assets/Shaders/SinShaderUtility.cginc"

sampler2D _NormalMap;
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
    SIN_SH_LIGHT_COORD(6)
};


//----------------------------------------------------------------------------------------------------------------------
PS_IN NormalMapVS(appdata_tan v)
{
	PS_IN o;

	o.pos = UnityObjectToClipPos(v.vertex);
	o.uv = v.texcoord;
	o.wsLightDir = normalize(WorldSpaceLightDir(v.vertex));

    //[Note] Won't work for non-uniform scaling because we are not using inverse transpose
	o.wsNormal = mul(float3x3(unity_ObjectToWorld), v.normal.xyz);
    o.wsTangent = mul(float3x3(unity_ObjectToWorld), v.tangent.xyz);

    //vertex lighting for additional lights
    const float4 ws_pos = mul(unity_ObjectToWorld, v.vertex);   

	TRANSFER_VERTEX_TO_FRAGMENT(o);
    SIN_TRANSFER_SH_LIGHT_TO_FRAGMENT(o,o.wsNormal);

	return o; 
}

//----------------------------------------------------------------------------------------------------------------------

float4 NormalMapPS(PS_IN input) : COLOR
{
    //prepare
    const half3 l = normalize(input.wsLightDir);
	const half3 n = normalize(input.wsNormal);
    const half3 t = normalize(input.wsTangent);

    const half3 ws_normal_map = CalculateNormalMap(_NormalMap, input.uv, t,n);

    //calculation
	const float attenuation = LIGHT_ATTENUATION(input) * 2;
	const float n_dot_l = saturate(dot(ws_normal_map, l));
	const float3 lighting_result = n_dot_l * _LightColor0.rgb * attenuation;

    //final
	float4 final_color = float4(lighting_result,1);
    SIN_SH_LIGHT(final_color,input);

	return final_color;
}

//----------------------------------------------------------------------------------------------------------------------



#endif
