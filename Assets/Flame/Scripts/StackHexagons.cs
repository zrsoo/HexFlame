using UnityEngine;

public class StackHexagons : MonoBehaviour
{
    public GameObject hexagonPrefab;
    public int count = 11;
    public float scale = 0.4f;
    public float distanceBetweenCenters = 0.01f;

    public float sizeMax = 0.08f;  // max size of hexagon
    public float sizeMin = 0.03f;  // min size of hexagon

    private void Start()
    {
        //distanceBetweenCenters *= scale;

        //for (int i = 0; i < count; i++)
        //{
        //    GameObject hexagon = Instantiate(hexagonPrefab, 
        //        new Vector3(transform.position.x, transform.position.y + i * distanceBetweenCenters, transform.position.z), Quaternion.identity, this.transform);
        //    DrawHexagon hexagonScript = hexagon.GetComponent<DrawHexagon>();

        //    // Custom hexagon sizes.
        //    switch (i)
        //    {
        //        case 0:
        //            hexagonScript.width = 0.114f * scale;
        //            // hexagonScript.height = 0.1f * scale;
        //            hexagonScript.height = 0.114f * scale;
        //            break;
        //        case 1:
        //            hexagonScript.width = 0.14f * scale;
        //            //hexagonScript.height = 0.1f * scale;
        //            hexagonScript.height = 0.14f * scale;
        //            break;
        //        case 2:
        //            hexagonScript.width = 0.16f * scale;
        //            //hexagonScript.height = 0.1f * scale;
        //            hexagonScript.height = 0.16f * scale;
        //            break;
        //        case 3:
        //            hexagonScript.width = 0.096f * scale;
        //            //hexagonScript.height = 0.1f * scale;
        //            hexagonScript.height = 0.096f * scale;
        //            break;
        //        case 4:
        //            hexagonScript.width = 0.086f * scale;
        //            //hexagonScript.height = 0.1f * scale;
        //            hexagonScript.height = 0.086f * scale;
        //            break;
        //        case 5:
        //            hexagonScript.width = 0.076f * scale;
        //            //hexagonScript.height = 0.1f * scale;
        //            hexagonScript.height = 0.076f * scale;
        //            break;
        //        case 6:
        //            hexagonScript.width = 0.076f * scale;
        //            //hexagonScript.height = 0.1f * scale;
        //            hexagonScript.height = 0.076f * scale;
        //            break;
        //        case 7:
        //            hexagonScript.width = 0.066f * scale;
        //            //hexagonScript.height = 0.1f * scale;
        //            hexagonScript.height = 0.066f * scale;
        //            break;
        //        case 8:
        //            hexagonScript.width = 0.056f * scale;
        //            //hexagonScript.height = 0.1f * scale;
        //            hexagonScript.height = 0.056f * scale;
        //            break;
        //        case 9:
        //            hexagonScript.width = 0.046f * scale;
        //            //hexagonScript.height = 0.1f * scale;
        //            hexagonScript.height = 0.046f * scale;
        //            break;
        //        case 10:
        //            hexagonScript.width = 0.036f * scale;
        //            //hexagonScript.height = 0.1f * scale;
        //            hexagonScript.height = 0.036f * scale;
        //            break;
        //        case 11:
        //            hexagonScript.width = 0.026f * scale;
        //            //hexagonScript.height = 0.1f * scale;
        //            hexagonScript.height = 0.026f * scale;
        //            break;
        //    }


        //    hexagonScript.Draw();
        ////////////////////////////////////////////////////
        //distanceBetweenCenters *= scale;

        //// Parameters for Gaussian function
        //float A = 1;
        //float B = count / 4.5f;  // set the position of the peak ( / 2.0f = at the middle)
        //float C = count / 3.5f;  // adjust this parameter to control the width of the bell

        //for (int i = 0; i < count; i++)
        //{
        //    GameObject hexagon = Instantiate(hexagonPrefab,
        //        new Vector3(transform.position.x, transform.position.y + i * distanceBetweenCenters, transform.position.z), Quaternion.identity, this.transform);
        //    DrawHexagon hexagonScript = hexagon.GetComponent<DrawHexagon>();

        //    // Calculate size using Gaussian function
        //    float size = A * Mathf.Exp(-(Mathf.Pow((i - B), 2)) / (2 * Mathf.Pow(C, 2)));

        //    // Rescale size to be between sizeMin and sizeMax
        //    size = sizeMin + (size * (sizeMax - sizeMin));

        //    // Assign calculated size to the hexagon
        //    hexagonScript.width = size * scale;
        //    hexagonScript.height = size * scale;

        //    hexagonScript.Draw();
        //}
        ///////////////////////////////////////////////////////


        distanceBetweenCenters *= scale;

        float maxScale = 0.2f;
        float minScale = 0.03f;

        for (int i = 0; i < count; i++)
        {
            GameObject hexagon = Instantiate(hexagonPrefab,
                new Vector3(transform.position.x, transform.position.y + i * distanceBetweenCenters, transform.position.z), Quaternion.identity, this.transform);
            DrawHexagon hexagonScript = hexagon.GetComponent<DrawHexagon>();

            // Adjust the distribution of the sine function using a power function
            float adjustedI = Mathf.Pow(i / (float)(count - 1), 0.5f);

            // Use the sin function to create a flame shape
            float factor = Mathf.Sin(adjustedI * Mathf.PI);

            // Mix the min and max scales according to the factor
            float size = Mathf.Lerp(minScale, maxScale, factor);

            // Assign calculated size to the hexagon
            hexagonScript.width = size * scale;
            hexagonScript.height = size * scale;

            hexagonScript.Draw();
        }
    }
}
