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

    public void GetHexagonMeshRenderers(MeshRenderer[] meshRenderers2)
    {
        numberOfHexagons = gameObject.GetComponent<StackHexagons>().count;
        meshRenderers = new MeshRenderer[numberOfHexagons];

        // Debug.Log("FLAME");
        // Debug.Log("NUMBERHEX: " + numberOfHexagons);
        for (int i = 0; i < numberOfHexagons; ++i)
        {
            this.meshRenderers[i] = meshRenderers2[i];
            // Debug.Log("HexagonMeshRenderer: " + this.meshRenderers[i].name);
        }
    }

    public void SetupFlame()
    {
        SetHexagonsHeight();
        // SetHexagonsInnerColor(rootHexagonStack, innerRedChannel, innerGreenChannel, innerBlueChannel);
        // SetHexagonsOuterColor(rootHexagonStack, outerRedChannel, outerGreenChannel, outerBlueChannel);
    }

    private void SetHexagonsHeight()
    {
        Debug.Log(numberOfHexagons);

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
}