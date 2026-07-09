Shader "Custom/BarrierShader"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        [MainTexture] _BaseMap("Base Map", 2D) = "white" {}

        _BallColor("Ball Color", Color) = (1,1,1,1)
        _EmissionStrength("Emission Strength", Float) = 2

        _PlayerColor("Player Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
        }

        Pass
        {
            Name "barrierPass"

            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            float4 _Balls[100];
            float _BallCount;

            float4 _PlayerPos;
            float _PlayerRadius;
            float _PlayerActive;

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                half4 _BallColor;
                half _EmissionStrength;
                half4 _PlayerColor;
                float4 _BaseMap_ST;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                //water uv texture
                float2 uv = IN.uv;
                
                float t = _Time.y * 0.25;
                
                float n1 = sin(uv.y * 6.0 + t);
                float n2 = cos(uv.x * 5.0 - t);
                float n3 = sin((uv.x + uv.y) * 4.0 + t * 1.3);
                
                float2 flow = float2(
                    n1 + n3,
                    n2 - n3
                );
                
                float n4 = sin(uv.x * 12.0 + uv.y * 8.0 + t * 2.0);
                float2 turbulence = float2(n4, -n4);
                
                float2 distortion = (flow + turbulence) * 0.03;
                
                float2 distortedUV = uv + distortion;

                half4 baseCol = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, distortedUV) * _BaseColor;

                //drawing balls onto the texture
                float ballMask = 0;

                for (int i = 0; i < (int)_BallCount; i++)
                {
                    float2 center = _Balls[i].xy;
                    float radius = _Balls[i].z;

                    float dist = distance(uv, center);
                    float circle = 1.0 - smoothstep(radius - 0.01, radius, dist);

                    ballMask = max(ballMask, circle);
                }

                //fake emission
                float3 color = baseCol.rgb;
                color += _BallColor.rgb * ballMask * _EmissionStrength;
                
                //drawing player radius onto texture
                float playerMask = 0;
                float emissionMask = 0;

                if (_PlayerActive > 0.5)
                {
                    float2 p = _PlayerPos.xy;
                    float d = distance(uv, p);
                    float r = _PlayerRadius;
                    
                    playerMask = 1.0 - smoothstep(r, r + 0.02, d);
                    
                    float edgeWidth = 0.015;

                    float innerEdge = smoothstep(r - edgeWidth, r, d);
                    float outerEdge = smoothstep(r, r + edgeWidth, d);

                    emissionMask = innerEdge * (1.0 - outerEdge);
                }
                
                color = lerp(color, _PlayerColor.rgb, 0.25 * playerMask);
                
                //player fake emission
                float3 emission = _PlayerColor.rgb * emissionMask * 2.5;

                color += emission;

                //transparancy of background
                float alpha = baseCol.a * 0.4;
                alpha = max(alpha, ballMask);

                return half4(color, alpha);
            }

            ENDHLSL
        }
    }
}