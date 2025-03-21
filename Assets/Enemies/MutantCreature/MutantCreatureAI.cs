using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MutantCreatureAI : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    public int maxHealth = 100;

    private int currentPatrolIndex = 0;
    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private bool isAttacking = false;
    private bool isSniffing = false;
    private float lastAttackTime = 0f;
    private int currentHealth;

    public EnemyData enemyData;
    private PlayerHealthAndStamina playerHealth;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.enabled = true; // Ensure the Animator is active
        player = GameObject.FindGameObjectWithTag("Player").transform;

        agent.speed = patrolSpeed;
        currentHealth = maxHealth;
        MoveToNextPatrolPoint();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Attack range check
        if (distanceToPlayer <= attackRange)
        {
            // Debug.Log("Attacking Player");
            Attack();
        }
        // Sniff or chase behavior when the player is within detection range
        else if (distanceToPlayer <= detectionRange)
        {
            if (!isSniffing)
            {
                // Debug.Log("Player Detected, Sniffing or Chasing");
                Sniff();
            }
            else
            {
                // If already sniffing, start chasing the player
                agent.speed = chaseSpeed;  // Change to chase speed while sniffing
                agent.destination = player.position;  // Move towards the player
                animator.SetBool("isWalking", true);
                animator.SetBool("isAttacking", false);  // Ensure the monster is walking and not attacking
            }
        }
        // No player in range, continue patrolling
        else
        {
            Debug.Log("Patrolling");
            Patrol();
        }

     Debug.Log($"Animator States => Walking: {animator.GetBool("isWalking")}, Attacking: {animator.GetBool("isAttacking")}, Sniffing: {isSniffing}");
    }

    void Sniff()
    {
        if (!isSniffing)
        {
            isSniffing = true;
            agent.isStopped = true;
            animator.SetBool("isWalking", false); // Ensure walking animation stops
            animator.ResetTrigger("Sniff");
            animator.SetTrigger("Sniff");
            StartCoroutine(ResumePatrolAfterSniff());
        }
    }

    IEnumerator ResumePatrolAfterSniff()
    {
        yield return new WaitForSeconds(2f);

        isSniffing = false;
        agent.isStopped = false;
        animator.ResetTrigger("Sniff");

        // Check if the player is still within attack range and attack immediately
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)
        {
            Attack();
        }
    }

    void Patrol()
    {
        if (isSniffing || isAttacking) return;

        // Debug.Log("Patrolling... Remaining Distance: " + agent.remainingDistance);

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            MoveToNextPatrolPoint();
        }

        // Debug.Log("Setting isWalking to true for Patrol");
        bool shouldWalk = agent.velocity.magnitude > 0.1f;
        animator.SetBool("isWalking", shouldWalk);
    }

    void MoveToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;
        agent.destination = patrolPoints[currentPatrolIndex].position;
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    void Attack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;

        Debug.Log("Attacking Player!"); // Debugging output
        isAttacking = true;
        isSniffing = false;  // Stop sniffing
        agent.isStopped = true;

        // Reset Sniff trigger to prevent looping
        animator.ResetTrigger("Sniff");
        animator.SetBool("isAttacking", true);
        animator.SetBool("isWalking", false);

        // Now attack the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)
        {
            PlayerHealthAndStamina playerHealth = player.GetComponent<PlayerHealthAndStamina>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(enemyData.enemyAttack); 
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

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetTrigger("Die");
        agent.isStopped = true;
        Destroy(gameObject, 2f);
    }
}