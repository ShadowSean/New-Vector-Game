using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Vector9Movement : MonoBehaviour
{
    [SerializeField] private Transform playerPosition;
    [SerializeField] private Transform[] patrolAreas;

    public Animator animator;
    //[SerializeField] private CanvasGroup gameOverCanvas;
    //[SerializeField] private AudioSource jumpscareSource;
    //[SerializeField] private AudioClip jumpscareClip;

    //public GameObject inventory,staminaAndItem,scope;

   

    [SerializeField] float waitTime = 2f;
    [SerializeField] float vectorPatrolSpeed = 2f;
    [SerializeField] float vectorChaseSpeed = 10f;
    //[SerializeField] float fadeDuration = 2f;

    //[SerializeField] float attackRange = 1f;
    
    int currentPatrolIndex = 0;
    bool isPlayerInRange;
    bool waiting;
    //bool gameOverTriggered;
    
    private NavMeshAgent agent;

    //[SerializeField]private Collider triggerCollider;
    //[SerializeField]private Collider solidCollider;



    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        


        //if(triggerCollider) triggerCollider.enabled = true;
        //if(solidCollider) solidCollider.enabled = false;
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
        //if (gameOverTriggered) return;
        if (!isPlayerInRange)
        {
            Patrol();
        }
        //else
        //{
        //    ChasePlayer();
        //}

        //float distance = Vector3.Distance(transform.position,playerPosition.position);

        //if (distance <= attackRange)
        //{
        //    if (triggerCollider) triggerCollider.enabled = false;
        //    if (solidCollider) solidCollider.enabled = true;
        //}
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

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (!gameOverTriggered && other.CompareTag("Player"))
    //    {
    //        gameOverTriggered = true;
    //        StartCoroutine(GameOverSequence());
    //    }
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (gameOverTriggered) return;

    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        gameOverTriggered = true;
    //        agent.isStopped = true;

    //        if (jumpscareSource && jumpscareClip)
    //        {
    //            jumpscareSource.PlayOneShot(jumpscareClip);
    //        }
    //        StartCoroutine(GameOverSequence());
    //    }
    //}


    //IEnumerator GameOverSequence()
    //{
    //    agent.isStopped = true;
    //    inventory.SetActive(false);
    //    staminaAndItem.SetActive(false);
    //    scope.SetActive(false);
    //    //animator.SetTrigger("AttackPlayer");
    //    float t = 0f;
    //    while (t < fadeDuration)
    //    {
    //        t += Time.deltaTime;
    //        if (gameOverCanvas)
    //        {
    //            gameOverCanvas.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
    //        }

    //        yield return null;
    //    }

    //    yield return new WaitForSeconds(4f);
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    //}

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
    //void ChasePlayer()
    //{
    //    if (playerPosition)
    //    {
    //        agent.destination = playerPosition.position;
    //    }
    //}
}
