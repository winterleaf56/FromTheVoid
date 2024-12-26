using UnityEngine;


// Self note, this does not cap the max and min rotation of the camera
public class CameraController : MonoBehaviour {
    public float moveSpeed = 10f;
    public float rotationSpeed = 100f;
    public GameObject boundaryObject;

    private Vector2 xBounds;
    private Vector2 zBounds;

    void Start() {
        if (boundaryObject != null) {
            Renderer renderer = boundaryObject.GetComponent<Renderer>();
            if (renderer != null) {
                Vector3 size = renderer.bounds.size;
                Vector3 center = boundaryObject.transform.position;
                xBounds = new Vector2(center.x - size.x / 2, center.x + size.x / 2);
                zBounds = new Vector2(center.z - size.z / 2, center.z + size.z / 2);
            }
        }
    }

    void Update() {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        Vector3 newPosition = transform.position + transform.TransformDirection(movement) * moveSpeed * Time.deltaTime;

        newPosition.y = transform.position.y;

        newPosition.x = Mathf.Clamp(newPosition.x, xBounds.x, xBounds.y);
        newPosition.z = Mathf.Clamp(newPosition.z, zBounds.x, zBounds.y);

        transform.position = newPosition;

        if (Input.GetMouseButton(2)) {
            float rotationX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float rotationY = -Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

            // Rotate the camera
            transform.Rotate(Vector3.up, rotationX, Space.World);
            transform.Rotate(Vector3.right, rotationY, Space.Self);

            // Clamp the rotation on the X-axis
            Vector3 currentRotation = transform.localEulerAngles;
            currentRotation.x = Mathf.Clamp(currentRotation.x > 180 ? currentRotation.x - 360 : currentRotation.x, -20f, 55f);
            transform.localEulerAngles = new Vector3(currentRotation.x, currentRotation.y, currentRotation.z);
        }
    }
}