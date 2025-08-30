using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 100;

    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log("[EnemyHealth] Took damage, health now: " + health);

        if (health <= 0)
        {
            Debug.Log("[EnemyHealth] Enemy dead!");
            Destroy(gameObject);
        }
    }
}
