Shader "Unlit/FlameShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FlameOpacity ("Flame Opacity", Range(0.0, 1.0)) = 1.0
        _SampledNoise ("Noise", Range(0.0, 1.1)) = 0.0
        _HexagonYPosition ("Hexagon Y Position", Range(0.0, 100.0)) = 0.0
        _FlameHeight ("FlameHeight", Range(0.0, 100.0)) = 0.0
        _RandomPhaseOffset ("Random Phase Offset", Range(0.0, 100.0)) = 0.0
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
            float _SampledNoise;
            float _HexagonYPosition;
            float _FlameHeight;
            float _RandomPhaseOffset;

            v2f vert (appdata v)
            {
                v2f o;

                _SampledNoise *= 0.4f;

                // Compute a delay based on the hexagon's y position
                float delay = _HexagonYPosition * 2.0f; 
                float speedFactor = 0.1f;

                // Compute a factor that increases from 0 to 1 as the y position increases from 0 to a maximum, then decreases back to 0
                // This uses a cosine function, which has the desired property
                // The factor of 0.2 is just an example;
                float factor = cos(_HexagonYPosition * 0.2f);

                // Modulate displacement with y-coordinate
                float scaledPosition = (_HexagonYPosition / _FlameHeight) * 2.0 - 1.0;  // scale to range from -1 to 1
                float displacementFactor = smoothstep(-2.0, 1.0, scaledPosition);

                // Apply the noise value with the delay
                float displacement = _SampledNoise * factor * 
                    sin((_Time.y + delay + _RandomPhaseOffset) * speedFactor) * 
                    displacementFactor * 0.1f;

                o.vertex = UnityObjectToClipPos(v.vertex + float4(displacement, 0, 0, 0));
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
