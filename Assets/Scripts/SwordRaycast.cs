using UnityEngine;

public class SwordRaycast : MonoBehaviour
{
    public float range = 2f; // The range of the sword attack
    public Camera playerCamera; // Reference to the player's camera

    // This function is triggered by the animation event
    public void PerformRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, range))
        {
            Debug.Log("Hit: " + hit.collider.name); // Log the hit object

            // Check if the object is tagged as "Enemy"
            if (hit.collider.CompareTag("Enemy"))
            {
                Destroy(hit.collider.gameObject); // Destroy the enemy
                Debug.Log("Enemy Destroyed!");
            }
        }
    }
}
