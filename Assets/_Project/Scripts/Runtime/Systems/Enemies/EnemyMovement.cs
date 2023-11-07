using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    private Transform playerTransform;

    public Transform hitBoxPos;
    public GameObject hitboxPrefab;

    public float delayAttack;
    public float distanceToAttack;
    public float distanceToView;

    private bool canAttack;
    private bool isDead;

    public LayerMask playerMask;

    [Range(1, 100)] public float walkRadius;

    private Collider[] view;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();

        playerMask = LayerMask.GetMask("Player");

    }

    private void Update()
    {
        Patrol();
        FindPlayer();
    }

    private void CheckDistanceToPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= distanceToAttack && !canAttack)
        {
            canAttack = true;
            StartCoroutine(AttackPlayer());
        }
    }

    private void Patrol()
    {
        if (agent != null && agent.remainingDistance <= agent.stoppingDistance && !canAttack)
        {
            agent.SetDestination(RandomNavMeshLocation());
        }
    }

    private IEnumerator AttackPlayer()
    {
        print("CHAMEI");

        animator.SetTrigger("attack");

        yield return new WaitForSeconds(0.2f);

        Instantiate(hitboxPrefab, hitBoxPos.position, transform.localRotation);

        yield return new WaitForSeconds(delayAttack);

        canAttack = false;
    }

    public void TriggerDead()
    {
        isDead = true;
        agent.speed = 0f;
        animator.SetTrigger("dead");
        GetComponent<CapsuleCollider>().enabled = false;    
    }

    private Vector3 RandomNavMeshLocation()
    {
        agent.speed = 1;
        Vector3 finalPosistion = Vector3.zero;
        Vector3 randomPosition = Random.insideUnitSphere * walkRadius;
        randomPosition += transform.position;

        if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, walkRadius, 1))
        {
            finalPosistion = hit.position;
        }

        return finalPosistion;
    }

    private void FindPlayer()
    {
        view = Physics.OverlapSphere(transform.position, distanceToView, playerMask);

        if (view.Length > 0)
        {
            agent.SetDestination(view[0].transform.position);
            CheckDistanceToPlayer();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, distanceToView);
        Gizmos.DrawWireSphere(transform.position, distanceToAttack);
        Gizmos.DrawWireSphere(transform.position, walkRadius);
    }


}
