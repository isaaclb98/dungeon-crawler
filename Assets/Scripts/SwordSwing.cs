using UnityEngine;

public class SwordSwing : MonoBehaviour
{
    private Animator animator;
    private bool canAttack = true; // Prevents continuous swinging
    public WeaponData weaponData; // Reference to weapon stats

    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("❌ No Animator found on the Sword prefab!");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canAttack) // If left click and can attack
        {
            Attack();
        }
    }

    void Attack()
    {
        if (animator == null)
        {
            Debug.LogError("❌ Animator is missing, cannot play animation!");
            return;
        }

        animator.SetTrigger("Swing"); // Play swing animation
        canAttack = false; // Stop further attacks until cooldown

        // If using WeaponData, delay should be based on attackSpeed, otherwise default to 1 sec
        float attackDelay = weaponData != null ? weaponData.attackSpeed : 1.0f;
        Invoke("ResetAttack", attackDelay);
    }

    void ResetAttack()
    {
        animator.Play("Idle"); // Force back to Idle state
        canAttack = true;
    }
}
