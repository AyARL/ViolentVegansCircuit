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
            var insts = instructions.Where(i => i.TriggerState == state);
            //foreach (TutorialInstruction i in insts)
            //{
            //    instructions.Remove(i);
            //}

            return insts;
        }
        else
        {
            return null;
        }
    }
}
