using UnityEngine;

public class StackHexagons : MonoBehaviour
{
    public GameObject hexagonPrefab;
    public int count = 11;
    public float scale = 0.4f;
    public float distanceBetweenCenters = 0.01f;

    public float sizeMax = 0.2f;  // max size of hexagon
    public float sizeMin = 0.02f;  // min size of hexagon

    private MeshRenderer[] meshRenderers;

    private FlameController flameController;

    private void Start()
    {
        Transform transform = this.transform;

        meshRenderers = new MeshRenderer[count];
        distanceBetweenCenters *= scale;

        float hexagonStackHeight = 0;

        for (int i = 0; i < count; i++)
        {
            GameObject hexagon = Instantiate(hexagonPrefab,
                new Vector3(transform.position.x, transform.position.y + i * distanceBetweenCenters, transform.position.z), Quaternion.identity, transform);
            DrawHexagon hexagonScript = hexagon.GetComponent<DrawHexagon>();

            // Adjust the distribution of the sine function using a power function
            float adjustedI = Mathf.Pow(i / (float)(count - 1), 0.5f);

            // Use the sin function to create a flame shape
            float factor = Mathf.Sin(adjustedI * Mathf.PI);

            // Mix the min and max scales according to the factor
            float size = Mathf.Lerp(sizeMin, sizeMax, factor);

            // Assign calculated size to the hexagon
            hexagonScript.width = size * scale;
            hexagonScript.height = size * scale;

            hexagonScript.Draw();

            meshRenderers[i] = hexagon.GetComponent<MeshRenderer>();
            hexagonStackHeight += hexagon.transform.localScale.y;
        }

        flameController = gameObject.GetComponent<FlameController>();

        flameController.SetHexagonMeshRenderers(meshRenderers);
    }
}
