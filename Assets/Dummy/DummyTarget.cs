using UnityEngine;

public class DummyTarget : MonoBehaviour
{
    [Header("Respawn Settings")]
    public float respawnDelay = 2f;          
    public Vector3 respawnAreaCenter;        
    public Vector3 respawnAreaSize = new Vector3(10, 0, 10); 

    private Quaternion initialRotation;      
    private GameObject dummyInstance;        
    private bool isDestroyed = false;

    void Start()
    {
        // Save original rotation
        initialRotation = transform.rotation;
        dummyInstance = gameObject;

        // If center not set in Inspector, use current position
        if (respawnAreaCenter == Vector3.zero)
            respawnAreaCenter = transform.position;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Only react to missiles
        if (collision.gameObject.CompareTag("Missile"))
        {
            DestroyDummy();
        }
    }

    void DestroyDummy()
    {
        if (isDestroyed) return; // Prevent multiple triggers

        isDestroyed = true;

        // Hide dummy
        dummyInstance.SetActive(false);

        // Respawn after delay
        Invoke(nameof(RespawnDummy), respawnDelay);
    }

    void RespawnDummy()
    {
        // Pick random point inside respawn area
        Vector3 randomOffset = new Vector3(
            Random.Range(-respawnAreaSize.x / 2, respawnAreaSize.x / 2),
            Random.Range(-respawnAreaSize.y / 2, respawnAreaSize.y / 2),
            Random.Range(-respawnAreaSize.z / 2, respawnAreaSize.z / 2)
        );

        Vector3 respawnPosition = respawnAreaCenter + randomOffset;

        dummyInstance.transform.position = respawnPosition;
        dummyInstance.transform.rotation = initialRotation;
        dummyInstance.SetActive(true);

        isDestroyed = false;
    }

    //Draw respawn area in Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawCube(respawnAreaCenter, respawnAreaSize);
    }
}
