using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("AI Settings")]
    public Transform[] waypoints;
    public float detectionRange = 20f;
    public float fireRate = 1.5f;
    public float bodyRotationSpeed = 3f;

    [Header("References")]
    public GameObject bulletPrefab;
    public Transform firePoint;   // 👈 child of Enemy
    public Transform drone;       // assign Drone in inspector

    private NavMeshAgent agent;
    private int currentWaypoint = 0;
    private float fireCooldown = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (waypoints.Length > 0)
        {
            agent.destination = waypoints[0].position;
        }
    }

    void Update()
    {
        if (drone == null) return;

        float distance = Vector3.Distance(transform.position, drone.position);

        if (distance <= detectionRange)
        {
            // Stop patrol when attacking
            agent.isStopped = true;

            // Rotate enemy body horizontally toward drone
            Vector3 bodyDir = (drone.position - transform.position).normalized;
            bodyDir.y = 0;
            if (bodyDir.sqrMagnitude > 0.001f)
            {
                Quaternion targetRot = Quaternion.LookRotation(bodyDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, bodyRotationSpeed * Time.deltaTime);
            }

            // Fire bullets
            if (fireCooldown <= 0f)
            {
                Shoot();
                fireCooldown = fireRate;
            }
        }
        else
        {
            // Resume patrol
            agent.isStopped = false;
            Patrol();
        }

        if (fireCooldown > 0f)
            fireCooldown -= Time.deltaTime;
    }

    void Patrol()
    {
        if (waypoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
            agent.destination = waypoints[currentWaypoint].position;
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null || drone == null) return;

        // ✅ Calculate bullet direction toward drone
        Vector3 dir = (drone.position - firePoint.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(dir);

        // ✅ Spawn bullet at firePoint position, but rotated toward drone
        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.transform.position, lookRot);

        // Pass shooter reference to bullet (to ignore self-collision)
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
            bullet.SetShooter(gameObject);

        Debug.Log("[EnemyAI] Enemy fired at drone!");
    }

    // ✅ Debug Gizmos
    void OnDrawGizmosSelected()
    {
        // Draw detection radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Draw fire direction toward drone
        if (firePoint != null && drone != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(firePoint.position, drone.position);
        }
    }
}
