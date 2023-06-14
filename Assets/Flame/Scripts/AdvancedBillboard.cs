using UnityEngine;

public class AdvancedBillboard : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 initialPosition;
    public float offsetRadius;

    private void Start()
    {
        mainCamera = Camera.main;
        initialPosition = transform.position;
    }

    private void Update()
    {
        Vector3 directionToCamera = mainCamera.transform.position - initialPosition;
        directionToCamera.y = 0f;

        // Normalize the direction to get a unit vector, and scale by the offset radius.
        Vector3 offsetDirection = directionToCamera.normalized * offsetRadius;

        // Set the new position to be the initial position plus the offset.
        transform.position = initialPosition + offsetDirection;

        if (directionToCamera != Vector3.zero) // To avoid errors when the object is exactly at the camera position
        {
            transform.rotation = Quaternion.LookRotation(-directionToCamera, Vector3.up);
        }
    }
}