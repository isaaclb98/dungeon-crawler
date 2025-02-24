using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public EnemyData enemyData;
    private int currentHP;
    public GameObject floatingDamagePrefab;
    
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
        
            // Base offset: 1 unit toward the player and 1 unit downward.
            Vector3 offset = directionToPlayer * 1f + Vector3.up * -1f;
        
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
        // Give rewards to the player
        PlayerStats.Instance.GainXp(enemyData.xpReward);
        PlayerStats.Instance.GainGold(enemyData.goldReward);

        Debug.Log(enemyData.enemyName + " defeated! XP: " + enemyData.xpReward + ", Gold: " + enemyData.goldReward);

        // Destroy the enemy
        Destroy(gameObject);
    }
}