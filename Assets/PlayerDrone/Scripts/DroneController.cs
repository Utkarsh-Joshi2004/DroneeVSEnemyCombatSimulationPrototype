using UnityEngine;

public class DroneController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float ascendSpeed = 5f;
    public float rotationSpeed = 100f; 

    [Header("Missile Settings")]
    public GameObject missilePrefab;
    public Transform firePoint;

    [Header("Effects")]
    public ParticleSystem fireEffect;
    public AudioSource fireSound;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // drone hovers
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
            // Spawn missile
            GameObject missile = Instantiate(missilePrefab, firePoint.position, firePoint.rotation);

            //Ignore collision with drone 
            Collider droneCollider = GetComponent<Collider>();
            Collider missileCollider = missile.GetComponent<Collider>();
            if (droneCollider != null && missileCollider != null)
            {
                Physics.IgnoreCollision(missileCollider, droneCollider);
            }

            // Play effects
            if (fireEffect != null) fireEffect.Play();
            if (fireSound != null) fireSound.Play();
        }
    }
}
