using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using Circuit;

public class CircuitTileFlow : MonoBehaviour
{
    [SerializeField]
    private PathMarker entryMarker = null;
    public PathMarker EntryMarker { get { return entryMarker; } }

    public enum EntryType { Entry_Invalid, Entry_Flow, Entry_Connector }
    public EntryType TypeOfEntry { get; private set; }

    public enum ExitType { Exit_Invalid, Exit_Flow, Exit_Connector, Exit_Terminator }
    public ExitType TypeOfExit { get; private set; }

    private GameObject ballTether = null;

    private int m_iAudioID;

    public bool BallAttached { get; private set; }
    public UnityAction OnBallAttached { get; set; }
    public UnityAction OnBallDetached { get; set; }

    private void Reset()
    {
        Initialise();
    }

    private void Awake()
    {
        Initialise();
    }

    public void Initialise()
    {
        if (gameObject.GetComponent<CircuitTile>().TileType != CircuitTile.CircuitTileType.Tile_Empty && 
            gameObject.GetComponent<CircuitTile>().TileType != CircuitTile.CircuitTileType.Tile_Obstacle)
        {
            entryMarker = gameObject.GetComponentInChildren<StartPathMarker>();
            if (entryMarker != null)
            {
                TypeOfEntry = EntryType.Entry_Flow;
            }
            else
            {
                entryMarker = gameObject.GetComponentInChildren<InConnectorPathMarker>();
                if (entryMarker != null)
                {
                    TypeOfEntry = EntryType.Entry_Connector;
                }
                else
                {
                    TypeOfEntry = EntryType.Entry_Invalid;
                    Debug.LogError(string.Format("Tile {0} has no StartPathMarker or InConnectorMarker", gameObject.name));
                    return;
                }
            }

            PathMarker end = gameObject.GetComponentInChildren<EndPathMarker>();
            if (end != null)
            {
                TypeOfExit = ExitType.Exit_Flow;
            }
            else
            {
                end = gameObject.GetComponentInChildren<OutConnectorPathMarker>();
                if (end != null)
                {
                    TypeOfExit = ExitType.Exit_Connector;
                }
                else
                {
                    end = gameObject.GetComponentInChildren<TerminatorPathMarker>();
                    if (end != null)
                    {
                        TypeOfExit = ExitType.Exit_Terminator;
                    }
                    else
                    {
                        TypeOfExit = ExitType.Exit_Invalid;
                        Debug.LogError(string.Format("Tile {0} has no EndPathMarker or OutConnectorMarker", gameObject.name));
                        return;
                    }
                }
            }
        }
    }

    private void OnReceiveMessage(Vector3 ballPos)
    {
        if (TypeOfExit == ExitType.Exit_Connector || TypeOfEntry == EntryType.Entry_Connector)
        {
            // Set the audioID variable to keep track of the current ball tether and play the effect.
            if ( m_iAudioID <= 0 )
                m_iAudioID = CAudioControl.CreateAndPlayAudio( CAudio.AUDIO_EFFECT_ELECTRIC_LOOP, true, true, false, 0.2f );

            UpdateBallTether(gameObject.transform.position, ballPos);
        }
        
    }

    private void UpdateBallTether(Vector3 startPos, Vector3 target)
    {
        BoardFlowControl flowControl = GetComponentInParent<BoardFlowControl>();
        GameObject tether = flowControl.BallTether;

        float tetherScale = Vector3.Distance(startPos, target);
        float angle = Vector3.Angle(Vector3.right, new Vector3(target.x - startPos.x, 0, target.z - startPos.z));
        float dot = Vector3.Dot(Vector3.forward, target - startPos);
        if (dot > 0)
        {
            angle = 360 - angle;
        }

        if (ballTether == null)
        {
            ballTether = Instantiate(tether, startPos, Quaternion.identity) as GameObject;
            BallAttached = true;
            if (OnBallAttached != null)
            {
                OnBallAttached();
            }
        }

        if (ballTether != null)
        {
            float radAngle = Mathf.Deg2Rad * angle;
            foreach (ParticleSystem ps in ballTether.GetComponentsInChildren<ParticleSystem>())
            {
                ps.gameObject.transform.localPosition = new Vector3(tetherScale / 2 * Mathf.Cos(radAngle), 0, tetherScale / 2 * -Mathf.Sin(radAngle));
                ps.startSize = tetherScale;
                ps.startRotation = radAngle;
            }
        }
    }

    private void OnBallExit(Vector3 ballPos)
    {
        if (ballTether != null)
        {
            CAudioControl.StopSound( m_iAudioID, false );
            m_iAudioID = 0;
            Destroy(ballTether);
            ballTether = null;
            BallAttached = false;
            if (OnBallDetached != null)
            {
                OnBallDetached();
            }
        }
    }
}
