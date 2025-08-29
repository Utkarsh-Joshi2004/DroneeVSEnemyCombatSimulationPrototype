using UnityEngine;

public class DroneController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float ascendSpeed = 5f;
    public float rotationSpeed = 100f; // yaw rotation speed

    [Header("Missile Settings")]
    public GameObject missilePrefab;
    public Transform firePoint;
    public float missileForce = 20f;

    [Header("Effects")]
    public ParticleSystem fireEffect;
    public AudioSource fireSound;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // keep drone hovering
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleFiring();
    }

    void HandleMovement()
    {
        // Forward/backward
        float moveForward = Input.GetAxis("Vertical") * moveSpeed;

        // Ascend/Descend
        float ascend = 0f;
        if (Input.GetKey(KeyCode.E)) ascend = ascendSpeed;
        if (Input.GetKey(KeyCode.Q)) ascend = -ascendSpeed;

        // Apply movement (in drone's forward direction)
        Vector3 move = transform.forward * moveForward + Vector3.up * ascend;
        rb.linearVelocity = move;
    }

    void HandleRotation()
    {
        // Rotate left/right with A/D
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up * rotation);
    }

    void HandleFiring()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireMissile();
        }
    }

    void FireMissile()
    {
        if (missilePrefab != null && firePoint != null)
        {
            GameObject missile = Instantiate(missilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody missileRb = missile.GetComponent<Rigidbody>();
            missileRb.AddForce(firePoint.forward * missileForce, ForceMode.VelocityChange);

            if (fireEffect != null) fireEffect.Play();
            if (fireSound != null) fireSound.Play();
        }
    }
}
