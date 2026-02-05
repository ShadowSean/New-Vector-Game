using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Vector9Movement : MonoBehaviour
{
    [SerializeField] private Transform playerPosition;
    public Transform[] patrolAreas;
    public float chaseDistance;
    public CanvasGroup gameOverCanvas;    
    public float fadeDuration = 2f;

    public GameObject scope;

    public Animator animator;
    //[SerializeField] private CanvasGroup gameOverCanvas;
    //[SerializeField] private AudioSource jumpscareSource;
    //[SerializeField] private AudioClip jumpscareClip;

    //public GameObject inventory,staminaAndItem,scope;

   

    public float waitTime = 2f;
    public float vectorPatrolSpeed = 2f;
    public float vectorChaseSpeed = 10f;
    //[SerializeField] float fadeDuration = 2f;

    //[SerializeField] float attackRange = 1f;
    public bool isStunned;
    public float stunRange = 10f;
    int currentPatrolIndex = 0;
    bool isPlayerInRange;
    bool waiting;
    //bool gameOverTriggered;
    
    public NavMeshAgent agent;

    public GameObject stunIcon;

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
        if (isStunned)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            return;   // <-- prevents chase/patrol logic from running
        }
        float dist = Vector3.Distance(transform.position, playerPosition.position);
        if (scope != null)
        {
            if (dist <= stunRange)
                scope.SetActive(false);   
            else
                scope.SetActive(true);    
        }
        stunIcon.SetActive(dist <= stunRange);
        if (dist <= chaseDistance)
        {
            isPlayerInRange = true;
            agent.speed = vectorChaseSpeed;
            agent.angularSpeed = 800f;
            agent.acceleration = 20f;
            if(dist >= 5f)
            {
                animator.speed = 5f;
            }

            agent.destination = playerPosition.position;
        }
        else 
        {
            
            animator.speed = 1f;
            agent.speed = vectorPatrolSpeed;
            agent.angularSpeed = 120f;
            agent.acceleration = 15f;

            if (patrolAreas.Length > 0)
            {
                agent.destination = patrolAreas[currentPatrolIndex].position;
            }
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



    private IEnumerator GameOverSequence()
    {
        yield return new WaitForSeconds(2.5f);

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            if (gameOverCanvas)
                gameOverCanvas.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }
        print("bye");
        yield return new WaitForSeconds(3f);
        print("Hi");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        yield break;
    }

    public void StartFade()
    {
        StartCoroutine(GameOverSequence());
    }

    public void Stun()
    {
        
        isStunned = true;
        animator.SetTrigger("Stun");
        
        agent.isStopped = true;
        
        StartCoroutine(Recover());
    }

    IEnumerator Recover()
    {
        yield return new WaitForSeconds(10f);
        isStunned = false;
        agent.isStopped = false;
        animator.ResetTrigger("Stun");

        animator.Play("Walking", 0);
    }
}
