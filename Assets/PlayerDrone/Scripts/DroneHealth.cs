using UnityEngine;
using System.Collections;

public class DroneHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    int currentHealth;

    [Header("Death")]
    public float quitDelaySeconds = 2f;   // delay before quitting

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"[DroneHealth] Took {amount} damage. HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            StartCoroutine(DieAndQuit());
        }
    }

    IEnumerator DieAndQuit()
    {
        Debug.Log("[DroneHealth] Drone destroyed. Quitting…");
        gameObject.SetActive(false);
        yield return new WaitForSeconds(quitDelaySeconds);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
