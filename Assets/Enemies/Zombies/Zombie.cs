using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
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
    private float lastAttackTime = 0f;
    private int currentHealth;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent.speed = patrolSpeed;
        currentHealth = maxHealth;
        MoveToNextPatrolPoint();
    }

    void Update()
    {
        if (player == null) return;

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
            Patrol();
        }
    }

    void ChasePlayer()
    {
        agent.speed = chaseSpeed;
        agent.destination = player.position;
        animator.SetBool("isWalking", true);
        animator.SetBool("isAttacking", false);
    }

    void Patrol()
    {
        if (isAttacking) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            MoveToNextPatrolPoint();
        }

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

        isAttacking = true;
        agent.isStopped = true;

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

