using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FlameMovementController : MonoBehaviour
{
    private RaycastHit hitMove;

    private bool flameOnTable;

    private Vector3 movementDirection;
    public float speed;

    public GameObject flamePrefab;

    private Vector3 lastTrailPosition;
    public float trailSpawnPositionDifference = 0.005f;

    public float trailFlameGrowthChance = 0.7f;
    public float trailFlameGrowthThreshold = 1.0f;

    public float innerRedChannel, innerGreenChannel, innerBlueChannel;
    public float outerRedChannel, outerGreenChannel, outerBlueChannel;

    private List<FlameController> flameControllers;
    int numberOfFlames;
    private MeshRenderer[] meshRenderers;

    private FlameController flameController;

    // Start is called before the first frame update
    void Start()
    {
        flameController = gameObject.GetComponent<FlameController>();
        meshRenderers = new MeshRenderer[20000];

        // SET FRAMERATE
        Application.targetFrameRate = 500;

        flameControllers = new List<FlameController>();

        speed = 0.01f;

        innerRedChannel = 255.0f;
        innerGreenChannel = 127.5f;
        innerBlueChannel = 0.0f;

        outerRedChannel = 255.0f;
        outerGreenChannel = 0.0f;
        outerBlueChannel = 0.0f;

        GenerateRandomMovementDirection();

        // Set initial flame's height
        StartCoroutine(DelayedSetupInitialFlame());
        PlaceFlameOnTable(gameObject);

        flameControllers.Add(flameController);

        numberOfFlames = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Comment next 3 lines for stationary flame
        // transform.Translate(movementDirection * speed * Time.deltaTime, Space.World);
        // KeepFlameOnSurface();
        
        // if(flames.Count < 2)
        LeaveTrail();
    }

    public static void PlaceFlameOnTable(GameObject flameStack)
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
                    flameStack.transform.position = hitPlace.point + new Vector3(0, 0.005f + ComputeYPosition(flameStack.transform.localScale.y), 0);
                }
            }
        }
    }

    public static float ComputeYPosition(float scale)
    {
        float scale1 = 0.15f, yPos1 = 0.0f;
        float scale2 = 0.98f, yPos2 = 0.025f;

        float m = (yPos2 - yPos1) / (scale2 - scale1);
        float b = yPos1 - m * scale1;

        float yPos = m * scale + b;

        return yPos;
    }

    private void KeepFlameOnSurface()
    {
        // Cast ray straight down (while looking ahead, in order to change course before going off the table).
        if (Physics.Raycast(transform.position + movementDirection * (speed + 0.4f) * Time.deltaTime, -Vector3.up, out hitMove))
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

            newFlame.SetActive(false);

            // Delay setup for a frame so that instantiation has time to finish properly.
            StartCoroutine(DelayedSetupTrailingFlame(newFlame));

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
        FlameHeightController flameHeightController = trailFlame.AddComponent<FlameHeightController>();
        flameHeightController.baseGrowthSpeed = 0.05f;
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

    private IEnumerator DelayedSetupTrailingFlame(GameObject newFlame)
    {
        yield return null;  // Wait for the next frame

        FlameController flameController = newFlame.GetComponent<FlameController>();

        flameControllers.Add(flameController);
        flameController.SetupFlame();

        SetupFlame(newFlame);
    }

    private IEnumerator DelayedSetupInitialFlame()
    {
        yield return null;
        flameController.SetupFlame();
    }

    private void SetupFlame(GameObject rootHexagonStack)
    {
        if (rootHexagonStack.name.Contains("Clone"))
        {
            SimplexNoise simplexNoise = rootHexagonStack.AddComponent<SimplexNoise>();
            simplexNoise.seed = flameControllers.Count * 1000;
        }

        PlaceFlameOnTable(rootHexagonStack);
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

    public void OnColorChanged()
    {
        foreach(FlameController flameController in flameControllers)
        {
            flameController.SetHexagonsInnerColor(innerRedChannel, innerGreenChannel, innerBlueChannel);
            flameController.SetHexagonsOuterColor(outerRedChannel, outerGreenChannel, outerBlueChannel);
        }
    }
}
