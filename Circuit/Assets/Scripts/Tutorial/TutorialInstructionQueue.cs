using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TutorialInstructionQueue : ScriptableObject
{
    [SerializeField]
    private List<TutorialInstruction> instructionResources = null;

    private List<TutorialInstruction> instructions = null;

    public void Initialise()
    {
        instructions = new List<TutorialInstruction>(instructionResources);
    }

    public IEnumerable<TutorialInstruction> GetInstructionsForState(TutorialGameController.TutorialState state)
    {
        if (instructions.Count > 0)
        {
            var stateInstructions = instructions.Where(i => i.TriggerState == state);

            instructions = instructions.Except(stateInstructions).ToList();

            // Why can't I do that
            //foreach (TutorialInstruction i in stateInstructions)
            //{
            //    instructions.Remove(i);
            //}

            return stateInstructions;
        }
        else
        {
            return null;
        }
    }
}
