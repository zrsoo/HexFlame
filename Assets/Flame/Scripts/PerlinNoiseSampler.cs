using UnityEngine;

public class PerlinNoiseSampler : MonoBehaviour
{
    public Texture2D perlinNoiseTexture;
    private int perlinNoiseTextureWidth = 512;
    private int perlinNoiseTextureHeight = 512;
    private float timeMultiplier = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Sample the Perlin Noise texture at a point that moves with time
        // Use modulo to loop the sampling point when it reaches the end of the texture
        float u = (Time.time * timeMultiplier) % perlinNoiseTextureWidth;
        float v = (Time.time * timeMultiplier) % perlinNoiseTextureHeight;

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
