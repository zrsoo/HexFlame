using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameController : MonoBehaviour
{
    private MeshRenderer[] meshRenderers;
    int numberOfHexagons;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetHexagonMeshRenderers(MeshRenderer[] meshRenderers)
    {
        numberOfHexagons = gameObject.GetComponent<StackHexagons>().count;
        this.meshRenderers = new MeshRenderer[numberOfHexagons];

        // Debug.Log("FLAME");
        // Debug.Log("NUMBERHEX: " + numberOfHexagons);
        for (int i = 0; i < numberOfHexagons; ++i)
        {
            this.meshRenderers[i] = meshRenderers[i];
            // Debug.Log("HexagonMeshRenderer: " + this.meshRenderers[i].name);
        }
    }

    public void SetupFlame()
    {
        SetHexagonsHeight();
    }

    private void SetHexagonsHeight()
    {
        // Debug.Log(numberOfHexagons);

        for (int i = 0; i < numberOfHexagons; i++)
        {
            meshRenderers[i].material.SetFloat("_HexagonYPosition", i);
            meshRenderers[i].material.SetFloat("_FlameHeight", numberOfHexagons);
        }
    }

    public void SetHexagonsInnerColor(float red, float green, float blue)
    {
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

    public MeshRenderer[] getMeshRenderers()
    {
        return meshRenderers;
    }

    private void SetRandomAmplitudeFactor()
    {
        float factor = Random.Range(0.7f, 1.3f);

        for (int i = 0; i < numberOfHexagons; i++)
        {
            meshRenderers[i].material.SetFloat("_RandomAmplitudeFactor", factor);
        }
    }
}
