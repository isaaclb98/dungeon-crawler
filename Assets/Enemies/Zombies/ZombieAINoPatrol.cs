using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAINoPatrol : MonoBehaviour
{
    public float chaseSpeed = 2f;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 2f;

    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private EnemyHealth enemyHealth;
    private bool isAttacking = false;
    private float lastAttackTime = 0f;
    private bool isDead = false;

    public EnemyData enemyData;
    private PlayerStats _playerStats;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        _playerStats = PlayerStats.Instance;

        agent.speed = chaseSpeed;
        agent.updateRotation = true; // Let NavMeshAgent handle rotation

        agent.autoBraking = false;
        agent.angularSpeed = 120f;
        agent.acceleration = 8f;
        agent.avoidancePriority = 50;

        animator.SetBool("isWalking", false);
    }

    void Update()
    {
        if (!player || isDead) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange && HasLineOfSight())
        {
            if (distanceToPlayer <= attackRange)
            {
                Attack();
            }
            else
            {
                ChasePlayer();
            }
        }
        else
        {
            Idle();
        }

        PreventSliding();
    }

    void ChasePlayer()
    {
        if (isAttacking || isDead) return;

        agent.isStopped = false;

        if (!agent.pathPending && Vector3.Distance(agent.destination, player.position) > 1f)
        {
            agent.SetDestination(player.position);
        }

        bool isMoving = agent.velocity.magnitude > 0.1f;
        animator.SetBool("isWalking", isMoving);
        animator.SetBool("isAttacking", false);
    }

    void Attack()
    {
        if (Time.time - lastAttackTime < attackCooldown || isDead) return;

        isAttacking = true;
        agent.isStopped = true;

        animator.SetBool("isAttacking", true);
        animator.SetBool("isWalking", false);
        
        // Check if the player is within attack range
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)
        {
            
            if (_playerStats != null)
            {
                // Use the enemy's attack value from the EnemyData ScriptableObject
                _playerStats.TakeDamage(enemyData.enemyAttack);  // Apply damage based on enemyAttack
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

        // **Fix sudden movement after attack** - Resume smoothly
        yield return new WaitForSeconds(0.1f);
        if (!isDead && !isAttacking) agent.isStopped = false;
    }

    void Idle()
    {
        if (!isAttacking && !isDead)
        {
            agent.isStopped = true;
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", false);
        }
    }

    bool HasLineOfSight()
    {
        RaycastHit hit;
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        if (Physics.Raycast(transform.position + Vector3.up, directionToPlayer, out hit, detectionRange))
        {
            return hit.transform.CompareTag("Player");
        }

        return false;
    }

    void PreventSliding()
    {
        //  **Fix agent sliding issue** - Stop when not moving
        if (agent.remainingDistance <= agent.stoppingDistance && !isAttacking)
        {
            agent.isStopped = true;
            animator.SetBool("isWalking", false);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        enemyHealth.TakeDamage(damage);
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        animator.SetTrigger("Die");
        agent.isStopped = true;
        Destroy(gameObject, 2f);
    }
}
