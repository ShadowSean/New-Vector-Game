using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Animator))]
public class CameraShake : MonoBehaviour
{
    [SerializeField] private float shakeAmount = 0.02f;

    [SerializeField] private float shakeDuration = 3f;

    public GameObject typingText;
    public GameObject sprintUI;
    public GameObject sprintText;
    public GameObject inventoryUI;
    public GameObject inventoryText;

    private Animator anim;

    private Vector3 initalPos;
    bool isShaking;

    private FPController playerMovement;

   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        initalPos = transform.localPosition;
    }

    private void Start()
    {
        playerMovement = FindFirstObjectByType<FPController>();
        anim = gameObject.GetComponent<Animator>();

        
        StartCoroutine(Tutorial());
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isShaking)
        {
            transform.localPosition = initalPos + Random.insideUnitSphere * shakeAmount;
        }
        else
        {
            transform.localPosition = initalPos;
        }
    }

    IEnumerator Tutorial()
    {
        isShaking = true;
        playerMovement.canMove = false;
        anim.enabled = false;
        
        yield return new WaitForSeconds(shakeDuration);
        isShaking = false;
        
        playerMovement.canMove=true;
        anim.enabled = true;

        typingText.SetActive(true);
        yield return new WaitForSeconds(5f);
        typingText.SetActive(false);

        playerMovement.canMove = false;
        anim.enabled = false;

        sprintUI.SetActive(true);
        sprintText.SetActive(true);
        yield return new WaitForSeconds(5f);
        
        sprintText.SetActive(false);

        

        inventoryUI.SetActive(true);
        inventoryText.SetActive(true);
        yield return new WaitForSeconds(8f);
        inventoryText.SetActive(false);

        playerMovement.canMove = true;
        anim.enabled = true;


    }

   
}
