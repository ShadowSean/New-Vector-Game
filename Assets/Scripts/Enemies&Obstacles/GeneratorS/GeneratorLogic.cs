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
    public float repairSpeed = 0.5f;
    public float textDuration = 5f;

    bool inRange;
    public static bool isFixed;

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

                    if (repairPercentage.value >= repairPercentage.maxValue)
                    {

                        repairPercentage.value = repairPercentage.maxValue;
                        isFixed = true;

                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;

                        

                        StartCoroutine(GeneratorRepairedOne());
                        
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
