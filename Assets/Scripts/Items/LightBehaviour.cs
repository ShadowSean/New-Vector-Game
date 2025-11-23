using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class LightBehaviour : MonoBehaviour
{
    Light newFlashlight;

    public GameObject batteryUI;

    public bool drainOvertime;
    public float maxBrightness;
    public float minBrightness;
    public float drainRate;
    public GameObject batteryInt;

    public GameObject battery;
    public Slider batteryBar;
    bool canReplaceBattery;

    private void Start()
    {
        
        newFlashlight = GetComponent<Light>();
    }

    private void Update()
    {
        newFlashlight.intensity = Mathf.Clamp(newFlashlight.intensity, minBrightness, maxBrightness);
        if (drainOvertime == true && newFlashlight.enabled == true)
        {
            if (newFlashlight.intensity > minBrightness)
            {
                newFlashlight.intensity -= Time.deltaTime * (drainRate / 1000);
            }
        }

        batteryBar.value = Mathf.InverseLerp(minBrightness,maxBrightness,newFlashlight.intensity);

        if (Input.GetKeyDown(KeyCode.F))
        {
            newFlashlight.enabled = !newFlashlight.enabled;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            batteryInt.SetActive(false);
            battery.SetActive(false);
            ReplaceBattery(1f);
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Battery"))
        {
           canReplaceBattery = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Battery"))
        {
            canReplaceBattery = false;
        }
    }

    public void ReplaceBattery(float amount)
    {
        newFlashlight.intensity = Mathf.Clamp(newFlashlight.intensity + amount, minBrightness, maxBrightness);
    }
}
