﻿using UnityEngine;
using System.Collections;
using System.Linq;
using Pathfinding.Serialization.JsonFx;

public class GameController : MonoBehaviour
{
    public bool shuffledDrop = false;   //not staying here

    public enum GameState { Game_Start, Game_Setup, Game_Play, Game_Win, Game_Fail, Game_ScoreDisplay }
    private GameState gameState;

    [SerializeField]
    private GameObject board = null;

    private CircuitBoard circuitBoard = null;
    private BoardFlowControl flowControl = null;

    [SerializeField]
    private GameObject Player = null;
    [SerializeField]
    private GameObject PlayerFace = null;

    private bool WinConditionMet = false;
    private bool FailConditionMet = false;

    private int endPointsTotal = -1;
    private int numberOfInactiveEndPoints = -1;

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
        flowControl.OnEndPointActivated += EndPointActivated;

        endPointsTotal = circuitBoard.GetEndTilesCount();
        numberOfInactiveEndPoints = endPointsTotal;

        // Spawn tiles
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

        PlayerFace.SetActive(true);
        Player.SetActive(true);

        yield return new WaitForSeconds(1f);

        gameState = GameState.Game_Play;
        yield break;
    }

    private IEnumerator Game_Play()
    {
        // Spawn Impulse
        flowControl.SpawnImpulse();
        flowControl.RunImpulses();

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
        flowControl.OnEndPointActivated -= EndPointActivated;

        Debug.Log("Win!");

        SetLevelStatus(true);
        Application.LoadLevelAdditive("LevelEndScreen");
        gameState = GameState.Game_ScoreDisplay;
        yield break;
    }

    private IEnumerator Game_Fail()
    {
        flowControl.OnImpulseRemoved -= ImpulseLost;
        flowControl.OnEndPointActivated -= EndPointActivated;

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

        SetLevelStatus(false);
        Application.LoadLevelAdditive("LevelEndScreen");
        gameState = GameState.Game_ScoreDisplay;
    }

    private IEnumerator Game_ScoreDisplay()
    {
        while(true)
        {
            yield return null;
        }
    }

    private void EndPointActivated()
    {
        numberOfInactiveEndPoints -= 1;
        if(numberOfInactiveEndPoints == 0)
        {
            WinConditionMet = true;
        }
    }

    private void ImpulseLost(int impulsesLeft)
    {
        if (impulsesLeft == 0)
        {
            if (numberOfInactiveEndPoints == endPointsTotal)
            {
                FailConditionMet = true;
            }
            else
            {
                WinConditionMet = true;
            }
        }
    }

    private void SetLevelStatus(bool levelWon)
    {
        string levelStatus = JsonWriter.Serialize(new CompletedLevelStatus()
        {
            LevelIndex = Application.loadedLevel,
            LevelWon = levelWon,
            ActivatedChips = endPointsTotal - numberOfInactiveEndPoints,
            MaxChips = endPointsTotal
        });

        PlayerPrefs.SetString("LevelStatus", levelStatus);
    }
}