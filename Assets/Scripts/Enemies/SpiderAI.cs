using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderAI : MonoBehaviour
{
    public Transform player;           // Player's Transform
    public float detectionRange = 10f; // Range within which the spider can detect the player
    public float attackRange = 2f;     // Range within which the spider attacks
    public float health = 100f;        // Spider's health
    public LayerMask obstacleMask;     // LayerMask for objects that can block vision
    public float attackCooldown = 2f;
    public Transform[] patrolPoints;
    public float patrolWaitTime = 2f;

    private NavMeshAgent agent;
    private Animator animator;
    private bool canSeePlayer = false;
    private bool isAttacking = false;
    private float lastAttackTime = 0f;
    private int currentPatrolIndex = 0;
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.Warp(transform.position);

        if (patrolPoints.Length > 0)
        {
            StartCoroutine(Patrol());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        canSeePlayer = HasLineOfSight();

        if (canSeePlayer && distanceToPlayer <= detectionRange)
        {
            StopCoroutine(Patrol());
            agent.isStopped = false;  // ? Ensure the agent is not stopped
            agent.SetDestination(player.position);
            animator.SetBool("isWalking", true);

            if (distanceToPlayer <= attackRange)
            {
                if (!isAttacking && Time.time >= lastAttackTime + attackCooldown)
                {
                    agent.isStopped = true;  // Stop moving to attack
                    PlayRandomAttackAnimation();
                    isAttacking = true;
                    lastAttackTime = Time.time;
                    StartCoroutine(ResetAttack());
                }
            }
            else
            {
                isAttacking = false;
                agent.isStopped = false;
            }
        }
            else if (!isAttacking)
            {
                animator.SetBool("isWalking", false);
                agent.isStopped = true;
            }
    }

    private void PlayRandomAttackAnimation()
    {
        int attackIndex = Random.Range(0, 2); // Randomly pick between 0 and 1
        if (attackIndex == 0)
        {
            animator.SetTrigger("Attack1");
        }
        else
        {
            animator.SetTrigger("Attack2");
        }
    }

    IEnumerator Patrol()
    {
        while (!canSeePlayer && !isDead)
        {
            if (patrolPoints.Length > 0)
            {
                agent.SetDestination(patrolPoints[currentPatrolIndex].position);
                animator.SetBool("isWalking", true);

                while (agent.pathPending || agent.remainingDistance > 0.5f)
                {
                    yield return null;
                }

                animator.SetBool("isWalking", false);
                yield return new WaitForSeconds(patrolWaitTime);
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            }
            else if (!canSeePlayer && !isAttacking && !agent.pathPending && agent.remainingDistance < 0.5f)
            {
                // Only restart the patrol if the spider has finished moving and the player isn't detected
                StartCoroutine(Patrol());
            }
            yield return null;
        }
    }

    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        agent.isStopped = false;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        animator.SetTrigger("Die");
        agent.isStopped = true;
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 5f);
    }

    private bool HasLineOfSight()
    {
        RaycastHit hit;
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        if (Physics.Raycast(transform.position + Vector3.up * 1.0f, directionToPlayer, out hit, detectionRange))
        {
            Debug.DrawLine(transform.position + Vector3.up * 1.0f, hit.point, Color.red);
            return hit.transform == player; // Return true if the player is hit
        }

        return false; // Return false if no line of sight
    }

    private void OnDrawGizmos()
    {
        // Visualize the detection range and raycast in the Scene view
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.blue;
        if (player != null)
        {
            Gizmos.DrawLine(transform.position + Vector3.up * 1.0f, player.position);
        }
    }
}
