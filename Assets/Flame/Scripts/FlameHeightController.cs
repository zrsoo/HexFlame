using System.Collections;
using UnityEngine;

public class FlameHeightController : MonoBehaviour
{
    public float baseGrowthSpeed = 0.05f;
    private bool isGrowing = true;
    private float minFlameHeight;
    private float maxFlameHeight;
    private float randomGrowthFactor;
    private float transitionDuration = 10f; // duration over which random values will change

    private Transform localTransform;

    private FlameController flameController;

    // Start is called before the first frame update
    private void Start()
    {
        flameController = GetComponent<FlameController>();

        minFlameHeight = flameController.GetTrailFlameGrowthThreshold();

        randomGrowthFactor = Random.Range(0.8f, 1.7f);
        maxFlameHeight = Random.Range(minFlameHeight, 1.4f);

        localTransform = transform;

        StartCoroutine(ChangeGrowthFactorOverTime());
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateFlameHeight();
    }

    private IEnumerator ChangeGrowthFactorOverTime()
    {
        while (true)
        {
            float initialGrowthFactor = randomGrowthFactor;
            float finalGrowthFactor = Random.Range(0.7f, 1.0f);

            float timePassed = 0;
            while (timePassed < transitionDuration)
            {
                randomGrowthFactor = Mathf.Lerp(initialGrowthFactor, finalGrowthFactor, timePassed / transitionDuration);
                timePassed += Time.deltaTime;
                yield return null;
            }
        }
    }

    private void UpdateFlameHeight()
    {
        float prevHeight = transform.localScale.y;
        float prevWitdh = transform.localScale.x;

        // Determine the current direction of growth.
        if (localTransform.localScale.y >= maxFlameHeight)
        {
            isGrowing = false;
        }
        else if (localTransform.localScale.y <= minFlameHeight)
        {
            isGrowing = true;
        }

        // Adjust growth speed based on direction and randomness.
        float currentGrowthSpeed = baseGrowthSpeed * randomGrowthFactor;
        float currentHeight = prevHeight + Time.deltaTime * currentGrowthSpeed * (isGrowing ? 1 : -1);
        float currentWidth = prevWitdh + Time.deltaTime * currentGrowthSpeed * (isGrowing ? 1 : -1);

        // Keep the currentHeight within bounds.
        currentHeight = Mathf.Clamp(currentHeight, minFlameHeight, maxFlameHeight);
        currentWidth = Mathf.Clamp(currentWidth, minFlameHeight, maxFlameHeight);

        localTransform.localScale = new Vector3(currentWidth, currentHeight, localTransform.localScale.z);
        flameController.PlaceFlameOnSurface();
    }
}