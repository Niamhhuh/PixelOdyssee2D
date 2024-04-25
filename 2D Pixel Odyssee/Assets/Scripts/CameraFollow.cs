using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform target;
    [SerializeField] private float yOffset = 0f; // Offset in the Y direction

    private void Update()
    {
        // Calculate target position with only X-axis following
        Vector3 targetPosition = new Vector3(target.position.x, transform.position.y + yOffset, transform.position.z);

        // Smoothly move the camera to the target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
