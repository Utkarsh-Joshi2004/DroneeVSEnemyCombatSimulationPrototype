using UnityEngine;

public class Missile : MonoBehaviour
{
    [Header("Missile Settings")]
    public float speed = 25f;
    public float lifeTime = 5f;
    public int damage = 25;

    [Header("Effects")]
    public GameObject explosionEffect;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Always fly forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // ✅ Ignore collision with drone
        if (collision.collider.CompareTag("Player"))
            return;

        // ✅ Check if hit enemy
        if (collision.collider.CompareTag("Enemy"))
        {
            Debug.Log("Missile hit enemy: " + collision.collider.name);

            EnemyHealth enemy = collision.collider.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        // Spawn explosion effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Destroy missile on any impact
        Destroy(gameObject);
    }
}
