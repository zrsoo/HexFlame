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

    private float GetFlameHeight()
    {
        float totalHeight = 0;

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Transform hexagonTransform = gameObject.transform.GetChild(i);

            // Assuming the hexagon's height is represented by the y-scale of its local transform
            totalHeight += hexagonTransform.localScale.y;
        }

        return totalHeight;
    }
}
