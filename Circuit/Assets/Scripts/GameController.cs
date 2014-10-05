using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject board = null;

    private CircuitBoard circuitBoard = null;
    private BoardFlowControl flowControl = null;

    private void Start()
    {
        circuitBoard = board.GetComponent<CircuitBoard>();
        flowControl = board.GetComponent<BoardFlowControl>();

        flowControl.SpawnImpulse();
        flowControl.RunImpulses();
    }

}
