using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10;
    public float lifeTime = 5f;
    public float speed = 30f;

    private Rigidbody rb;
    private GameObject shooter; // who fired this bullet

    // Called by the shooter right after instantiation
    public void SetShooter(GameObject owner)
    {
        shooter = owner;

        // Ignore all colliders on shooter
        Collider bulletCol = GetComponent<Collider>();
        Collider[] shooterCols = shooter.GetComponentsInChildren<Collider>();

        if (bulletCol != null)
        {
            foreach (var col in shooterCols)
            {
                if (col != null)
                    Physics.IgnoreCollision(bulletCol, col, true);
            }
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("[Bullet] Rigidbody missing!");
            return;
        }

        rb.useGravity = false;
        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        // Move forward
        rb.linearVelocity = transform.forward * speed;

        // Destroy after lifetime
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // 🚫 Ignore the shooter completely
        if (shooter != null && (other.gameObject == shooter || other.transform.IsChildOf(shooter.transform)))
            return;

        // ✅ Only damage Drone
        DroneHealth drone = other.GetComponent<DroneHealth>() ?? other.GetComponentInParent<DroneHealth>();
        if (drone != null)
        {
            drone.TakeDamage(damage);
            Debug.Log($"[Bullet] Damaged drone for {damage}");
        }

        // Destroy bullet no matter what it hit
        Destroy(gameObject);
    }
}
