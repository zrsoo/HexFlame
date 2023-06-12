using System.Collections;
using UnityEngine;

public class FlameController : MonoBehaviour
{
    private MeshRenderer[] meshRenderers;
    int numberOfHexagons;

    private float trailFlameGrowthChance;
    private float trailFlameGrowthThreshold = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        GlobalFlameManager.instance.RegisterFlameController(this);

        PlaceFlameOnTable();

        if(!gameObject.name.Contains("Clone"))
            StartCoroutine(DelayedSetupInitialFlame());
        else
            StartCoroutine(DelayedSetupTrailingFlame());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHexagonMeshRenderers(MeshRenderer[] meshRenderers)
    {
        numberOfHexagons = gameObject.GetComponent<StackHexagons>().count;
        this.meshRenderers = new MeshRenderer[numberOfHexagons];

        for (int i = 0; i < numberOfHexagons; ++i)
        {
            this.meshRenderers[i] = meshRenderers[i];
        }
    }

    public void SetupFlame()
    {
        SetHexagonsHeight();
        SetRandomAmplitudeFactor();
    }

    private void SetHexagonsHeight()
    {
        for (int i = 0; i < numberOfHexagons; i++)
        {
            meshRenderers[i].material.SetFloat("_HexagonYPosition", i);
            meshRenderers[i].material.SetFloat("_FlameHeight", numberOfHexagons);
        }
    }

    public void SetHexagonsInnerColor(float red, float green, float blue)
    {
        red /= 255.0f;
        green /= 255.0f;
        blue /= 255.0f;

        for (int i = 0; i < numberOfHexagons; i++)
        {
            meshRenderers[i].material.SetFloat("_RedChannel", red);
            meshRenderers[i].material.SetFloat("_GreenChannel", green);
            meshRenderers[i].material.SetFloat("_BlueChannel", blue);
        }
    }

    public void SetHexagonsOuterColor(float red, float green, float blue)
    {
        red /= 255.0f;
        green /= 255.0f;
        blue /= 255.0f;

        for (int i = 0; i < numberOfHexagons; i++)
        {
            meshRenderers[i].material.SetFloat("_OuterRedChannel", red);
            meshRenderers[i].material.SetFloat("_OuterGreenChannel", green);
            meshRenderers[i].material.SetFloat("_OuterBlueChannel", blue);
        }
    }

    public void SetHexagonsOpacity(float opacity)
    {
        // Loop over the children of the root GameObject and set their opacity.
        for (int i = 0; i < numberOfHexagons; i++)
        {
            meshRenderers[i].material.SetFloat("_FlameOpacity", opacity);
        }
    }

    private void SetRandomAmplitudeFactor()
    {
        float factor = Random.Range(0.7f, 1.3f);

        for (int i = 0; i < numberOfHexagons; i++)
        {
            meshRenderers[i].material.SetFloat("_RandomAmplitudeFactor", factor);
        }
    }

    public void SetNoiseSeed(float seed)
    {
        SimplexNoise simplexNoise = gameObject.AddComponent<SimplexNoise>();
        simplexNoise.seed = seed;
    }

    public void SetTrailFlameGrowthChance(float chance)
    {
        trailFlameGrowthChance = chance;
    }

    public void PlaceFlameOnTable()
    {
        RaycastHit hitPlace;
        Transform flameStackTransform = gameObject.transform;

        // Cast a ray straight down.
        if (Physics.Raycast(flameStackTransform.position, -Vector3.up, out hitPlace))
        {
            // If it hits the table.
            if (hitPlace.collider.gameObject.CompareTag("Table"))
            {
                if (gameObject.name.Contains("TRAIL") && !gameObject.name.Contains("BIG"))
                {
                    // Position flame on table but not slightly above since flame is already small.
                    flameStackTransform.position = hitPlace.point + new Vector3(0, 0.003f, 0);
                }
                else
                {
                    // Position flame on table (slightly above).
                    flameStackTransform.position = hitPlace.point + new Vector3(0, 0.003f + ComputeYPosition(flameStackTransform.localScale.y), 0);
                }
            }
        }
    }

    public static float ComputeYPosition(float scale)
    {
        float scale1 = 0.15f, yPos1 = 0.0f;
        float scale2 = 0.98f, yPos2 = 0.014f;

        float m = (yPos2 - yPos1) / (scale2 - scale1);
        float b = yPos1 - m * scale1;

        float yPos = m * scale + b;

        return yPos;
    }

    private IEnumerator DelayedSetupInitialFlame()
    {
        yield return null;
        SetupFlame();
    }

    private IEnumerator DelayedSetupTrailingFlame()
    {
        yield return null;  // Wait for the next frame

        SetupFlame();

        PlaceFlameOnTable();

        StartCoroutine(RiseFromTable(0.5f));
    }

    private IEnumerator GrowTrailFlame()
    {
        float growthSpeed = 0.0001f;
        Transform trailFlameTransform = gameObject.transform;

        while (trailFlameTransform.localScale.y <= trailFlameGrowthThreshold)
        {
            if (trailFlameTransform.localScale.x < 1)
                trailFlameTransform.localScale += new Vector3(growthSpeed, growthSpeed, 0);
            else
                trailFlameTransform.localScale += new Vector3(0, growthSpeed, 0);

            PlaceFlameOnTable();

            yield return null;
        }

        // Once the flame reaches the growth threshold, add the FlameHeightController component so it starts varying in height.
        FlameHeightController flameHeightController = gameObject.AddComponent<FlameHeightController>();
        flameHeightController.baseGrowthSpeed = 0.05f;
    }

    private IEnumerator RiseFromTable(float duration)
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

                // SetHexagonsOpacity(flame, opacity);
                SetHexagonsOpacity(opacity);
            }
            else
            {
                // SetHexagonsOpacity(flame, 0.0f);
                SetHexagonsOpacity(0.0f);
            }

            yield return null;
        }

        float random = Random.Range(0.0f, 1.0f);
        bool isBig = (trailFlameGrowthChance - random >= 0);

        // Start growth process with certain probability.
        if (isBig)
        {
            gameObject.name += "BIG";
            StartCoroutine(GrowTrailFlame());
        }
    }
}
