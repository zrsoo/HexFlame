using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

// TODO Adjust flame flicker speed in ordinance with flame movement speed

public class FlameMovementController : MonoBehaviour
{
    private RaycastHit hitPlace;
    private RaycastHit hitMove;

    private bool flameOnTable;

    private Vector3 movementDirection;
    public float speed;

    public GameObject flamePrefab;

    private Vector3 lastFlamePosition;
    private float flameSpawnPositionDifference = 0.05f;

    public Texture2D[] noiseTextures;

    // Start is called before the first frame update
    void Start()
    {
        speed = 0.01f;

        PlaceFlameOnTable();
        GenerateRandomMovementDirection();

        noiseTextures = Resources.LoadAll<Texture2D>("NoiseTextures");

        lastFlamePosition = transform.position;

        // Set initial flame's height
        SetHexagonsHeight(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(movementDirection * speed * Time.deltaTime, Space.World);
        KeepFlameOnSurface();
        ExpandFlame();
    }

    private void KeepFlameOnSurface()
    {
        Debug.DrawRay(transform.position + movementDirection * (speed + 0.3f) * Time.deltaTime, -Vector3.up * 10, Color.red);

        // Cast ray straight down (while looking ahead, in order to change course before going off the table).
        if (Physics.Raycast(transform.position + movementDirection * (speed + 0.1f) * Time.deltaTime, -Vector3.up, out hitMove))
        {
            // If it hits the table.
            if (hitMove.collider.gameObject.tag == "Table")
            {
                flameOnTable = true;
            }
            else
            {
                flameOnTable = false;
            }
        }

        // If the flame is not on the table, pick another random direction.
        if (!flameOnTable)
        {
            GenerateRandomMovementDirection();
            flameOnTable = true;
        }
    }

    void PlaceFlameOnTable()
    {
        // Cast a ray straight down.
        if (Physics.Raycast(transform.position, -Vector3.up, out hitPlace))
        {
            // If it hits the table.
            if (hitPlace.collider.gameObject.tag == "Table")
            {
                // Position flame on table (slightly above).
                transform.position = hitPlace.point + new Vector3(0, 0.05f, 0);
            }
        }
    }

    void GenerateRandomMovementDirection()
    {
        GameObject table = GameObject.FindGameObjectWithTag("Table");
        Vector3 tableCenterPosition = new Vector3(table.transform.position.x, transform.position.y, table.transform.position.z);
        Vector3 directionToTableCenter = (tableCenterPosition - transform.position).normalized;

        // Create a random offset.
        float angleOffset = Random.Range(-45.0f, 45.0f); // Change this range depending on how much randomness you want
        Quaternion rotation = Quaternion.Euler(0, angleOffset, 0);

        // Apply the random offset to the direction.
        movementDirection = rotation * directionToTableCenter;
    }

    // TODO problem with flame scaling
    // TODO problem with how flames spawn, disappear, spawn again

    private void ExpandFlame()
    {
        // Instantiate new flames as the flame moves (spread).
        float distanceTraveled = Vector3.Distance(transform.position, lastFlamePosition);

        // If the flame has traveled enough, spawn another flame.
        if (distanceTraveled > flameSpawnPositionDifference)
        {
            GameObject newFlame = Instantiate(flamePrefab, transform.position - new Vector3(0.0f, 0.0f, 0.01f), transform.rotation);
            newFlame.SetActive(false);
            StartCoroutine(RiseFromTable(newFlame, 0.5f));

            lastFlamePosition = transform.position;
        }
    }

    // TODO Maybe implement y axis rising as well, if necessary
    IEnumerator RiseFromTable(GameObject flame, float duration)
    {
        SetHexagonsHeight(flame);
        AddRandomPerlinNoiseTexture(flame);

        float elapsed = 0.0f;

        float initialOpacity = 0.0f;
        float finalOpacity = 1.0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float tOpacity;
            float percentage = elapsed / duration;

            if (percentage > 0.5f)
            {
                // Account for jumping over 50%.
                tOpacity = (percentage - 0.5f) / 0.5f;

                tOpacity = tOpacity * tOpacity * (3.0f - 2.0f * tOpacity);

                float opacity = Mathf.Lerp(initialOpacity, finalOpacity, tOpacity);

                SetHexagonsOpacity(flame, opacity);
            }
            else
            {
                SetHexagonsOpacity(flame, 0.0f);
            }

            flame.SetActive(true);

            yield return null;
        }
    }

    private void AddRandomPerlinNoiseTexture(GameObject flame)
    {
        PerlinNoiseSampler perlinNoiseSampler = flame.AddComponent<PerlinNoiseSampler>();
        Texture2D noiseTexture = noiseTextures[Random.Range(0, noiseTextures.Length)];
        perlinNoiseSampler.perlinNoiseTexture = noiseTexture;
    }

    void SetHexagonsOpacity(GameObject rootHexagonStack, float opacity)
    {
        // Loop over the children of the root GameObject and set their opacity.
        for (int i = 0; i < rootHexagonStack.transform.childCount; i++)
        {
            Transform hexagon = rootHexagonStack.transform.GetChild(i);
            MeshRenderer meshRenderer = hexagon.GetComponent<MeshRenderer>();
            meshRenderer.material.SetFloat("_FlameOpacity", opacity);
        }
    }

    void SetHexagonsScale(GameObject rootHexagonStack, Vector3 scale)
    {
        for (int i = 0; i < rootHexagonStack.transform.childCount; i++)
        {
            Transform hexagon = rootHexagonStack.transform.GetChild(i);
            hexagon.transform.localScale = scale;
        }
    }

    void SetHexagonsHeight(GameObject rootHexagonStack)
    {
        float flameHeight = GetFlameHeight(rootHexagonStack);

        for (int i = 0; i < rootHexagonStack.transform.childCount; i++)
        {
            Transform hexagonTransform = rootHexagonStack.transform.GetChild(i);
            MeshRenderer hexagonRenderer = hexagonTransform.GetComponent<MeshRenderer>();

            hexagonRenderer.material.SetFloat("_FlameHeight", flameHeight);
        }
    }

    float GetFlameHeight(GameObject rootHexagonStack)
    {
        float totalHeight = 0;

        for (int i = 0; i < rootHexagonStack.transform.childCount; i++)
        {
            Transform hexagonTransform = rootHexagonStack.transform.GetChild(i);

            // Assuming the hexagon's height is represented by the y-scale of its local transform
            totalHeight += hexagonTransform.localScale.y;
        }

        return totalHeight;
    }

    
}
