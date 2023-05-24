using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoiseSampler : MonoBehaviour
{
    public Texture2D perlinNoiseTexture;
    private float timeOffset;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Assign a random time offset when the script starts
        timeOffset = Random.value * 100; // Multiplied by 100 to increase variability

        // Sample the Perlin Noise texture at a point that moves with time
        // Use modulo to loop the sampling point when it reaches the end of the texture
        float u = (Time.time * 0.05f) % 512;
        float v = ((Time.time / 2) * 0.05f) % 512;

        Color sampledNoise = perlinNoiseTexture.GetPixelBilinear(u, v);

        // Convert the color to a float
        float sampledNoiseFloat = sampledNoise.r;

        // Pass the value to the shader
        SetNoiseOfHexagons((sampledNoiseFloat));
    }

    void SetNoiseOfHexagons(float noise)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Transform hexagon = transform.GetChild(i);
            MeshRenderer meshRenderer = hexagon.GetComponent<MeshRenderer>();

            meshRenderer.material.SetFloat("_SampledNoise", noise);
            meshRenderer.material.SetFloat("_HexagonYPosition", i);
        }
    }
}
