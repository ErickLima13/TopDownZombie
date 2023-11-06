using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent nav;
    private Animator animator;

    private Transform playerTransform;

    public Transform hitBoxPos;
    public GameObject hitboxPrefab;

    public float delayAttack;
    public float distanceToAttack;

    public bool canAttack;

    private bool isDead;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponentInChildren<Animator>();
        nav = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (playerTransform != null && !isDead)
        {
            nav.destination = playerTransform.position;

            float distanceToPlayer = Vector3.Distance(transform.position, nav.destination);

            if (distanceToPlayer <= distanceToAttack && !canAttack)
            {
                canAttack = true;
                StartCoroutine(AttackPlayer());
            }
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
        animator.SetTrigger("dead");
    }

}
