using UnityEngine;

public class FinalDoorActivation : MonoBehaviour
{
    public GameObject finalDoor;
    private bool doorDeactivated = false;

    private void Update()
    {
        if (!doorDeactivated && GeneratorLogic.isFixed  && SecondGeneratorLogic.isSecondFixed  && GeneratorThree.isThirdFixed 
            && GeneratorFour.isFourthFixed && GeneratorFive.isFifthFixed && GeneratorSix.isSixthFixed )
        {
            finalDoor.SetActive(false);
            doorDeactivated = true;
        }
    }
}
