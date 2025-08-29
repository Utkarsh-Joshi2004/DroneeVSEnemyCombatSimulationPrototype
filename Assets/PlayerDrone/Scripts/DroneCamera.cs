using UnityEngine;

public class DroneCamera : MonoBehaviour
{
    public Transform drone;
    public Vector3 offset = new Vector3(0, 2, -5);
    public float smoothSpeed = 0.125f;
    public float rotationSmoothSpeed = 0.1f;

    private void LateUpdate()
    {
        if (drone == null) return;

        // Desired position
        Vector3 desiredPosition = drone.position + drone.TransformDirection(offset);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Smooth rotation to look at the drone
        Quaternion targetRotation = Quaternion.LookRotation(drone.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothSpeed);
    }
}
