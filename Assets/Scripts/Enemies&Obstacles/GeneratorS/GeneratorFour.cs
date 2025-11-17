using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GeneratorFour : MonoBehaviour
{
    [Header("Generator UI")]
    public GameObject repairAndGenerator;
    public Slider repairPercentage;
    public GameObject repairedFour;
    public Button closeGeneratorUI;

    [Header("Base Settings")]
    public GameObject partsNeeded, playerCursor;
    public float repairSpeed = 0.5f;
    public float textDuration = 5f;

    [Header("Gen Sounds")]
    public AudioClip genFixing;
    public AudioClip genFixed;
    public AudioSource genFixingSource;

    bool inRange;
    public static bool isFourthFixed;
    private bool isPlayingFixingSound;

    private FPController movement;



    private void Start()
    {

        movement = FindFirstObjectByType<FPController>();
        closeGeneratorUI.onClick.AddListener(CloseCodeUI);

        repairAndGenerator.SetActive(false);
        partsNeeded.SetActive(false);
        repairPercentage.gameObject.SetActive(false);

        if (genFixingSource != null)
        {
            genFixingSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (inRange)
        {
            if (CrateFourUI.partsCollectedFour && !isFourthFixed)
            {
                if (Input.GetMouseButton(0))
                {
                    if (movement != null)
                    {
                        movement.canMove = false;
                    }
                    repairPercentage.value += repairSpeed * Time.deltaTime;

                    GetComponent<Collider>().enabled = false;

                    if (!isPlayingFixingSound && genFixing != null)
                    {
                        genFixingSource.clip = genFixing;
                        genFixingSource.loop = true;
                        genFixingSource.Play();
                        isPlayingFixingSound = true;
                    }

                    if (repairPercentage.value >= repairPercentage.maxValue)
                    {

                        repairPercentage.value = repairPercentage.maxValue;
                        isFourthFixed = true;

                        if (genFixingSource.isPlaying)
                        {
                            genFixingSource.Stop();
                        }

                        if (genFixed != null)
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



                        StartCoroutine(GeneratorRepairedFour());

                    }
                }

                else
                {
                    if (isPlayingFixingSound)
                    {
                        genFixingSource.Stop();
                        isPlayingFixingSound = false;
                    }
                }
            }
            else if (!CrateFourUI.partsCollectedFour)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    StartCoroutine(ShowPartsMessageFour());
                }
            }
        }

        else
        {
            if (isPlayingFixingSound)
            {
                genFixingSource.Stop();
                isPlayingFixingSound = false;
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
                isPlayingFixingSound = false;
            }
        }
    }

    IEnumerator ShowPartsMessageFour()
    {
        repairAndGenerator.SetActive(false);
        partsNeeded.SetActive(true);
        yield return new WaitForSeconds(textDuration);
        partsNeeded.SetActive(false);
    }

    IEnumerator GeneratorRepairedFour()
    {
        repairedFour.SetActive(true);
        yield return new WaitForSeconds(textDuration);
        repairedFour.SetActive(false);
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
