// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

#ifndef SIN_BASIC_SIMPLE_DIFFUSE_NORMAL
#define SIN_BASIC_SIMPLE_DIFFUSE_NORMAL

#include "UnityCG.cginc"
#include "AutoLight.cginc"
#include "Assets/Shaders/SinForwardLight.cginc"
#include "Assets/Shaders/SinShaderUtility.cginc"

sampler2D _DiffuseTexture;
sampler2D _NormalTexture;
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
    SIN_SH_LIGHT_COORD(6)

};


//----------------------------------------------------------------------------------------------------------------------
PS_IN VS(appdata_tan v)
{
    PS_IN o;

    o.pos = UnityObjectToClipPos(v.vertex);
    o.uv = v.texcoord;
    o.wsLightDir = normalize(WorldSpaceLightDir(v.vertex));
    o.wsNormal = mul(float3x3(unity_ObjectToWorld), v.normal.xyz);
    o.wsTangent = mul(float3x3(unity_ObjectToWorld), v.tangent.xyz);

    TRANSFER_VERTEX_TO_FRAGMENT(o);
    SIN_TRANSFER_SH_LIGHT_TO_FRAGMENT(o,o.wsNormal);

    return o; 
}

//----------------------------------------------------------------------------------------------------------------------

float4 PS(PS_IN input) : COLOR
{                   
    const half3 l = normalize(input.wsLightDir);
    const half3 n = normalize(input.wsNormal);     
    const half3 t = normalize(input.wsTangent);     
    
    const half3 ws_normal_map = CalculateNormalMap(_NormalTexture, input.uv, t,n);

    //calculation
    const float attenuation = LIGHT_ATTENUATION(input) * 2;     
    const float n_dot_l = saturate(dot(ws_normal_map, l));  
    const float3 diffuse_term = n_dot_l * _LightColor0.rgb * _DiffuseTint.rgb * attenuation;
    const float4 diffuse_tex = tex2D(_DiffuseTexture, input.uv);
    const float3 albedo = pow(diffuse_tex.rgb,GAMMA);

    //final
    float4 final_color;    
    final_color.rgb = diffuse_term * albedo;
    final_color.a = diffuse_tex.a;
    SIN_SH_LIGHT(final_color,input);

    final_color.rgb = pow( final_color.rgb,INV_GAMMA);

    return final_color;
}

//----------------------------------------------------------------------------------------------------------------------



#endif
