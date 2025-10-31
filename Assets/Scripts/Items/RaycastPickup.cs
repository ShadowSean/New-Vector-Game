using System.ComponentModel;
using UnityEngine;

public class RaycastPickup : MonoBehaviour
{
    public float pickupRange = 3f;
    public LayerMask interactLayer;
    public KeyCode pickupKey = KeyCode.E;

    public Camera playerCamera;
    public GameObject scope;
    public ItemSwitcher itemSwitcher;

    pickupItem currentPickup;
    Door currentDoor;
    KeyCard currentKeyCard;
    EscapePodCode currentEscapePodCode;
    
    

    private void Update()
    {
        RaycastHit hit;
        bool hitSomething = false;

        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * pickupRange, Color.green);

        if (Physics.Raycast(playerCamera.transform.position,  playerCamera.transform.forward, out hit, pickupRange, interactLayer))
        {

            hitSomething = true;
            currentPickup = hit.collider.GetComponent<pickupItem>();
            currentDoor = hit.collider.GetComponentInParent<Door>();
            currentKeyCard = hit.collider.GetComponent<KeyCard>();
            currentEscapePodCode = hit.collider.GetComponent<EscapePodCode>();

            if (currentPickup != null)
            {
               

                if (Input.GetKeyDown(pickupKey))
                {
                    PickUp(currentPickup);
                }
            }
            else if (currentDoor != null)
            {
                currentDoor.ShowInteractPromt(true);
                if (Input.GetKeyDown(pickupKey))
                {
                    currentDoor.Interact();
                    
                }
            }
            else if (currentKeyCard != null)
            {
                currentKeyCard.ShowInteractPromt(true);
                if (Input.GetKeyDown(pickupKey))
                {
                    currentKeyCard.Interact();
                    currentKeyCard.ShowInteractPromt(false);
                    currentKeyCard = null;
                }
            }
            else if (currentEscapePodCode != null)
            {
                currentEscapePodCode.ShowInteractPromt(true);
                if (Input.GetKeyDown(pickupKey))
                {
                    currentEscapePodCode.Interact();
                    currentEscapePodCode.ShowInteractPromt(false);
                    currentEscapePodCode = null;
                }
            }
        }
        if (!hitSomething)
        {
            if (currentDoor != null)
            {
                currentDoor.ShowInteractPromt(false);
                currentDoor = null;
            }
            if (currentKeyCard != null)
            {
                currentKeyCard.ShowInteractPromt(false);
                currentKeyCard = null;
            }
            if (currentEscapePodCode == null)
            {
                currentEscapePodCode.ShowInteractPromt(false);
                currentEscapePodCode = null;    
            }
            if (currentPickup != null)
            {
                currentPickup = null;
            }
        }
     
    }

    void PickUp(pickupItem currentPickup)
    {
        if (currentPickup.playerItems != null)
        {
            currentPickup.playerItems.SetActive(true);
        }
        if (itemSwitcher != null)
        {
            switch (currentPickup.itemType)
            {
                case ItemType.Flashlight:
                    itemSwitcher.PickupFlashlight();
                    break;
                case ItemType.Taser:
                    itemSwitcher.PickupTaser();
                    break;
            }
        }
        currentPickup.gameObject.SetActive(false);
    }
}
