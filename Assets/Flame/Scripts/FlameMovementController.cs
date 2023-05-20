using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

// TODO problem with how flames spawn, disappear, spawn again.

// TODO make flame grow taller and shrink in tallness over time, spawning flames of different sizes.

// TODO make newly spawned flames grow over time.

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

    private float baseGrowthSpeed = 0.1f;
    private bool isGrowing = true;
    private float minFlameHeight = 1.0f; // example value, replace with your minimum
    private float maxFlameHeight;
    private float randomGrowthFactor;
    private float transitionDuration = 5f; // duration over which random values will change

    // Start is called before the first frame update
    void Start()
    {
        speed = 0.01f;

        PlaceFlameOnTable();
        GenerateRandomMovementDirection();

        noiseTextures = Resources.LoadAll<Texture2D>("NoiseTextures");

        lastFlamePosition = transform.position;

        // Set initial flame's height
        SetupFlame(gameObject);

        randomGrowthFactor = Random.Range(0.8f, 1.2f);
        maxFlameHeight = Random.Range(1.5f, 3.0f); // replace with your desired range

        StartCoroutine(ChangeGrowthFactorOverTime());
        StartCoroutine(ChangeMaxHeightOverTime());
    }

    // Update is called once per frame
    void Update()
    {
        // transform.Translate(movementDirection * speed * Time.deltaTime, Space.World);
        KeepFlameOnSurface();
        // ExpandFlame();
        UpdateFlameHeight();
    }

    IEnumerator ChangeGrowthFactorOverTime()
    {
        while (true)
        {
            float initialGrowthFactor = randomGrowthFactor;
            float finalGrowthFactor = Random.Range(0.1f, 0.3f);

            float timePassed = 0;
            while (timePassed < transitionDuration)
            {
                randomGrowthFactor = Mathf.Lerp(initialGrowthFactor, finalGrowthFactor, timePassed / transitionDuration);
                timePassed += Time.deltaTime;
                yield return null;
            }
        }
    }

    IEnumerator ChangeMaxHeightOverTime()
    {
        while (true)
        {
            float initialMaxHeight = maxFlameHeight;
            float finalMaxHeight = Random.Range(1.0f, 2.0f); // replace with your desired range

            float timePassed = 0;
            while (timePassed < transitionDuration)
            {
                maxFlameHeight = Mathf.Lerp(initialMaxHeight, finalMaxHeight, timePassed / transitionDuration);
                timePassed += Time.deltaTime;
                yield return null;
            }
        }
    }

    private void UpdateFlameHeight()
    {
        float prevHeight = transform.localScale.y;

        // Determine the current direction of growth.
        if (transform.localScale.y >= maxFlameHeight)
        {
            isGrowing = false;
        }
        else if (transform.localScale.y <= minFlameHeight)
        {
            isGrowing = true;
        }

        // Adjust growth speed based on direction and randomness.
        float currentGrowthSpeed = baseGrowthSpeed * (isGrowing ? randomGrowthFactor : 1 / randomGrowthFactor);
        float currentHeight = prevHeight + Time.deltaTime * currentGrowthSpeed * (isGrowing ? 1 : -1);

        // Keep the currentHeight within bounds.
        currentHeight = Mathf.Clamp(currentHeight, minFlameHeight, maxFlameHeight);

        transform.localScale = new Vector3(transform.localScale.x, currentHeight, transform.localScale.z);
        transform.position = new Vector3(transform.position.x, transform.position.y + (currentHeight - prevHeight) / 2 / 50, transform.position.z);
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

    private void PlaceFlameOnTable()
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

    private void ExpandFlame()
    {
        // Instantiate new flames as the flame moves (spread).
        float distanceTraveled = Vector3.Distance(transform.position, lastFlamePosition);

        // If the flame has traveled enough, spawn another flame.
        if (distanceTraveled > flameSpawnPositionDifference)
        {
            GameObject newFlame = Instantiate(flamePrefab, transform.position - new Vector3(0.0f, 0.0f, 0.01f), transform.rotation);
            newFlame.SetActive(false);

            // Delay setup for a frame so that instantiation has time to finish properly.
            StartCoroutine(DelayedSetup(newFlame));

            newFlame.SetActive(true);

            StartCoroutine(RiseFromTable(newFlame, 0.5f));

            lastFlamePosition = transform.position;
        }
    }

    // TODO Maybe implement y axis rising as well, if necessary
    private IEnumerator RiseFromTable(GameObject flame, float duration)
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
        Debug.Log("HEIGHT:" + flameHeight);
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

    private void SetupFlame(GameObject rootHexagonStack)
    {
        SetHexagonsRandomDelay(rootHexagonStack);
        AddRandomPerlinNoiseTexture(rootHexagonStack);
        SetHexagonsHeight(rootHexagonStack);
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
