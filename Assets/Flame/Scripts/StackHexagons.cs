using UnityEngine;

public class StackHexagons : MonoBehaviour
{
    public GameObject hexagonPrefab;
    public int count = 5;
    public float scale = 0.4f;
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
                    hexagonScript.width = 0.04f * scale;
                    hexagonScript.height = 0.1f * scale;
                    break;
                case 1:
                    hexagonScript.width = 0.03f * scale;
                    hexagonScript.height = 0.1f * scale;
                    break;
                case 2:
                    hexagonScript.width = 0.02f * scale;
                    hexagonScript.height = 0.1f * scale;
                    break;
                case 3:
                    hexagonScript.width = 0.015f * scale;
                    hexagonScript.height = 0.1f * scale;
                    break;
                case 4:
                    hexagonScript.width = 0.01f * scale;
                    hexagonScript.height = 0.1f * scale;
                    break;
            }


            hexagonScript.Draw();
        }
    }
}
