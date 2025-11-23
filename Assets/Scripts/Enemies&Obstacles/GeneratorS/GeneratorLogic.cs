using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorLogic : MonoBehaviour
{
    [Header("Generator UI")]
    public GameObject repairAndGenerator;
    public Slider repairPercentage;
    public GameObject repairedOne;
    public Button closeGeneratorUI;

    [Header("Base Settings")]
    public GameObject partsNeeded, playerCursor;
    public float repairSpeed = 1f/ 10f;
    public float textDuration = 5f;

    [Header("Gen Sounds")]
    public AudioClip genFixing;
    public AudioClip genFixed;
    public AudioSource genFixingSource;

    bool inRange;
    public static bool isFixed;
    private bool isPlayingFixingSound;

    private FPController movement;
    

    private void Start()
    {
        movement = FindFirstObjectByType<FPController>();
        closeGeneratorUI.onClick.AddListener(CloseCodeUI);

        repairAndGenerator.SetActive(false);
        partsNeeded.SetActive(false);
        repairPercentage.gameObject.SetActive(false);

        if(genFixingSource != null)
        {
            genFixingSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (inRange)
        {
            if (CrateUI.partsCollected && !isFixed)
            {
                if (Input.GetMouseButton(0))
                {
                    if (movement != null)
                    {
                        movement.canMove = false;
                    }
                    repairPercentage.value += repairSpeed * Time.deltaTime;

                    GetComponent<Collider>().enabled = false;

                    if(!isPlayingFixingSound && genFixing != null)
                    {
                        genFixingSource.clip = genFixing;
                        genFixingSource.loop = true;
                        genFixingSource.Play();
                        isPlayingFixingSound = true;
                    }

                    if (repairPercentage.value >= repairPercentage.maxValue)
                    {

                        repairPercentage.value = repairPercentage.maxValue;
                        isFixed = true;

                        GeneratorCounter.Instance.AddGenerator();


                        if (genFixingSource.isPlaying)
                        {
                            genFixingSource.Stop();
                        }

                        if(genFixed != null)
                        {
                            genFixingSource.clip = genFixed;
                            genFixingSource.loop = true;
                            genFixingSource.spatialBlend = 1f;
                            genFixingSource.rolloffMode = AudioRolloffMode.Logarithmic;
                            genFixingSource.minDistance = 3f;
                            genFixingSource.maxDistance = 25f;
                            genFixingSource.dopplerLevel = 0f;
                            genFixingSource.Play();
                        }

                        isPlayingFixingSound = false;

                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;

                        

                        StartCoroutine(GeneratorRepairedOne());
                        
                    }
                }

                else
                {
                    if(isPlayingFixingSound)
                    {
                        genFixingSource.Stop();
                        isPlayingFixingSound = false;
                    }
                }
            }
            else if (!CrateUI.partsCollected)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    StartCoroutine(ShowPartsMessage());
                }
            }
        }
        else
        {
            if(isPlayingFixingSound)
            {
                genFixingSource.Stop();
                isPlayingFixingSound= false;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
         
            playerCursor.SetActive(false);
            inRange = true;
            repairAndGenerator.SetActive(true);
            repairPercentage.gameObject.SetActive(true);

            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
          
            playerCursor.SetActive(true);
            inRange = false;
            repairAndGenerator.SetActive(false);
            repairPercentage.gameObject.SetActive(false);
            partsNeeded.SetActive(false);

            if (isPlayingFixingSound)
            {
                genFixingSource.Stop();
                isPlayingFixingSound= false;
            }
        }
    }

    IEnumerator ShowPartsMessage()
    {
        repairAndGenerator.SetActive(false);
        partsNeeded.SetActive(true);
        yield return new WaitForSeconds(textDuration);
        partsNeeded.SetActive(false);
    }

    IEnumerator GeneratorRepairedOne()
    {
        repairedOne.SetActive(true);
        yield return new WaitForSeconds(textDuration);
        repairedOne.SetActive(false);
    }

    void CloseCodeUI()
    {
        CloseUI(repairAndGenerator);
    }

    void CloseUI(GameObject UI)
    {
        UI.SetActive(false);
        if (movement != null)
        {
            movement.canMove = true;
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}
