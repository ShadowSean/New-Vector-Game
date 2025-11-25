using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.AI;

public class PlayerCollisionHandler : MonoBehaviour
{
    [Header("Game Over Settings")]
    public CanvasGroup gameOverCanvas;    // drag your GameOverCanvas (with CanvasGroup)
    public float fadeDuration = 2f;       // how fast the fade happens
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
           
            gameOverTriggered = true;

            // Stop player movement if you have movement logic here
            // (optional depending on your controller script)

            // Play jumpscare sound
            if (jumpscareSource && jumpscareClip)
            {
                jumpscareSource.PlayOneShot(jumpscareClip);
                cameraObj.SetActive(true);
                Vector9.GetComponent<Animator>().SetTrigger("Scare");
      
            }
            Vector9.GetComponent<NavMeshAgent>().isStopped = true;




            if (inventory) inventory.SetActive(false);
            if (staminaAndItem) staminaAndItem.SetActive(false);
            if (scope) scope.SetActive(false);
        }
    }

    private IEnumerator GameOverSequence()
    {
        // Disable HUD/UI
       

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            if (gameOverCanvas)
                gameOverCanvas.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
