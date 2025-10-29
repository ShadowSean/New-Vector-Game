using UnityEngine;
using UnityEngine.UI;

public class EscapePodCode : MonoBehaviour
{
    public GameObject escapeUI;
    public InputField escapeCode;
    public GameObject scope, inventory, flashBarandStamina,escapePodDoor;
    public string escapecodeText = "54321";

    private FPController moveMent;

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
            inventory.SetActive(false);
            flashBarandStamina.SetActive(false);
            
            escapeUI.SetActive(true);
            escapeCode.onEndEdit.AddListener(CorrectCode);
        }
    }

    void CorrectCode(string playerInput)
    {
        if (playerInput == escapecodeText)
        {
            escapePodDoor.SetActive(true);
            escapeUI.SetActive(false );
        }
    }
}
