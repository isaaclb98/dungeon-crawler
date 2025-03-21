using UnityEngine;
using UnityEngine.AI;

public class LizardAI : MonoBehaviour
{
    public Animator animator;
    public NavMeshAgent agent;
    public Transform player;
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float attackRange = 1.5f;
    public float detectionRange = 10f;
    public float attackCooldown = 2f;
    private float lastAttackTime;

    public EnemyData enemyData;
    private EnemyHealth enemyHealth;
    private bool isAttacking;

    public float wanderRadius = 20f; // The radius within which the lizard will wander
    public float wanderTime = 5f; // Time interval between wandering to a new point

    private float wanderTimer;

    void Start()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!agent) agent = GetComponent<NavMeshAgent>();
        enemyHealth = GetComponent<EnemyHealth>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (!player) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            Attack();
        }
        else if (distanceToPlayer <= detectionRange)
        {
            ChasePlayer();
        }
        else
        {
            Idle();
        }
    }

    void ChasePlayer()
    {
        if (isAttacking) return;

        agent.speed = runSpeed;

        RaycastHit hit;
        Vector3 direction = (player.position - transform.position).normalized;
        Debug.DrawRay(transform.position, direction * detectionRange, Color.red);

        if (Physics.Raycast(transform.position, (player.position - transform.position).normalized, out hit, detectionRange))
        {
            if (hit.collider.CompareTag("Player"))
            {
                // No obstacle, continue chasing
                agent.SetDestination(player.position);  // Set the destination to player's position
                agent.isStopped = false;
                animator.SetBool("isWalking", false);  // Stop walking animation
                animator.SetBool("isRunning", true);   // Play running animation
                animator.SetBool("isAttacking", false); // Stop attacking animation
            }
            else
            {
                // An obstacle is detected in the way, stop chasing
                agent.isStopped = true;
                animator.SetBool("isWalking", false);  // Stop walking animation
                animator.SetBool("isRunning", false);  // Stop running animation
                animator.SetBool("isAttacking", false); // Stop attacking animation
            }
        }
        Debug.Log("Distance to Player: " + Vector3.Distance(transform.position, player.position));
    }

    void Attack()
    {
        if (isAttacking || Time.time - lastAttackTime < attackCooldown) return;

        isAttacking = true;
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isAttacking", true);

        agent.isStopped = true;

        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);

        Debug.DrawRay(transform.position, direction * attackRange, Color.green);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (player.position - transform.position).normalized, out hit, attackRange))
        {
            if (hit.collider.CompareTag("Player"))
            {
                // If the ray hits the player, deal damage
                PlayerHealthAndStamina playerHealth = player.GetComponent<PlayerHealthAndStamina>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(enemyData.enemyAttack);
                }
            }
        }

        lastAttackTime = Time.time;
        Invoke("ResetAttack", 1f);

    }

    void ResetAttack()
    {
        isAttacking = false;
        agent.isStopped = false;

        Debug.Log("Attack finished, checking if player is within detection range");

        if (Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            Debug.Log("Player is within range, chasing...");
            ChasePlayer();  // Continue chasing the player
        }
        else
        {
            Debug.Log("Player out of range, idling...");
            Idle(); // If player is out of range, go idle
        }

        animator.SetBool("isAttacking", false);
    }

    void Wander()
    {
        // If the wander timer has elapsed, pick a new random position
        if (wanderTimer <= 0)
        {
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection += transform.position; // Set the random point relative to the lizard's current position

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }

            wanderTimer = wanderTime; // Reset the wander timer
        }
        else
        {
            wanderTimer -= Time.deltaTime; // Decrease the timer
        }

        animator.SetBool("isWalking", true);
        animator.SetBool("isRunning", false);
        animator.SetBool("isAttacking", false);
    }

    void Idle()
    {
        agent.ResetPath();
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isAttacking", false);

        agent.isStopped = true;

        Debug.Log("Going idle, resetting path and stopping agent");

        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    public void TakeDamage(int damage)
    {
        enemyHealth.TakeDamage(damage);
        if (enemyHealth.enemyData.enemyHealth <= 0)
        {
            animator.SetTrigger("Die");
            agent.isStopped = true;
            this.enabled = false;
        }
    }
}