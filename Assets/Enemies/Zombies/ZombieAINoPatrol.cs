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

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>(); // Get the health script
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent.speed = chaseSpeed;
        agent.updateRotation = true; // Let NavMeshAgent handle rotation

        animator.SetBool("isWalking", false);
    }

    void Update()
    {
        if (player == null || isDead) return;

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
    }

    void ChasePlayer()
    {
        if (isAttacking || isDead) return;

        agent.isStopped = false;

        if (Vector3.Distance(agent.destination, player.position) > 1f)
        {
            agent.destination = player.position;
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
        agent.velocity = Vector3.zero;

        animator.SetBool("isAttacking", true);
        animator.SetBool("isWalking", false);

        lastAttackTime = Time.time;
        StartCoroutine(ResetAttack());
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        agent.isStopped = false;
    }

    void Idle()
    {
        if (!isAttacking && !isDead)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
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
    }
}