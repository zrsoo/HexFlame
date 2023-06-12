using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameController : MonoBehaviour
{
    private MeshRenderer[] meshRenderers;
    int numberOfHexagons;

    private float innerRed, innerGreen, innerBlue;
    private float outerRed, outerGreen, outerBlue;

    // Start is called before the first frame update
    void Start()
    {
        GlobalFlameManager.instance.RegisterFlameController(this);

        //innerRed = 255.0f;
        //innerGreen = 127.5f;
        //innerBlue = 0.0f;

        //outerRed = 255.0f;
        //outerGreen = 0.0f;
        //outerBlue = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHexagonMeshRenderers(MeshRenderer[] meshRenderers)
    {
        numberOfHexagons = gameObject.GetComponent<StackHexagons>().count;
        this.meshRenderers = new MeshRenderer[numberOfHexagons];

        for (int i = 0; i < numberOfHexagons; ++i)
        {
            this.meshRenderers[i] = meshRenderers[i];
        }
    }

    public void SetupFlame()
    {
        SetHexagonsHeight();
        SetRandomAmplitudeFactor();
        // SetHexagonsInnerColor(innerRed, innerGreen, innerBlue);
        // SetHexagonsOuterColor(outerRed, outerGreen, outerBlue);
    }

    private void SetHexagonsHeight()
    {
        for (int i = 0; i < numberOfHexagons; i++)
        {
            meshRenderers[i].material.SetFloat("_HexagonYPosition", i);
            meshRenderers[i].material.SetFloat("_FlameHeight", numberOfHexagons);
        }
    }

    public void SetHexagonsInnerColor(float red, float green, float blue)
    {
        innerRed = red;
        innerGreen = green;
        innerBlue = blue;

        red /= 255.0f;
        green /= 255.0f;
        blue /= 255.0f;

        for (int i = 0; i < numberOfHexagons; i++)
        {
            meshRenderers[i].material.SetFloat("_RedChannel", red);
            meshRenderers[i].material.SetFloat("_GreenChannel", green);
            meshRenderers[i].material.SetFloat("_BlueChannel", blue);
        }
    }

    public void SetHexagonsOuterColor(float red, float green, float blue)
    {
        outerRed = red;
        outerGreen = green;
        outerBlue = blue;

        red /= 255.0f;
        green /= 255.0f;
        blue /= 255.0f;

        for (int i = 0; i < numberOfHexagons; i++)
        {
            meshRenderers[i].material.SetFloat("_OuterRedChannel", red);
            meshRenderers[i].material.SetFloat("_OuterGreenChannel", green);
            meshRenderers[i].material.SetFloat("_OuterBlueChannel", blue);
        }
    }

    public void SetHexagonsOpacity(float opacity)
    {
        // Loop over the children of the root GameObject and set their opacity.
        for (int i = 0; i < numberOfHexagons; i++)
        {
            meshRenderers[i].material.SetFloat("_FlameOpacity", opacity);
        }
    }

    private void SetRandomAmplitudeFactor()
    {
        float factor = Random.Range(0.7f, 1.3f);

        for (int i = 0; i < numberOfHexagons; i++)
        {
            meshRenderers[i].material.SetFloat("_RandomAmplitudeFactor", factor);
        }
    }

    public void GetColors(float innerRed, float innerGreen, float innerBlue,
        float outerRed, float outerGreen, float outerBlue)
    {
        this.innerRed = innerRed;
        this.innerGreen = innerGreen;
        this.innerBlue = innerBlue;

        this.outerRed = outerRed;
        this.outerGreen = outerGreen;
        this.outerBlue = outerBlue;
    }

    public void setNoiseSeed(float seed)
    {
        SimplexNoise simplexNoise = gameObject.AddComponent<SimplexNoise>();
        simplexNoise.seed = seed;
    }
}
