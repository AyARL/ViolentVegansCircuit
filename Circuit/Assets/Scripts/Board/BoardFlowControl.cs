using UnityEngine;
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
    private GameObject ImpulsePrefab = null;

    [SerializeField]
    private List<Impulse> impulses;

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

        var startTiles = board.Tiles.Where(t => t.TileType == CircuitTile.CircuitTileType.Tile_Start).Select(t => t.gameObject.GetComponent<CircuitTileFlow>());
        foreach(CircuitTileFlow tile in startTiles)
        {
            StartPathMarker startMarker = tile.GetEntryMarker as StartPathMarker;
            if(startMarker != null)
            {
                GameObject impulse = Instantiate(ImpulsePrefab) as GameObject;
                Impulse impulseComp = impulse.GetComponent<Impulse>();
                impulseComp.Initialise(startMarker, startMarker.NextMarker);
                impulseComp.Speed = impulseSpeed;

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
        foreach(Impulse i in impulses)
        {
            i.RunImpulse();
        }
    }

    private void ResetImpulses()
    {
        foreach(Impulse i in impulses.ToList())
        {
            Destroy(i.gameObject);
        }
        impulses.Clear();
    }
}
