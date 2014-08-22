#ifndef SIN_LIGHT
#define SIN_LIGHT

#include "UnityCG.cginc"
#include "AutoLight.cginc"

//----------------------------------------------------------------------------------------------------------------------
#define GAMMA 2.2
#define INV_GAMMA 0.45

//----------------------------------------------------------------------------------------------------------------------
//FORWARDBASE
#ifdef UNITY_PASS_FORWARDBASE	
	#define SIN_LIGHTING_COORDS(x,y) LIGHTING_COORDS(x, y)
    #define SIN_TRANSFER_VERTEX_TO_FRAGMENT(x)  TRANSFER_VERTEX_TO_FRAGMENT(x)
    #define SIN_LIGHT_ATTENUATION(a)            LIGHT_ATTENUATION(a)

//----------------------------------------------------------------------------------------------------------------------
//FORWARDADD (taken and modified from AutoLight.cginc)
//Since there can be only one shadow casting directional light in forward rendering, disable shadow related macros
#elif defined(UNITY_PASS_FORWARDADD)
#ifdef POINT
    #define SIN_LIGHTING_COORDS(idx1,idx2) float3 _LightCoord : TEXCOORD##idx1; 
    #define SIN_TRANSFER_VERTEX_TO_FRAGMENT(a) a._LightCoord = mul(_LightMatrix0, mul(_Object2World, v.vertex)).xyz;
    #define SIN_LIGHT_ATTENUATION(a)    (tex2D(_LightTexture0, dot(a._LightCoord,a._LightCoord).rr).UNITY_ATTEN_CHANNEL)    
#elif defined SPOT
    #define SIN_LIGHTING_COORDS(idx1,idx2) float4 _LightCoord : TEXCOORD##idx1;
    #define SIN_TRANSFER_VERTEX_TO_FRAGMENT(a) a._LightCoord = mul(_LightMatrix0, mul(_Object2World, v.vertex));
    #define SIN_LIGHT_ATTENUATION(a)    ( (a._LightCoord.z > 0) * UnitySpotCookie(a._LightCoord) * UnitySpotAttenuate(a._LightCoord.xyz)  )    
#elif defined DIRECTIONAL
	#define SIN_LIGHTING_COORDS(idx1,idx2) 
    #define SIN_TRANSFER_VERTEX_TO_FRAGMENT(a) 
    #define SIN_LIGHT_ATTENUATION(a)    1
#elif defined POINT_COOKIE
    #define SIN_LIGHTING_COORDS(idx1,idx2) float3 _LightCoord : TEXCOORD##idx1; 
    #define SIN_TRANSFER_VERTEX_TO_FRAGMENT(a) a._LightCoord = mul(_LightMatrix0, mul(_Object2World, v.vertex)).xyz; 
    #define SIN_LIGHT_ATTENUATION(a)    (tex2D(_LightTextureB0, dot(a._LightCoord,a._LightCoord).rr).UNITY_ATTEN_CHANNEL * texCUBE(_LightTexture0, a._LightCoord).w )
    
#elif defined DIRECTIONAL_COOKIE
    #define SIN_LIGHTING_COORDS(idx1,idx2) float2 _LightCoord : TEXCOORD##idx1;
    #define SIN_TRANSFER_VERTEX_TO_FRAGMENT(a) a._LightCoord = mul(_LightMatrix0, mul(_Object2World, v.vertex)).xy;
    #define SIN_LIGHT_ATTENUATION(a)    (tex2D(_LightTexture0, a._LightCoord).w )
    
#endif //end POINT
	

#endif //end UNITY_PASS_FORWARDBASE

//----------------------------------------------------------------------------------------------------------------------
//[TODO: 2014-8-22] Put in an utility file

//Calculate the normal map in world space
half3 CalculateNormalMap(sampler2D normalMap, const float2 uv, const half3 t, const half3 n ) {

    //construct the tbn matrix
    const half3 b = cross(n, t); //not cross(t,n) because in tangent space, +z points to us rather than away from us
    const half3x3 tbn = half3x3(t, b, n) ;

    //Is the v always upside down for every normal map ?
    const float3 ts_normal_map = tex2D(normalMap,uv * float2(1,-1)).xyz * 2.0 - 1.0;
    return mul(tbn,ts_normal_map);
}

//----------------------------------------------------------------------------------------------------------------------

#endif //end SIN_LIGHT