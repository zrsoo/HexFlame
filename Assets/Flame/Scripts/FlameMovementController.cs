using System.Collections;
using UnityEngine;

public class FlameMovementController : MonoBehaviour
{
    private RaycastHit hitMove;

    private bool flameOnTable;

    private Vector3 movementDirection;
    public float speed;

    public GameObject flamePrefab;

    private Vector3 lastTrailPosition;
    public float trailSpawnPositionDifference = 0.005f;

    public float trailFlameGrowthChance = 0.5f;
    public float trailFlameGrowthThreshold = 1.0f;

    public Texture2D[] noiseTextures;

    // Start is called before the first frame update
    void Start()
    {
        speed = 0.01f;

        PlaceFlameOnTable(gameObject);
        GenerateRandomMovementDirection();

        noiseTextures = Resources.LoadAll<Texture2D>("NoiseTextures");

        // Set initial flame's height
        StartCoroutine(DelayedSetup(gameObject));
        StartCoroutine(GrowTrailFlame(gameObject));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(movementDirection * speed * Time.deltaTime, Space.World);
        KeepFlameOnSurface();
        LeaveTrail();
    }

    private void PlaceFlameOnTable(GameObject flameStack)
    {
        RaycastHit hitPlace;
        // Cast a ray straight down.
        if (Physics.Raycast(flameStack.transform.position, -Vector3.up, out hitPlace))
        {
            // If it hits the table.
            if (hitPlace.collider.gameObject.CompareTag("Table"))
            {
                if (flameStack.name.Contains("TRAIL") && !flameStack.name.Contains("BIG"))
                {
                    // Position flame on table but not slightly above since flame is already small.
                    flameStack.transform.position = hitPlace.point + new Vector3(0, 0.005f, 0);
                }
                else
                {
                    // Position flame on table (slightly above).
                    flameStack.transform.position = hitPlace.point + new Vector3(0, GetDistanceFromTable(flameStack.transform.localScale.y), 0);
                }
            }
        }
    }

    private float GetDistanceFromTable(float scale)
    {
        return scale * 0.04f;
    }

    private void KeepFlameOnSurface()
    {
        // Cast ray straight down (while looking ahead, in order to change course before going off the table).
        if (Physics.Raycast(transform.position + movementDirection * speed * Time.deltaTime, -Vector3.up, out hitMove))
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

    private void GenerateRandomMovementDirection()
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

    private void LeaveTrail()
    {
        float distanceTraveled = Vector3.Distance(transform.position, lastTrailPosition);

        if (distanceTraveled > trailSpawnPositionDifference)
        {
            bool isBig = Random.value <= trailFlameGrowthChance;
            GameObject newFlame = Instantiate(flamePrefab, transform.position - new Vector3(0.0f, 0.0f, 0.01f), transform.rotation);

            StackHexagons stackHexagons = newFlame.GetComponent<StackHexagons>();
            stackHexagons.distanceBetweenCenters *= 0.15f;
            newFlame.transform.localScale *= 0.15f;

            newFlame.SetActive(false);

            // Delay setup for a frame so that instantiation has time to finish properly.
            StartCoroutine(DelayedSetup(newFlame));

            newFlame.SetActive(true);
            newFlame.name += "TRAIL";

            StartCoroutine(RiseFromTable(newFlame, 0.5f, isBig));

            lastTrailPosition = transform.position;
        }
    }

    private IEnumerator GrowTrailFlame(GameObject trailFlame)
    {
        float growthSpeed = 0.0001f;


        while (trailFlame.transform.localScale.y <= trailFlameGrowthThreshold)
        {
            float prevHeight = trailFlame.transform.localScale.y;

            if (trailFlame.transform.localScale.x < 1)
                trailFlame.transform.localScale += new Vector3(growthSpeed, growthSpeed, 0);
            else
                trailFlame.transform.localScale += new Vector3(0, growthSpeed, 0);

            PlaceFlameOnTable(trailFlame);

            yield return null;
        }

        // Once the flame reaches the growth threshold, add the FlameHeightController component so it starts varying in height.
        trailFlame.AddComponent<FlameHeightController>();
        SetBaseGrowthSpeed(trailFlame, 0.05f);
    }

    private IEnumerator RiseFromTable(GameObject flame, float duration, bool isBig)
    {
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

            yield return null;
        }

        // Start growth process with certain probability.
        if (isBig)
        {
            flame.name += "BIG";
            StartCoroutine(GrowTrailFlame(flame));
        }
    }

    private IEnumerator DelayedSetup(GameObject newFlame)
    {
        yield return null;  // Wait for the next frame
        SetupFlame(newFlame);
    }

    private void AddRandomPerlinNoiseTexture(GameObject flame)
    {
        PerlinNoiseSampler perlinNoiseSampler = flame.AddComponent<PerlinNoiseSampler>();
        Texture2D noiseTexture = noiseTextures[Random.Range(0, noiseTextures.Length)];
        perlinNoiseSampler.perlinNoiseTexture = noiseTexture;
    }

    private void SetupFlame(GameObject rootHexagonStack)
    {
        SetHexagonsRandomDelay(rootHexagonStack);
        AddRandomPerlinNoiseTexture(rootHexagonStack);

        if(!rootHexagonStack.name.Contains("Clone"))
            SetBaseGrowthSpeed(rootHexagonStack, 0.05f);

        PlaceFlameOnTable(rootHexagonStack);
        SetHexagonsHeight(rootHexagonStack);
    }

    private void SetHexagonsOpacity(GameObject rootHexagonStack, float opacity)
    {
        // Loop over the children of the root GameObject and set their opacity.
        for (int i = 0; i < rootHexagonStack.transform.childCount; i++)
        {
            Transform hexagon = rootHexagonStack.transform.GetChild(i);
            MeshRenderer meshRenderer = hexagon.GetComponent<MeshRenderer>();
            meshRenderer.material.SetFloat("_FlameOpacity", opacity);
        }
    }

    private void SetHexagonsHeight(GameObject rootHexagonStack)
    {
        float flameHeight = GetFlameHeight(rootHexagonStack);

        for (int i = 0; i < rootHexagonStack.transform.childCount; i++)
        {
            Transform hexagonTransform = rootHexagonStack.transform.GetChild(i);
            MeshRenderer hexagonRenderer = hexagonTransform.GetComponent<MeshRenderer>();

            hexagonRenderer.material.SetFloat("_FlameHeight", flameHeight);
        }
    }

    private void SetHexagonsRandomDelay(GameObject rootHexagonStack)
    {
        float randomDelay = Random.Range(0.0f, 100.0f);

        for (int i = 0; i < rootHexagonStack.transform.childCount; i++)
        {
            Transform hexagonTransform = rootHexagonStack.transform.GetChild(i);
            MeshRenderer hexagonRenderer = hexagonTransform.GetComponent<MeshRenderer>();

            hexagonRenderer.material.SetFloat("_RandomPhaseOffset", randomDelay);
        }
    }

    private static void SetBaseGrowthSpeed(GameObject rootHexagonStack, float speed)
    {
        if (rootHexagonStack.name.Contains("Clone"))
        {
            FlameHeightController flameHeightController = rootHexagonStack.GetComponent<FlameHeightController>();
            flameHeightController.baseGrowthSpeed = speed;
        }
    }

    private float GetFlameHeight(GameObject rootHexagonStack)
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
