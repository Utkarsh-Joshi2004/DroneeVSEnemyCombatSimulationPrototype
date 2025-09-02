using UnityEngine;

public class DroneController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float ascendSpeed = 5f;
    public float rotationSpeed = 100f;
    public float acceleration = 5f; // how fast drone accelerates
    public float deceleration = 5f; // how fast drone slows down

    [Header("Missile Settings")]
    public GameObject missilePrefab;
    public Transform firePoint;

    [Header("Effects")]
    public ParticleSystem fireEffect;
    public AudioSource fireSound;

    private Rigidbody rb;
    private Vector3 targetVelocity;
    private float targetRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // drone hovers
    }

    void Update()
    {
        HandleInput();
        HandleFiring();
    }

    void FixedUpdate()
    {
        ApplyMovement();
        ApplyRotation();
    }

    void HandleInput()
    {
        // Forward/backward
        float moveForward = Input.GetAxis("Vertical") * moveSpeed;

        // Ascend/Descend
        float ascend = 0f;
        if (Input.GetKey(KeyCode.E)) ascend = ascendSpeed;
        if (Input.GetKey(KeyCode.Q)) ascend = -ascendSpeed;

        // Set target velocity
        targetVelocity = transform.forward * moveForward + Vector3.up * ascend;

        // Rotation input
        targetRotation = Input.GetAxis("Horizontal") * rotationSpeed;
    }

    void ApplyMovement()
    {
        // Smooth velocity change instead of snapping
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
    }

    void ApplyRotation()
    {
        if (Mathf.Abs(targetRotation) > 0.01f)
        {
            Quaternion deltaRotation = Quaternion.Euler(Vector3.up * targetRotation * Time.fixedDeltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
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
            // Spawn missile
            GameObject missile = Instantiate(missilePrefab, firePoint.position, firePoint.rotation);

            // Ignore collisions with drone
            Collider[] droneColliders = GetComponentsInChildren<Collider>();
            Collider missileCollider = missile.GetComponent<Collider>();

            if (missileCollider != null)
            {
                foreach (Collider c in droneColliders)
                {
                    Physics.IgnoreCollision(missileCollider, c);
                }
            }

            // Play firing effect & sound
            if (fireEffect != null) fireEffect.Play();
            if (fireSound != null) fireSound.Play();
        }
    }
}
