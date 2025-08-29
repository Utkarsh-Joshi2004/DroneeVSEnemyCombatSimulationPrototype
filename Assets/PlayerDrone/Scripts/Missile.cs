using UnityEngine;

public class Missile : MonoBehaviour
{
    [Header("Missile Settings")]
    public float speed = 25f;
    public float lifeTime = 5f;
    public int damage = 25;        // damage dealt to enemies

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
        // Check if hit an enemy
        EnemyHealth enemy = collision.collider.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        // Spawn explosion effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
