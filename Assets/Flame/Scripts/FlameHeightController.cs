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
    void Start()
    {
        flameController = GetComponent<FlameController>();

        minFlameHeight = flameController.GetTrailFlameGrowthThreshold();

        randomGrowthFactor = Random.Range(0.8f, 1.7f);
        maxFlameHeight = Random.Range(0.4f, 1.4f);

        localTransform = transform;

        StartCoroutine(ChangeGrowthFactorOverTime());
        StartCoroutine(ChangeMaxHeightOverTime());
    }

    // Update is called once per frame
    void Update()
    {
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
            float initialMaxHeight = 1.0f;
            float finalMaxHeight = Random.Range(0.4f, 1.4f);

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
        flameController.PlaceFlameOnTable();
    }
}
