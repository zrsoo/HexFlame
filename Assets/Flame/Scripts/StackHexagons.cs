using UnityEngine;

public class StackHexagons : MonoBehaviour
{
    public GameObject hexagonPrefab;
    public int count = 9;
    public float scale = 0.2f;
    public float distanceBetweenCenters = 0.01f;

    private void Start()
    {
        distanceBetweenCenters *= scale;

        for (int i = 0; i < count; i++)
        {
            GameObject hexagon = Instantiate(hexagonPrefab, 
                new Vector3(transform.position.x, transform.position.y + i * distanceBetweenCenters, transform.position.z), Quaternion.identity, this.transform);
            DrawHexagon hexagonScript = hexagon.GetComponent<DrawHexagon>();

            // Custom hexagon sizes.
            switch (i)
            {
                case 0:
                    hexagonScript.width = 0.1f * scale;
                    hexagonScript.height = 0.1f * scale;
                    break;
                case 1:
                    hexagonScript.width = 0.088f * scale;
                    hexagonScript.height = 0.1f * scale;
                    break;
                case 2:
                    hexagonScript.width = 0.076f * scale;
                    hexagonScript.height = 0.1f * scale;
                    break;
                case 3:
                    hexagonScript.width = 0.064f * scale;
                    hexagonScript.height = 0.1f * scale;
                    break;
                case 4:
                    hexagonScript.width = 0.052f * scale;
                    hexagonScript.height = 0.1f * scale;
                    break;
                case 5:
                    hexagonScript.width = 0.040f * scale;
                    hexagonScript.height = 0.1f * scale;
                    break;
                case 6:
                    hexagonScript.width = 0.028f * scale;
                    hexagonScript.height = 0.1f * scale;
                    break;
                case 7:
                    hexagonScript.width = 0.016f * scale;
                    hexagonScript.height = 0.1f * scale;
                    break;
                case 8:
                    hexagonScript.width = 0.01f * scale;
                    hexagonScript.height = 0.1f * scale;
                    break;
            }


            hexagonScript.Draw();
        }
    }
}
