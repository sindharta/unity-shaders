#ifndef SIN_SHADER_UTILITY
#define SIN_SHADER_UTILITY

//----------------------------------------------------------------------------------------------------------------------

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

#endif //end SIN_SHADER_UTILITY