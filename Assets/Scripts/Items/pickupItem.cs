using System.ComponentModel;
using UnityEngine;

public class pickupItem : MonoBehaviour
{
    public GameObject playerItems;
    public string itemName;

    public ItemType itemType;

    public GameObject interactUI;

    bool playerInRange;

    private void Start()
    {
        if (interactUI != null)
        {
            interactUI.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactUI != null)
            {
                interactUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactUI != null)
            {
                interactUI.SetActive(false);
            }
        }
    }

    public bool isPlayerInRange()
    {
        return playerInRange;
    }
}

public enum ItemType
{
    None,
    Flashlight,
    Taser,
    Map
    //CodeOne
   
}
