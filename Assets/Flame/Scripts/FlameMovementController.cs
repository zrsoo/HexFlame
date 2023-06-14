using UnityEngine;

public class FlameMovementController : MonoBehaviour
{
    private RaycastHit hitMove;

    private bool flameOnTable;

    private Vector3 movementDirection;
    public float speed;

    public GameObject flamePrefab;

    private Vector3 lastTrailPosition;
    public float trailSpawnPositionDifference = 0.01f;

    private Transform gameObjectTransform;

    public GameObject surface;

    // Start is called before the first frame update
    private void Start()
    {
        QualitySettings.vSyncCount = 0; // Disable Sync

        GlobalFlameManager.instance.RegisterSurface(surface);

        gameObjectTransform = transform;

        speed = 0.01f;

        GenerateRandomMovementDirection();
    }

    // Update is called once per frame
    private void Update()
    {
        // Comment next 3 lines for stationary flame
        transform.Translate(movementDirection * speed * Time.deltaTime, Space.World);
        KeepFlameOnSurface();
        LeaveTrail();
    }

    private void KeepFlameOnSurface()
    {
        // Cast ray straight down (while looking ahead, in order to change course before going off the table).
        if (Physics.Raycast(transform.position + movementDirection * (speed + 0.5f) * Time.deltaTime, -Vector3.up, out hitMove))
        {
            // If it hits the surface.
            if (hitMove.collider.gameObject == surface)
            {
                flameOnTable = true;
            }
            else
            {
                flameOnTable = false;
            }
        }

        // If the flame is not on the surface, pick another random direction.
        if (!flameOnTable)
        {
            GenerateRandomMovementDirection();
            flameOnTable = true;
        }
    }

    private void GenerateRandomMovementDirection()
    {
        Vector3 surfaceCenterPosition = new Vector3(surface.transform.position.x, transform.position.y, surface.transform.position.z);
        Vector3 directionToSurfaceCenter = (surfaceCenterPosition - transform.position).normalized;

        // Create a random offset.
        float angleOffset = Random.Range(-45.0f, 45.0f); // Change this range depending on how much randomness you want
        Quaternion rotation = Quaternion.Euler(0, angleOffset, 0);

        // Apply the random offset to the direction.
        movementDirection = rotation * directionToSurfaceCenter;
    }

    private void LeaveTrail()
    {
        float distanceTraveled = Vector3.Distance(gameObjectTransform.position, lastTrailPosition);

        if (distanceTraveled > trailSpawnPositionDifference)
        {
            GameObject newFlame = Instantiate(flamePrefab, gameObjectTransform.position - new Vector3(0.0f, 0.0f, 0.01f), gameObjectTransform.rotation);
            // newFlame.SetActive(false);

            // newFlame.SetActive(true);
            newFlame.name += "TRAIL";

            lastTrailPosition = gameObjectTransform.position;
        }
    }
}