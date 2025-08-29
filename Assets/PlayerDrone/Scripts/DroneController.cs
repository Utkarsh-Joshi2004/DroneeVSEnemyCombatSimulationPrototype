using UnityEngine;

public class DroneController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float ascendDescendSpeed = 5f;
    public float rotationSpeed = 5f;

    [Header("Combat Settings")]
    public GameObject missilePrefab;
    public Transform missileSpawnPoint;
    public float missileForce = 1000f;

    [Header("Effects")]
    public ParticleSystem missileFireEffect;
    public AudioClip missileFireSound;

    private Rigidbody rb;
    private AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        HandleMovement();
        HandleCombat();
    }

    void HandleMovement()
    {
        // Get input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float ascend = 0f;

        if (Input.GetKey(KeyCode.E)) ascend = 1f;
        if (Input.GetKey(KeyCode.Q)) ascend = -1f;

        // Calculate movement vectors
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;
        Vector3 worldMove = transform.TransformDirection(moveDirection) * moveSpeed;
        Vector3 verticalMove = Vector3.up * ascend * ascendDescendSpeed;

        // Apply movement
        Vector3 movement = worldMove + verticalMove;
        rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y + verticalMove.y, movement.z);

        // Smooth rotation toward movement direction
        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void HandleCombat()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireMissile();
        }
    }

    void FireMissile()
    {
        if (missilePrefab && missileSpawnPoint)
        {
            // Instantiate missile
            GameObject missile = Instantiate(missilePrefab, missileSpawnPoint.position, missileSpawnPoint.rotation);
            Rigidbody missileRb = missile.GetComponent<Rigidbody>();

            // Apply force
            if (missileRb)
            {
                missileRb.AddForce(missileSpawnPoint.forward * missileForce);
            }

            // Play effects
            if (missileFireEffect)
            {
                missileFireEffect.Play();
            }

            if (missileFireSound && audioSource)
            {
                audioSource.PlayOneShot(missileFireSound);
            }
        }
    }
}