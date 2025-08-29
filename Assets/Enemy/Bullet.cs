using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 10;
    public float lifeTime = 5f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Bullet triggered with: " + other.name + " (Tag: " + other.tag + ")");

        if (other.CompareTag("Player"))
        {
            DroneHealth drone = other.GetComponent<DroneHealth>();
            if (drone == null)
            {
                drone = other.GetComponentInParent<DroneHealth>();
            }

            if (drone != null)
            {
                drone.TakeDamage(damage);
                Debug.Log("✅ Bullet damaged drone for " + damage);
            }
            else
            {
                Debug.LogWarning("⚠ DroneHealth not found on Player object!");
            }
        }

        Destroy(gameObject);
    }
}
