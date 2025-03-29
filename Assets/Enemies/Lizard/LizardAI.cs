using UnityEngine;
using UnityEngine.AI;

public class LizardAI : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float attackRange = 1.5f;
    public float detectionRange = 10f;
    public float attackCooldown = 2f;

    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private bool isAttacking;
    private float lastAttackTime = 0f;

    private EnemyHealth enemyHealth;
    public EnemyData enemyData;
    private PlayerStats playerStats;


    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        enemyHealth = GetComponent<EnemyHealth>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        playerStats = PlayerStats.Instance;

        if (player == null)
        {
            Debug.LogError("Player GameObject with tag 'Player' not found! Make sure the tag is set correctly.");
            return;
        }

    }

    void Update()
    {
        if (!player) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange && HasLineOfSight())
        {
            Attack();
        }
        else if (distanceToPlayer <= detectionRange && HasLineOfSight())
        {
            Chase();
        }
        else
        {
            Idle();
        }
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

    void Chase()
    {
        if (isAttacking) return;

        agent.speed = runSpeed;

        if (agent.remainingDistance > 0.5f || agent.pathPending)
        {
            agent.SetDestination(player.position);
        }

        agent.isStopped = false;

        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", true);
        animator.SetBool("isAttacking", false);
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
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)), Time.deltaTime * 500f);

        if (HasLineOfSight() && Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            PlayerStats playerHealth = player.GetComponent<PlayerStats>();
            playerHealth?.TakeDamage(enemyData.enemyAttack);
        }

        lastAttackTime = Time.time;
        Invoke("ResetAttack", 1f);
    }

    void ResetAttack()
    {
        isAttacking = false;
        agent.isStopped = false;
        animator.SetBool("isAttacking", false);

        if (Vector3.Distance(transform.position, player.position) <= detectionRange && HasLineOfSight())
        {
            Chase();
        }
        else
        {
            Idle();
        }
    }

    void Idle()
    {
        agent.ResetPath();
        agent.isStopped = true;
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isAttacking", false);
    }

    public void TakeDamage(int damage)
    {
        enemyHealth.TakeDamage(damage);
    }
}
