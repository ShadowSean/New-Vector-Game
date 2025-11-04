using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GeneratorThree : MonoBehaviour
{
    [Header("Generator UI")]
    public GameObject repairAndGenerator;
    public Slider repairPercentage;
    public GameObject repairedThree;
    public Button closeGeneratorUI;

    [Header("Base Settings")]
    public GameObject partsNeeded, playerCursor;
    public float repairSpeed = 0.5f;
    public float textDuration = 5f;

    bool inRange;
    public static bool isThirdFixed;

    private FPController movement;


    private void Start()
    {
        movement = FindFirstObjectByType<FPController>();
        closeGeneratorUI.onClick.AddListener(CloseCodeUI);

        repairAndGenerator.SetActive(false);
        partsNeeded.SetActive(false);
        repairPercentage.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (inRange)
        {
            if (CrateThreeUI.partsCollectedThree && !isThirdFixed)
            {
                if (Input.GetMouseButton(0))
                {
                    repairPercentage.value += repairSpeed * Time.deltaTime;
                    GetComponent<Collider>().enabled = false;


                    if (repairPercentage.value >= repairPercentage.maxValue)
                    {
                        repairPercentage.value = repairPercentage.maxValue;
                        isThirdFixed = true;

                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;

                        StartCoroutine(GeneratorRepairedThree());

                    }
                }
            }
            else if (!CrateThreeUI.partsCollectedThree)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    StartCoroutine(ShowPartsMessageThree());
                }
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
        }
    }

    IEnumerator ShowPartsMessageThree()
    {
        repairAndGenerator.SetActive(false);
        partsNeeded.SetActive(true);
        yield return new WaitForSeconds(textDuration);
        partsNeeded.SetActive(false);
    }

    IEnumerator GeneratorRepairedThree()
    {
        repairedThree.SetActive(true);
        yield return new WaitForSeconds(textDuration);
        repairedThree.SetActive(false);
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
