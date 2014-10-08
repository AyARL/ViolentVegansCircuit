using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BoardFlowControl : MonoBehaviour
{
    [SerializeField]
    CircuitBoard board = null;

    [SerializeField]
    float impulseSpeed = 1.0f;

    [SerializeField]
    private GameObject impulsePrefab = null;

    [SerializeField]
    private GameObject ballTether = null;
    public GameObject BallTether { get { return ballTether; } }

    private List<Impulse> impulses;

    // Called when impulse is destroyed by reaching empty connector, passes number of remaining impulses as parameter
    public UnityAction<int> OnImpulseRemoved { get; set; }

    // Called when the impulse reaches 
    public UnityAction OnEndPointActivated { get; set; }

    private void Reset()
    {
        board = gameObject.GetComponent<CircuitBoard>();
    }

    private void Awake()
    {
        impulses = new List<Impulse>();
    }

    public void SpawnImpulse()
    {
        ResetImpulses();

        var startTiles = board.GetStartTiles().Select(t => t.GetComponent<CircuitTileFlow>());
        foreach (CircuitTileFlow tile in startTiles)
        {
            StartPathMarker startMarker = tile.EntryMarker as StartPathMarker;
            if (startMarker != null)
            {
                GameObject impulse = Instantiate(impulsePrefab) as GameObject;
                Impulse impulseComp = impulse.GetComponent<Impulse>();
                impulseComp.PutOnSegment(startMarker, startMarker.NextMarker);
                impulseComp.Speed = impulseSpeed;
                impulseComp.OnMarkerReached = m => ImpulseReachedTarget(impulseComp, m);    // Curry ;D

                impulses.Add(impulseComp);
            }
            else
            {
                Debug.LogException(new System.NullReferenceException(string.Format("Start tile {0} did not provide a StartPathMarker", tile.gameObject)));
            }
        }
    }

    public void RunImpulses()
    {
        foreach (Impulse i in impulses)
        {
            i.RunImpulse();
        }
    }

    private void ImpulseReachedTarget(Impulse impulse, PathMarker targetMarker)
    {
        if (TerminatorMarkerCheck(impulse, targetMarker)) { return; }

        if (PathPathMarkerCheck(impulse, targetMarker)) { return; }

        if (EndMarkerCheck(impulse, targetMarker)) { return; }

        if (OutConnectorCheck(impulse, targetMarker)) { return; }

        if (IntersectionConnectorCheck(impulse, targetMarker)) { return; }

        Debug.Log(string.Format("Unrecognised Target Marker: {0}", targetMarker));
    }

    #region MarkerChecks

    private bool PathPathMarkerCheck(Impulse impulse, PathMarker targetMarker)
    {
        PathPathMarker pathMarker = targetMarker as PathPathMarker;
        if (pathMarker != null)
        {
            impulse.PutOnSegment(pathMarker, pathMarker.NextMarker);
            impulse.RunImpulse();
            return true;
        }

        return false;
    }

    private bool EndMarkerCheck(Impulse impulse, PathMarker targetMarker)
    {
        EndPathMarker endMarker = targetMarker as EndPathMarker;
        if (endMarker != null)
        {
            CircuitTile tile = endMarker.GetComponentInParent<CircuitTile>();

            CircuitTile nextTile = board.GetTileInDirection(tile, Directions.LocalToBoardDirection(endMarker.ExitDirection, tile));
            if (nextTile != null)
            {
                CircuitTileFlow tileFlow = nextTile.GetComponent<CircuitTileFlow>();

                StartPathMarker startMarker = tileFlow.EntryMarker as StartPathMarker;
                if (startMarker != null)
                {
                    impulse.PutOnSegment(startMarker, startMarker.NextMarker);
                    impulse.RunImpulse();
                }
                else
                {
                    Debug.LogException(new System.NullReferenceException(string.Format("Start tile {0} did not provide a StartPathMarker", tile.gameObject)));
                }
            }

            return true;
        }

        return false;
    }

    private bool TerminatorMarkerCheck(Impulse impulse, PathMarker targetMarker)
    {
        TerminatorPathMarker terminatorMarker = targetMarker as TerminatorPathMarker;
        if (terminatorMarker != null)
        {
            EndTileStateControl endControl = targetMarker.GetComponentInParent<EndTileStateControl>();
            if(!endControl.Activated)
            {
                endControl.TileActivated();
                EndPointActivated(impulse);
            }
            return true;
        }

        return false;
    }

    private bool OutConnectorCheck(Impulse impulse, PathMarker targetMarker)
    {
        OutConnectorPathMarker outMarker = targetMarker as OutConnectorPathMarker;
        if (outMarker != null)
        {
            if(!outMarker.GetComponentInParent<CircuitTileFlow>().BallAttached)
            {
                RemoveImpulse(impulse);
                return true;
            }

            CircuitTile tile = outMarker.GetComponentInParent<CircuitTile>();
            Directions.Direction entryDirection = Directions.LocalToBoardDirection(Directions.Direction.SOUTH, tile);

            foreach (Directions.Direction dir in System.Enum.GetValues(typeof(Directions.Direction)))
            {
                if (dir == entryDirection)
                {
                    continue;
                }

                if (TrySkipToConnector(impulse, tile, dir))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool IntersectionConnectorCheck(Impulse impulse, PathMarker targetMarker)
    {
        IntersectionPathMarker intersectionMarker = targetMarker as IntersectionPathMarker;
        if (intersectionMarker != null)
        {
            bool first = true;
            foreach (PathMarker exit in intersectionMarker.IntersectionExits)
            {
                if (first)
                {
                    impulse.PutOnSegment(intersectionMarker, exit);
                    impulse.RunImpulse();
                    first = false;
                }
                else
                {
                    GameObject newImpulse = Instantiate(impulsePrefab) as GameObject;
                    Impulse impulseComp = newImpulse.GetComponent<Impulse>();
                    impulseComp.PutOnSegment(intersectionMarker, exit);
                    impulseComp.Speed = impulseSpeed;
                    impulseComp.OnMarkerReached = m => ImpulseReachedTarget(impulseComp, m);    // Curry ;D

                    impulses.Add(impulseComp);

                    impulseComp.RunImpulse();
                }
            }
            return true;
        }

        return false;
    }

    #endregion

    private bool TrySkipToConnector(Impulse impulse, CircuitTile currentTile, Directions.Direction boardDirection)
    {

        CircuitTile nextTile = board.GetTileInDirection(currentTile, boardDirection);
        if (nextTile != null)
        {
            CircuitTileFlow tileFlow = nextTile.GetComponent<CircuitTileFlow>();

            InConnectorPathMarker inMarker = tileFlow.EntryMarker as InConnectorPathMarker;
            if (inMarker != null)
            {
                if (tileFlow.BallAttached)
                {
                    impulse.PutOnSegment(inMarker, inMarker.NextMarker);
                    impulse.RunImpulse();
                }
                else
                {
                    RemoveImpulse(impulse);
                }
                return true;
            }
        }
        return false;
    }

    private void ResetImpulses()
    {
        foreach (Impulse i in impulses.ToList())
        {
            Destroy(i.gameObject);
        }
        impulses.Clear();
    }

    private void RemoveImpulse(Impulse impulse)
    {
        impulses.Remove(impulse);
        Destroy(impulse.gameObject);
        if(OnImpulseRemoved != null)
        {
            OnImpulseRemoved(impulses.Count);
        }
    }

    private void EndPointActivated(Impulse impulse)
    {
        if(OnEndPointActivated != null)
        {
            OnEndPointActivated();
        }
        RemoveImpulse(impulse);
    }
}
