using UnityEngine;

public class Missile : MonoBehaviour
{
    public GameObject explosionPrefab;

    [Header("Missile Settings")]
    public float speed = 50f;
    public float lifetime = 5f;

    [Header("Explosion Effect")]
    public ParticleSystem explosionEffectPrefab;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;

        // Destroy after lifetime if it doesn't hit anything
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // If collided with enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Delay enemy destruction slightly so explosion is visible
            Destroy(collision.gameObject, 0.2f);
            Explosion();
        }

        else
        {
            Explosion();
        }

        // Destroy missile immediately on impact
        Destroy(gameObject);
    }

    void Explosion()
    {
        GameObject Explosion = Instantiate(explosionPrefab, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(Explosion, 1f);
    }
}
