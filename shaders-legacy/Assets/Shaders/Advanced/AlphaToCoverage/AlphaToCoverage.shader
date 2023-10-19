//Source: https://bgolus.medium.com/anti-aliased-alpha-test-the-esoteric-alpha-to-coverage-8b177335ae4f
Shader "sin/Advanced/AlphaToCoverage" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Cutoff ("Alpha cutoff", Range(0.15,0.85)) = 0.4
        _MipScale ("Mip Level Alpha Scale", Range(0,1)) = 0.25
    }
    SubShader {
        Tags {
            "RenderQueue"="AlphaTest" "RenderType"="TransparentCutout"
        }
        Cull Off

        Pass {
            Tags {
                "LightMode"="ForwardBase"
            }
            AlphaToMask On

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            struct appdata{
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                half3 normal : NORMAL;
            };
    
            struct v2f{
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                half3 worldNormal : NORMAL;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;

            fixed _Cutoff;
            half _MipScale;

            v2f vert(appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag(v2f i, fixed facing : VFACE) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv);
                half3 worldNormal = normalize(i.worldNormal * facing);

                fixed ndotl = saturate(dot(worldNormal, normalize(_WorldSpaceLightPos0.xyz)));
                fixed3 lighting = ndotl * _LightColor0;
                lighting += ShadeSH9(half4(worldNormal, 1.0));

                col.rgb *= lighting;
                return col;
            }
            ENDCG
        }
    }
}