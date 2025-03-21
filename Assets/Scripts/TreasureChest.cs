using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TreasureChest : MonoBehaviour
{
    public Animator chestAnimator; // Reference to the Animator component
    public bool isOpen = false;   // Track if the chest is open

    private void Update()
    {
        // Check if the player is near the chest and presses 'E'
        if (Input.GetKeyDown(KeyCode.E) && IsPlayerNear())
        {
            if (!isOpen)
            {
                OpenChest();
            }
        }
    }

    private bool IsPlayerNear()
    {
        // Define the range for detecting the player
        float detectionRange = 2.0f;

        // Find the player (assuming the player has a "Player" tag)
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // Check the distance between the chest and the player
            float distance = Vector3.Distance(transform.position, player.transform.position);
            return distance <= detectionRange;
        }

        return false;
    }

    private void OpenChest()
    {
        // Trigger the "Open" animation
        if (chestAnimator != null)
        {
            chestAnimator.SetTrigger("Open");
            isOpen = true;
            Debug.Log("Chest opened!");
        }
    }
}
