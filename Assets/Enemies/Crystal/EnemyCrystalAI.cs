using System.Collections;
using UnityEngine;

public class HallowedCrystal : MonoBehaviour
{
    public GameObject projectilePrefab; // Assign EnergyProjectile prefab here
    public Transform firePoint; // The monster itself is the firepoint
    public float projectileSpeed = 10f;
    public float timeBetweenBursts = 3f;
    public AudioSource chargingSound; // Drag an AudioSource with a charging sound
    public float shakeIntensity = 0.1f;
    public float shakeDuration = 1f;
    public float detectrange = 15f;

    private int[] projectilePattern = { 4, 6, 8 , 12, 4, 8, 6}; // The shooting pattern
    private int currentPatternIndex = 0;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(ShootProjectiles());
    }

    IEnumerator ShootProjectiles()
    {
        while (player != null && Vector3.Distance(transform.position, player.transform.position) <= detectrange)
        {
         
            yield return StartCoroutine(ChargeAttack()); // Play charge-up effect

            int projectileCount = projectilePattern[currentPatternIndex];
            FireInAllDirections(projectileCount);

            currentPatternIndex = (currentPatternIndex + 1) % projectilePattern.Length; // Cycle through 4-6-8 pattern
            yield return new WaitForSeconds(timeBetweenBursts);
        }
    }

    IEnumerator ChargeAttack()
    {
        // Play Charging Sound
        if (chargingSound != null) chargingSound.Play();

        // Shake Effect
        Vector3 originalPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            transform.position = originalPosition + (Random.insideUnitSphere * shakeIntensity);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition; // Reset position
    }

    void FireInAllDirections(int count)
    {
        float angleStep = 360f / count;
        for (int i = 0; i < count; i++)
        {
            float angle = i * angleStep;
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward; // Use monster's forward direction

            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody>().velocity = direction * projectileSpeed;

            // Rotate projectile to face its movement direction
            projectile.transform.rotation = Quaternion.LookRotation(direction);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectrange);
    }
}