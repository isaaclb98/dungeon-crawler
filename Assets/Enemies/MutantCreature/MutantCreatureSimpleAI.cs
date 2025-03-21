using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MutantCreatureSimpleAI : MonoBehaviour
{
    public EnemyData enemyData; // Reference to the EnemyData ScriptableObject
    public float chaseSpeed = 4f;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 2f;

    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private bool isAttacking = false;
    private float lastAttackTime = 0f;

    private PlayerHealthAndStamina playerHealth;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.enabled = true; // Ensure the Animator is active
        player = GameObject.FindGameObjectWithTag("Player").transform;

        agent.speed = chaseSpeed;
        playerHealth = player.GetComponent<PlayerHealthAndStamina>(); // Get reference to PlayerHealthAndStamina script
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Attack range check
        if (distanceToPlayer <= attackRange)
        {
            Attack();
        }
        // Chase the player if within detection range
        else if (distanceToPlayer <= detectionRange)
        {
            Chase();
        }
    }

    void Chase()
    {
        // Move towards the player
        agent.destination = player.position;
        animator.SetBool("isWalking", true);
        animator.SetBool("isAttacking", false);  // Ensure the monster is walking and not attacking
    }

    void Attack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;

        Debug.Log("Attacking Player!"); // Debugging output
        isAttacking = true;

        // Stop moving while attacking
        agent.isStopped = true;
        animator.SetBool("isAttacking", true);
        animator.SetBool("isWalking", false);

        // Now attack the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(enemyData.enemyAttack); 
                Debug.Log($"Player Health after attack: {playerHealth.GetCurrentHealth()}"); // Debug the player's health
            }
        }

        lastAttackTime = Time.time;
        StartCoroutine(ResetAttack());
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        agent.isStopped = false;
    }
}

