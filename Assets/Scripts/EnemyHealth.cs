using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public EnemyData enemyData;
    private int currentHP;
    public GameObject floatingDamagePrefab;
    public ParticleSystem deathParticlesPrefab;
    
    void Start()
    {
        currentHP = enemyData.enemyHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (floatingDamagePrefab)
        {
            // Find the player's transform by tag
            Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        
            // Calculate the direction from the enemy to the player
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        
            // Base offset: 1 unit toward the player and 0.5 unit upward.
            Vector3 offset = directionToPlayer * 1f + Vector3.up * 0.5f;
        
            // Randomize lateral position
            Vector3 right = Vector3.Cross(Vector3.up, directionToPlayer).normalized;
            offset += right * Random.Range(-0.3f, 0.3f);

            // Instantiate the floating damage prefab
            GameObject floatingDamageInstance = Instantiate(floatingDamagePrefab, transform.position + offset, Quaternion.identity);
        
            // Compute the direction from the damage text to the player
            Vector3 lookDirection = (playerTransform.position - floatingDamageInstance.transform.position).normalized;
            
            // Set rotation so the forward direction of the object points toward the player.
            floatingDamageInstance.transform.rotation = Quaternion.LookRotation(lookDirection);
        
            // Mirror the text
            floatingDamageInstance.transform.Rotate(0, 180f, 0);
        
            // Get the FloatingDamage component and set the damage number.
            FloatingDamage fd = floatingDamageInstance.GetComponent<FloatingDamage>();
            if (fd)
            {
                fd.SetDamage(damage);
            }
        }

        if (currentHP <= 0)
        {
            Die();
        }
    }




    private void Die()
    {
        // Play the particle effect
        if (deathParticlesPrefab)
        {
            ParticleSystem ps = Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
            ps.Play();

            // Destroy the particle system object after it finishes
            Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
        }   
        
        // Give rewards to the player
        PlayerStats.Instance.GainXp(enemyData.xpReward);
        PlayerStats.Instance.GainGold(enemyData.goldReward);

        Debug.Log(enemyData.enemyName + " defeated! XP: " + enemyData.xpReward + ", Gold: " + enemyData.goldReward);

        // Destroy the enemy
        Destroy(gameObject);
    }
}