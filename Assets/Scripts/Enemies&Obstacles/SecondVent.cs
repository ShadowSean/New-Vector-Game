using UnityEngine;
using UnityEngine.UI;

public class SecondVent : MonoBehaviour
{
    public GameObject ventUI, scope, ventAreaone, ventAreaTwo, player;
    private FPController moveMent;

    public Button firstUI, secondUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            moveMent = other.GetComponent<FPController>();
            if (moveMent != null)
            {
                moveMent.lookXLimit = 0;
                moveMent.LookSpeed = 0;
            }
            scope.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            ventUI.SetActive(true);

            Button firstButton = firstUI.GetComponent<Button>();
            Button secondButton = secondUI.GetComponent<Button>();

            if (firstButton != null)
            {
                firstButton.onClick.AddListener(() => GotoVentArea(ventAreaone.transform.position));
            }
            if (secondButton != null)
            {
                secondButton.onClick.AddListener(() => GotoVentArea(ventAreaTwo.transform.position));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (moveMent != null)
            {
                moveMent.lookXLimit = 45;
                moveMent.LookSpeed = 5;
            }
            scope.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            ventUI.SetActive(false);
        }
    }

    void GotoVentArea(Vector3 targetPosition)
    {
        Debug.Log("Button clicked! Teleporting player...");
        if (player != null)
        {
            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null)
            {
                cc.enabled = false;

                RaycastHit hit;
                Vector3 adjustedPosition = targetPosition;
                if (Physics.Raycast(targetPosition + Vector3.up * 2, Vector3.down, out hit, 10f))
                {
                    adjustedPosition = hit.point + Vector3.up * (cc.height / 2f);
                }
                player.transform.position = adjustedPosition;
                cc.enabled = true;
            }
            else
            {
                player.transform.position = targetPosition;
            }

            ventUI.SetActive(false);
            scope.SetActive(true);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if (moveMent != null)
            {
                moveMent.lookXLimit = 45;
                moveMent.LookSpeed = 5;
            }
        }
        else
        {
            Debug.LogWarning("Player is null! No teleport performed.");
        }
    }

}
