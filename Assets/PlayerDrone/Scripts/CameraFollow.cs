using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    public Vector3 offset = new Vector3(0, 3, -6);
    public float positionSmoothSpeed = 5f;
    public float rotationSmoothSpeed = 3f;

    [Header("Tilt Settings")]
    public float tiltAmount = 10f;   // how much camera tilts on turns
    public float tiltSmooth = 5f;

    private Transform target;
    private float currentTilt = 0f;

    void Start()
    {
        // Auto find drone by tag
        GameObject drone = GameObject.FindGameObjectWithTag("Player");
        if (drone != null)
        {
            target = drone.transform;
        }
        else
        {
            Debug.LogWarning("No GameObject with tag 'Player' found. Please tag your Drone as Player.");
        }
    }

    void LateUpdate()
    {
        if (!target) return;

        // --- Smooth Position ---
        Vector3 desiredPosition = target.TransformPoint(offset);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, positionSmoothSpeed * Time.deltaTime);

        // --- Smooth Rotation ---
        Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSmoothSpeed * Time.deltaTime);

        // --- Camera Tilt (leans on left/right input) ---
        float horizontalInput = Input.GetAxis("Horizontal"); // A/D or Left/Right arrows
        float targetTilt = -horizontalInput * tiltAmount;    // negative so it tilts the right way
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * tiltSmooth);

        // Apply tilt to camera’s roll
        transform.rotation *= Quaternion.Euler(0, 0, currentTilt);
    }
}
