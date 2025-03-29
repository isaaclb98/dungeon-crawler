using UnityEngine;

public class KillPlane : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the PlayerStats component from the collided object
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();

            if (playerStats != null)
            {
                // Call a method on the player stats if needed
                playerStats.Die(); // Example: if you have a Die() method in PlayerStats
            }

            // Destroy the player object
            Destroy(collision.gameObject);
        }
    }
}