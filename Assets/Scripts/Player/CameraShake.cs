using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Animator))]
public class CameraShake : MonoBehaviour
{
    [SerializeField] private float shakeAmount = 0.02f;

    [SerializeField] private float shakeDuration = 3f;
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

        
        StartCoroutine(ShakeOnStartOnly());
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

    IEnumerator ShakeOnStartOnly()
    {
        isShaking = true;
        playerMovement.canMove = false;
        anim.enabled = false;
        
        yield return new WaitForSeconds(shakeDuration);
        isShaking = false;
        
        playerMovement.canMove=true;
        anim.enabled = true;
    }
}
