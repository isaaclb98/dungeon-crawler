using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChestController : MonoBehaviour
{
    public Animator animator;
    // private bool isOpen = false;

    void Start()
    {
        //animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Check if player is near the chest and presses 'E'
        if (Input.GetKeyDown(KeyCode.E) && IsPlayerNear())
        {
            ToggleChest();
        }
        else if(Input.GetKeyDown(KeyCode.C) && IsPlayerNear() && animator.GetCurrentAnimatorStateInfo(0).IsName("TreasureChest_OPEN"))
        {
            animator.Play("TreasureChest_CLOSE");
        }
    }

    void ToggleChest()
    {
        animator.Play("TreasureChest_OPEN");
        //isOpen = !isOpen; // Toggle state
        //animator.SetBool("OpenChest", isOpen);
    }

    bool IsPlayerNear()
    {
        // Adjust distance based on your game scale
        float interactDistance = 3f;
        GameObject player = GameObject.FindGameObjectWithTag("Player");//Optimisation

        if (player == null) return false;

        if(Vector3.Distance(transform.position, player.transform.position) <= interactDistance)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
}