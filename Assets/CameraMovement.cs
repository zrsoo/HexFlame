using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float panSpeed = 10.0f;
    public float rotateSpeed = 50.0f;
    public float zoomSpeed = 1000.0f;

    private Vector3 panMovement;

    void Start()
    {
    }

    void Update()
    {
        PanCamera();
        RotateCamera();
        ZoomCamera();
    }

    void PanCamera()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        panMovement = new Vector3(-horizontalInput, 0, -verticalInput).normalized * panSpeed * Time.deltaTime;

        transform.Translate(panMovement, Space.World);
    }

    void RotateCamera()
    {
        if (Input.GetMouseButton(1))
        {
            float horizontalMouseInput = Input.GetAxis("Mouse X");
            float verticalMouseInput = Input.GetAxis("Mouse Y");

            transform.RotateAround(transform.position, Vector3.up, horizontalMouseInput * rotateSpeed * Time.deltaTime);
            transform.RotateAround(transform.position, transform.right, -verticalMouseInput * rotateSpeed * Time.deltaTime);
        }
    }

    void ZoomCamera()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        Vector3 zoomMovement = scrollInput * zoomSpeed * Time.deltaTime * transform.forward;

        transform.Translate(zoomMovement, Space.World);
    }
}
