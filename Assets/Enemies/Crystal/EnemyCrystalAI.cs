using UnityEngine;
using System.Collections;

public class CrystalMonsterShooting : MonoBehaviour
{
    public GameObject projectilePrefab; // Assign crystal projectile prefab in the inspector
    public float projectileSpeed = 10f;
    public float shootInterval = 3f; // Time between attack cycles
    public float burstDelay = 0.2f; // Delay between each projectile in a burst

    private int[] shotPattern = { 4, 6, 8 }; // Pattern cycle
    private int patternIndex = 0; // Keeps track of current pattern

    void Start()
    {
        InvokeRepeating(nameof(StartShootingBurst), shootInterval, shootInterval);
    }

    void StartShootingBurst()
    {
        int projectileCount = shotPattern[patternIndex]; // Get current pattern count
        float angleStep = 360f / projectileCount; // Angle between projectiles

        // Start the coroutine to shoot with delays
        StartCoroutine(ShootBurst(projectileCount, angleStep));

        // Move to the next pattern (looping back to start)
        patternIndex = (patternIndex + 1) % shotPattern.Length;
    }

    IEnumerator ShootBurst(int projectileCount, float angleStep)
    {
        for (int i = 0; i < projectileCount; i++)
        {
            float angle = i * angleStep; // Calculate angle for each projectile
            FireProjectile(angle);
            yield return new WaitForSeconds(burstDelay); // Wait before firing next
        }
    }

    void FireProjectile(float angle)
    {
        if (projectilePrefab != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // Convert angle to direction
                Vector2 shootDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                rb.velocity = shootDirection * projectileSpeed;
            }
        }
    }
}


