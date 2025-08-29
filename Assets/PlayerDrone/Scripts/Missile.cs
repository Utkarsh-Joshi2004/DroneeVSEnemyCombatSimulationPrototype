using UnityEngine;

public class Missile : MonoBehaviour
{
    public float explosionRadius = 5f;
    public float explosionForce = 1000f;
    public GameObject explosionEffect;

    void OncollisionEnter(Collision collision)
    {
        Explode();
        Destroy(gameObject);
    }

    void Explode()
    {
        // Explosion effect
        if (explosionEffect )
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        // Apply physics force to nearby objects
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }
    }
}
