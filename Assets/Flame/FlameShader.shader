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

                float waveSpeed = 30.0f; // control the speed of the wave
                float waveAmplitude = 0.35f; // control the amplitude of the wave
                float noiseScale = 0.25f;

                // _RandomPhaseOffset is between 0 and 100, normalize it.
                // Multiply by 1.3f to increase variability and add to speed
                // so that each flame flickers at different speeds.
                waveSpeed *= (_RandomPhaseOffset / 100.0f) * 1.3f;

                // Compute a wave position based on the hexagon's y position and the current time
                float wavePosition = (_HexagonYPosition + ((_Time.y + _RandomPhaseOffset) 
                    * waveSpeed)) / 10.f;

                // Compute a displacement based on the wave position
                float displacement = sin(wavePosition) * waveAmplitude;

                displacement *= (_SampledNoise * noiseScale);

                // Modulate the displacement with y-coordinate
                // This causes the displacement to be 0 at the base of the flame (y = 0) and gradually increase towards the tip of the flame
                float displacementFactor = smoothstep(-0.55, 1.0, _HexagonYPosition / _FlameHeight);

                // Apply the displacement factor to the displacement
                displacement *= displacementFactor;

                // Adjust the vertex position based on the displacement
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
