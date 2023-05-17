using UnityEngine;

public class StackHexagons : MonoBehaviour
{
    public GameObject hexagonPrefab;
    public int count = 4;
    public float distanceBetweenCenters = 0.05f;

    private void Start()
    {
        for (int i = 0; i < count; i++)
        {
            GameObject hexagon = Instantiate(hexagonPrefab, 
                new Vector3(transform.position.x, transform.position.y + i * distanceBetweenCenters, transform.position.z), Quaternion.identity, this.transform);
            DrawHexagon hexagonScript = hexagon.GetComponent<DrawHexagon>();

            // Custom first hexagon size
            switch (i)
            {
                case 0:
                    hexagonScript.width = 0.05f;
                    hexagonScript.height = 0.13f;
                    break;
                case 1:
                    hexagonScript.width = 0.035f;
                    hexagonScript.height = 0.12f;
                    break;
                case 2:
                    hexagonScript.width = 0.025f;
                    hexagonScript.height = 0.1f;
                    break;
                case 3:
                    hexagonScript.width = 0.013f;
                    hexagonScript.height = 0.1f;
                    break;
            }


            hexagonScript.Draw();
        }
    }
}
