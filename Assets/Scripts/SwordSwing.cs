using UnityEngine;

public class SwordSwing : MonoBehaviour
{
    private Animator animator; // Reference to the Animator component

    void Start()
    {
        // Get the Animator component attached to this GameObject
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Detect left mouse button click
        if (Input.GetMouseButtonDown(0))
        {
            // Trigger the "Swing" animation
            animator.SetTrigger("Swing");
        }
    }
}
