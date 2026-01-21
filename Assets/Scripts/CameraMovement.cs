using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    public Transform target;            // Your Player
    public float smoothTime = 0.15f;    // Faster response for top-down
    public Vector3 offset = new Vector3(0, 15, -10); // High Y, negative Z
    public float cameraAngle = 65f;     // The downward tilt

    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        // Set the initial rotation for a top-down view
        transform.rotation = Quaternion.Euler(cameraAngle, 0, 0);
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Calculate the position we want the camera to be at
        Vector3 targetPosition = target.position + offset;

        // Smoothly move the camera
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}