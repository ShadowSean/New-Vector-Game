using UnityEngine;

public class GensFixed : MonoBehaviour
{
    private GeneratorLogic generatorLogic;
    private SecondGeneratorLogic secondGeneratorLogic;
    private GeneratorThree genThree;
    private GeneratorFour genFour;
    private GeneratorFive genFive;
    private GeneratorSix genSix;

    public GameObject door;

    private void Update()
    {
        if (GeneratorLogic.isFixed && SecondGeneratorLogic.isSecondFixed && GeneratorThree.isThirdFixed
            && GeneratorFour.isFourthFixed && GeneratorFive.isFifthFixed && GeneratorSix.isSixthFixed)
        {
            door.SetActive(false);
        }

    }
}
