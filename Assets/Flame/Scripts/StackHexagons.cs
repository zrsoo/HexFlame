using UnityEngine;
using UnityEngine.UIElements;

public class StackHexagons : MonoBehaviour
{
    public GameObject hexagonPrefab;
    public int count = 4;
    public float scale = 0.3f;
    public float distanceBetweenCenters = 0.05f;

    private void Start()
    {
        distanceBetweenCenters *= scale;

        for (int i = 0; i < count; i++)
        {
            GameObject hexagon = Instantiate(hexagonPrefab, 
                new Vector3(transform.position.x, transform.position.y + i * distanceBetweenCenters, transform.position.z), Quaternion.identity, this.transform);
            DrawHexagon hexagonScript = hexagon.GetComponent<DrawHexagon>();

            // Custom first hexagon size
            switch (i)
            {
                case 0:
                    hexagonScript.width = 0.05f * scale;
                    hexagonScript.height = 0.13f * scale;
                    break;
                case 1:
                    hexagonScript.width = 0.035f * scale;
                    hexagonScript.height = 0.12f * scale;
                    break;
                case 2:
                    hexagonScript.width = 0.025f * scale;
                    hexagonScript.height = 0.1f * scale;
                    break;
                case 3:
                    hexagonScript.width = 0.013f * scale;
                    hexagonScript.height = 0.1f * scale;
                    break;
            }


            hexagonScript.Draw();
        }
    }
}
