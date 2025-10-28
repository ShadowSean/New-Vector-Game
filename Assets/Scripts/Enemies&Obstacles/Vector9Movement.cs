using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Vector9Movement : MonoBehaviour
{
    [SerializeField] private Transform playerPosition;
    [SerializeField] private Transform[] patrolAreas;

    public Animator animator;

    [SerializeField] float waitTime = 2f;
    [SerializeField] float vectorPatrolSpeed = 2f;
    [SerializeField] float vectorChaseSpeed = 10f;
    
    int currentPatrolIndex = 0;
    bool isPlayerInRange;
    bool waiting;
    
    private NavMeshAgent agent;



    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if (patrolAreas.Length > 0)
        {
            agent.speed = vectorPatrolSpeed;
            agent.destination = patrolAreas[currentPatrolIndex].position;
        }
    }

    private void Update()
    {
        if (!isPlayerInRange)
        {
            Patrol();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            animator.speed = 5f;
            agent.speed = vectorChaseSpeed;
            agent.destination = playerPosition.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.speed = 1f;
            isPlayerInRange = true;
            agent.speed = vectorPatrolSpeed;

            if(patrolAreas.Length > 0)
            {
                agent.destination = patrolAreas[currentPatrolIndex].position;
            }
           
        }
    }

    void Patrol()
    {
        if (patrolAreas.Length == 0 || waiting)
        {
            return;
        }

        if (!agent.pathPending && agent.remainingDistance < 0.3)
        {
            StartCoroutine(WaitEachPoint());
        }
    }

    IEnumerator WaitEachPoint()
    {
        waiting = true;
        yield return new WaitForSeconds(waitTime);

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolAreas.Length;
        agent.destination = patrolAreas[currentPatrolIndex].position;
        waiting = false;
    }

}
