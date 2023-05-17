Shader "Unlit/FlameShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FlameOpacity ("Flame Opacity", Range(0.0, 1.0)) = 1.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            ZWrite Off
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha 
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _FlameOpacity;

            float generateNoise(float2 uv) {
                return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43.5453);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Generate noise
                float2 uvSegment = floor(v.uv * 6.0); // Assuming 6 segments (hexagons)
                float noise = generateNoise(uvSegment + _Time.yyy * 0.001);

                // Offset vertex position horizontally
                o.vertex.x += noise * 0.007;  // Adjust this multiplier as needed

                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Define the center of the texture
                float2 center = float2(0.5, 0.5);

                // Calculate the UV coordinates relative to the center
                float2 distUv = i.uv - center;

                // Calculate the distance from the center of the texture
                float dist = length(distUv);

                fixed4 colorOrange = fixed4(1.0, 0.5, 0.0, 1.0);
                fixed4 colorRed = fixed4(1.0, 0.0, 0.0, 1.0);

                fixed4 color;

                float t = smoothstep(0.15, 1.0, dist);
                color = lerp(colorOrange, colorRed, t);

                if(dist > 0.80) {
                    discard;
                }

                dist += 0.1;
                color.a = lerp(1.0, -0.08, dist);

                color.a *= _FlameOpacity;

                return color;
            }
            ENDCG
        }
    }
}
