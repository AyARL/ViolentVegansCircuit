using UnityEngine;
using System.Collections;
using System.Linq;

public class GameController : MonoBehaviour
{
    public bool shuffledDrop = false;   //not staying here

    public enum GameState { Game_Start, Game_Setup, Game_Play, Game_Win, Game_Fail }
    private GameState gameState;

    [SerializeField]
    private GameObject board = null;

    private CircuitBoard circuitBoard = null;
    private BoardFlowControl flowControl = null;

    private bool WinConditionMet = false;
    private bool FailConditionMet = false;

    private int numberOfTargets = -1;

    private void Start()
    {
        gameState = GameState.Game_Start;
        StartCoroutine(StateMachine());
    }

    // There isn't that much game logic, so a simple state machine should be enough
    private IEnumerator StateMachine()
    {
        while (true)
        {
            yield return StartCoroutine(gameState.ToString());
            yield return null;
        }
    }

    private void Game_Start()
    {
        circuitBoard = board.GetComponent<CircuitBoard>();
        flowControl = board.GetComponent<BoardFlowControl>();

        WinConditionMet = false;
        FailConditionMet = false;

        gameState = GameState.Game_Setup;
    }

    private IEnumerator Game_Setup()
    {
        flowControl.OnImpulseRemoved += ImpulseLost;

        var tileOrderIndices = Enumerable.Range(0, circuitBoard.Tiles.Count).ToList();
        if (shuffledDrop)
        {
            Utility.Shuffle(tileOrderIndices);
        }

        foreach (int i in tileOrderIndices)
        {
            circuitBoard.Tiles[i].GetComponentInChildren<Animator>().SetTrigger("FallIn");
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(1f);

        flowControl.SpawnImpulse();
        flowControl.RunImpulses();

        gameState = GameState.Game_Play;
        yield break;
    }

    private IEnumerator Game_Play()
    {
        while (true)
        {
            yield return null;

            if(WinConditionMet)
            {
                gameState = GameState.Game_Win;
                yield break;
            }

            if(FailConditionMet)
            {
                gameState = GameState.Game_Fail;
                yield break;
            }
        }
    }

    private IEnumerator Game_Win()
    {
        flowControl.OnImpulseRemoved -= ImpulseLost;
        while (true)
        {
            yield return null;
        }
    }

    private IEnumerator Game_Fail()
    {
        flowControl.OnImpulseRemoved -= ImpulseLost;

        var tileOrderIndices = Enumerable.Range(0, circuitBoard.Tiles.Count).ToList();
        if (shuffledDrop)
        {
            Utility.Shuffle(tileOrderIndices);
        }

        foreach (int i in tileOrderIndices)
        {
            circuitBoard.Tiles[i].GetComponentInChildren<Animator>().SetTrigger("FallOut");
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void ImpulseLost(int impulsesLeft)
    {
        if (impulsesLeft == 0)
        {
            FailConditionMet = true;
        }
    }
}
