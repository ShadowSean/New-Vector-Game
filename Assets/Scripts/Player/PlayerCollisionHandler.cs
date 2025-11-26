using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.AI;

public class PlayerCollisionHandler : MonoBehaviour
{
    [Header("Game Over Settings")]
   
    public AudioSource jumpscareSource;   // audio source that will play the jumpscare sound
    public AudioClip jumpscareClip;       // assign your jumpscare audio file here

    public GameObject inventory;
    public GameObject staminaAndItem;
    public GameObject cameraObj;
    public GameObject scope;


    public GameObject Vector9;

    private bool gameOverTriggered = false;

    private void Start()
    {
        cameraObj.SetActive(false);
 
    }

    private void OnTriggerEnter(Collider other)
    {
        // This runs whenever the CharacterController bumps into something solid
        if (gameOverTriggered) return;

        if (other.gameObject.CompareTag("Enemy"))
        {
            Vector9Movement stun = other.GetComponent<Vector9Movement>();
            if(stun != null && stun.isStunned)
            {
                return;
            }
           
            gameOverTriggered = true;

            
            if (jumpscareSource && jumpscareClip)
            {
                jumpscareSource.PlayOneShot(jumpscareClip);
                cameraObj.SetActive(true);

                Animator jumpScare = Vector9.GetComponent<Animator>();
                jumpScare.speed = 0.3f;
                jumpScare.SetTrigger("Scare");
      
            }
            Vector9.GetComponent<NavMeshAgent>().isStopped = true;




            if (inventory) inventory.SetActive(false);
            if (staminaAndItem) staminaAndItem.SetActive(false);
            if (scope) scope.SetActive(false);

            if(stun != null)
            {
                stun.StartFade();
            }
        }
    }

    
}
