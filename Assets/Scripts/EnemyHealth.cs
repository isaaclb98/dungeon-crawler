using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public EnemyData enemyData;
    private int currentHP;

    void Start()
    {
        currentHP = enemyData.enemyHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
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