using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public PlayerData playerData; // Reference to PlayerData scriptable object

    void Update()
    {
        // Check for player death
        if (playerData.IsDead())
        {
            // Handle player death here (e.g., show game over, respawn, etc.)
            Debug.Log("Player is dead!");
        }
    }

    // Method to apply damage (can be called from other scripts like Projectile)
    public void ApplyDamage(int damage)
    {
        playerData.TakeDamage(damage);
        Debug.Log($"Player took {damage} damage. Current Health: {playerData.startingHealth}");
    }

    // Method to heal (can be called when the player collects health items, etc.)
    public void HealPlayer(int amount)
    {
        playerData.Heal(amount);
        Debug.Log($"Player healed {amount}. Current Health: {playerData.startingHealth}");
    }
}
