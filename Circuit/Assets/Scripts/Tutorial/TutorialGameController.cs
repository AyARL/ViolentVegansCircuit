using UnityEngine;
using System.Collections;

public class TutorialGameController : GameController
{
    public enum TutorialState { Tutorial_None, Tutorial_Start, Tutorial_Play, Tutorial_Tether, Tutorial_Win, Tutorial_Fail }
    private TutorialState tutorialState = TutorialState.Tutorial_None;

    [SerializeField]
    private TutorialInstructionQueue tutorialInstructions = null;

    void Start()
    {
        
    }

}
