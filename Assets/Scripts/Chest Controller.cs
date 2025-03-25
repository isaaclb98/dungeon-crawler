using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChestController : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Check if player is near the chest and presses 'E'
        if (Input.GetKeyDown(KeyCode.E) && IsPlayerNear())
        {
            ToggleChest();
        }
    }

    void ToggleChest()
    {
        isOpen = !isOpen; // Toggle state
        animator.SetBool("OpenChest", isOpen);
    }

    bool IsPlayerNear()
    {
        // Adjust distance based on your game scale
        float interactDistance = 3f;
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null) return false;

        return Vector3.Distance(transform.position, player.transform.position) <= interactDistance;
    }
}