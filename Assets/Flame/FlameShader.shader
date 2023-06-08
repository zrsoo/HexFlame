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

        _RedChannel ("Red Channel", Range(0.0, 1.0)) = 1.0
        _GreenChannel ("Green Channel", Range(0.0, 1.0)) = 0.5
        _BlueChannel ("Blue Channel", Range(0.0, 1.0)) = 0.0

        _OuterRedChannel ("Outer Red Channel", Range(0.0, 1.0)) = 1.0
        _OuterGreenChannel ("Outer Green Channel", Range(0.0, 1.0)) = 0.0
        _OuterBlueChannel ("Outer Blue Channel", Range(0.0, 1.0)) = 0.0
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

            float _RedChannel;
            float _GreenChannel;
            float _BlueChannel;

            float _OuterRedChannel;
            float _OuterGreenChannel;
            float _OuterBlueChannel;

            v2f vert (appdata v)
            {
                v2f o;

                // 0.08 - For tabletop flames
                // 3.08 - For campfire flames
                // Control the amplitude of the displacement
                float displacementAmplitude = 0.15f; 

                // Compute a displacement based on the sampled noise
                float displacement = _SampledNoise * displacementAmplitude;

                // -0.8 - For tabletop flames
                // -0.3 - For campfire flames
                // Modulate the displacement with y-coordinate
                // This causes the displacement to be 0 at the base of the flame (y = 0) and gradually increase towards the tip of the flame
                float displacementFactor = smoothstep(0.0, 1.0, _HexagonYPosition / _FlameHeight);
                float displacementFactorMirrored = 1.0 - smoothstep(0.0, 1.0, _HexagonYPosition / _FlameHeight);

                // Apply the displacement factor to the displacement
                displacement *= displacementFactor;
                displacement *= displacementFactorMirrored;

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

                fixed4 innerColor = fixed4(_RedChannel, 
                    _GreenChannel, _BlueChannel, 1.0);
                fixed4 outerColor = fixed4(_OuterRedChannel, _OuterGreenChannel,
                 _OuterBlueChannel, 1.0);

                fixed4 color;

                // Changed from 0.0
                float t = smoothstep(-0.2, 1.0, dist);
                color = lerp(innerColor, outerColor, t);

                if(dist > 0.80) {
                    discard;
                }

                dist += 0.1;
                color.a = lerp(1.0, -0.08, dist);
                color.a *= 0.5;
                color.a *= _FlameOpacity;

                return color;
            }
            ENDCG
        }
    }
}
