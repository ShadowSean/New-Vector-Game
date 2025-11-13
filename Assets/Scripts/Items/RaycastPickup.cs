using System.Collections;
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
    public GameObject minimap;
    public GameObject minimapText;

    private FPController playerMovement;
    private Animator anim;
    

    pickupItem currentPickup;
    Door currentDoor;
    KeyCard currentKeyCard;

    private void Start()
    {
        playerMovement = GetComponent<FPController>();
        anim = GetComponentInChildren<Animator>();
    }


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
                case ItemType.Map:
                    if (minimap != null)
                    {
                        StartCoroutine(MapTutorial());
                    }
                    break;
            }
        }
        if (currentPickup.interactUI != null)
        {
            currentPickup.interactUI.SetActive(false);
        }
        currentPickup.gameObject.SetActive(false);
    }

    IEnumerator MapTutorial()
    {
        playerMovement.canMove = false;
        anim.enabled = false;
        minimap.SetActive(true);
        minimapText.SetActive(true);
        yield return new WaitForSeconds(5f);
        minimapText.SetActive(false);
        playerMovement.canMove = true;
        anim.enabled = true;
    }
}
