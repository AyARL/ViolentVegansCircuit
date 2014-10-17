using UnityEngine;
using System.Collections;

public class TutorialInstruction : ScriptableObject
{
    [SerializeField]
    private string instructionText = null;
    public string InstructionText { get { return instructionText; } }

    [SerializeField]
    private Vector2Int[] overlayTilesToHighlight = null;


}

[System.Serializable]
public struct Vector2Int
{
    [SerializeField]
    private int x;
    public int X { get { return x; } }

    [SerializeField]
    private int y;
    public int Y { get { return y; } }
}