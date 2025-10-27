using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GeneratorSix : MonoBehaviour
{
    [Header("Generator UI")]
    public GameObject repairAndGenerator;
    public Slider repairPercentage;
    public GameObject repairedSix;

    [Header("Base Settings")]
    public GameObject partsNeeded, playerCursor;
    public float repairSpeed = 0.5f;
    public float textDuration = 5f;

    bool inRange;
    public static bool isSixthFixed;


    private void Start()
    {
        repairAndGenerator.SetActive(false);
        partsNeeded.SetActive(false);
        repairPercentage.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (inRange)
        {
            if (CrateSixUI.partsCollectedSix && !isSixthFixed)
            {
                if (Input.GetMouseButton(0))
                {
                    repairPercentage.value += repairSpeed * Time.deltaTime;

                    if (repairPercentage.value >= repairPercentage.maxValue)
                    {
                        repairPercentage.value = repairPercentage.maxValue;
                        isSixthFixed = true;

                        StartCoroutine(GeneratorRepairedSix());

                    }
                }
            }
            else if (!CrateSixUI.partsCollectedSix)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    StartCoroutine(ShowPartsMessageSix());
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

    IEnumerator ShowPartsMessageSix()
    {
        repairAndGenerator.SetActive(false);
        partsNeeded.SetActive(true);
        yield return new WaitForSeconds(textDuration);
        partsNeeded.SetActive(false);
    }

    IEnumerator GeneratorRepairedSix()
    {
        repairedSix.SetActive(true);
        yield return new WaitForSeconds(textDuration);
        repairedSix.SetActive(false);
    }
}
