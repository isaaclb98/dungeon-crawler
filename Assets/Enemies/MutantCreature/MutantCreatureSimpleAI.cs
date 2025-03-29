using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MutantCreatureSimpleAI : MonoBehaviour
{
    public float chaseSpeed = 4f;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 2f;

    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private float lastAttackTime = 0f;

    private PlayerStats playerHealth;
    public EnemyData enemyData;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.enabled = true;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player == null)
        {
            Debug.LogError("Player GameObject with tag 'Player' not found! Make sure the tag is set correctly.");
            return;
        }

        if (enemyData == null)
        {
            Debug.LogError("EnemyData is missing! Assign an EnemyData scriptable object in the Inspector.");
        }

        agent.speed = chaseSpeed;
        playerHealth = player.GetComponent<PlayerStats>(); // Get reference to PlayerStats script
        if (playerHealth == null)
        {
            Debug.LogError("PlayerStats component not found on Player GameObject. Check if the script is attached.");
        }
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
        else
        {
            Idle();
        }
    }

    void Chase()
    {
        if (agent.isStopped)
            agent.isStopped = false;  // Ensure agent resumes movement

        agent.SetDestination(player.position);

        if (animator != null)
        {
            if (!animator.GetBool("isWalking"))
                animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false);
        }
    }

    void Attack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;

        Debug.Log("Attacking Player!"); // Debugging output

        agent.isStopped = true;

        // Stop moving while attacking
        if (animator != null)
        {
            animator.SetBool("isAttacking", true);
            animator.SetBool("isWalking", false);
        }

        // Now attack the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(enemyData.enemyAttack); 
                Debug.Log($"Player Health after attack: {playerHealth.currentHealth}");
                // Debug the player's health
            }
        }

        lastAttackTime = Time.time;
        StartCoroutine(ResetAttack());
    }

    void Idle()
    {
            agent.isStopped = true;
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", false);
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(attackCooldown);
        if (agent != null)
            agent.isStopped = false;

        if (animator != null)
            animator.SetBool("isAttacking", false);
    }
}

