using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int attackDamage = 2;
    public float attackCooldown = 2f; 
    private float lastAttackTime;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return; // Ignore non-player objects

        Debug.Log($"Enemy entered collision with Player.");
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return; // Ignore non-player objects

        Debug.Log("Enemy detected the player.");

        if (Time.time > lastAttackTime + attackCooldown)
        {
            Debug.Log("Enemy attacking the player!");
            PlayerStats.Instance.TakeDamage(attackDamage);
            lastAttackTime = Time.time; // Update attack cooldown timer
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return; // Ignore non-player objects

        Debug.Log("Enemy stopped colliding with Player.");
    }
}
