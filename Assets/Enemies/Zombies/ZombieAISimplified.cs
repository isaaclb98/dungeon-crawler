using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAISimplified : MonoBehaviour
{
    public float chaseSpeed = 2f;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 2f;

    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private EnemyHealth enemyHealth;
    private float lastAttackTime = 0f;
    private bool isDead = false;

    public EnemyData enemyData;
    private PlayerStats playerStats;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        playerStats = PlayerStats.Instance;

        agent.speed = chaseSpeed;
        agent.updateRotation = true;

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
                Chase();
            }
        }
        else
        {
            Idle();
        }
    }

    void Chase()
    {
        agent.destination = player.position;
        animator.SetBool("isWalking", true);
        animator.SetBool("isAttacking", false);
    }

    void Attack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;

        Debug.Log("Attacking Player!");

        agent.isStopped = true;
        animator.SetBool("isAttacking", true);
        animator.SetBool("isWalking", false);
        
        // Check if the player is within attack range
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)
        {
            
            if (playerStats != null)
            {
                playerStats.TakeDamage(enemyData.enemyAttack);  // Apply damage based on enemyAttack
                Debug.Log($"Player Health after attack: {playerStats.currentHealth}");
            }
        }

        lastAttackTime = Time.time;
        StartCoroutine(ResetAttack());
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(1f);
        agent.isStopped = false;
    }

    void Idle()
    {
            agent.isStopped = true;
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", false);
    }

    bool HasLineOfSight()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        if (Physics.Raycast(transform.position + Vector3.up, directionToPlayer, out RaycastHit hit, detectionRange))
        {
            return hit.transform.CompareTag("Player");
        }

        return false;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        enemyHealth.TakeDamage(damage);
    }
}
