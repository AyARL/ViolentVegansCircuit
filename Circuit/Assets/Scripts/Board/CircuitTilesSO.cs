using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Builds a dictionary mapping tile prefabs to CircuitTileTpe enum, using values assigned in the editor
/// </summary>
public class CircuitTilesSO : ScriptableObject
{
    [SerializeField]
    private GameObject EmptyTile = null;

    [SerializeField]
    private GameObject StartTile = null;

    [SerializeField]
    private GameObject EndTile = null;

    [SerializeField]
    private GameObject ConnectorTile = null;

    [SerializeField]
    private GameObject StraightTile = null;

    [SerializeField]
    private GameObject CornerTile = null;

    [SerializeField]
    private GameObject TIntersectionTile = null;

    [SerializeField]
    private GameObject XIntersectionTile = null;

    

    Dictionary<CircuitTile.CircuitTileType, GameObject> circuitTilePrefabs;
    public Dictionary<CircuitTile.CircuitTileType, GameObject> CircuitTilePrefabs { get { UpdateTileMapping(); return circuitTilePrefabs; } }

    public void UpdateTileMapping()
    {

        circuitTilePrefabs = new Dictionary<CircuitTile.CircuitTileType, GameObject>();

        circuitTilePrefabs.Add(CircuitTile.CircuitTileType.Tile_Empty, EmptyTile);
        circuitTilePrefabs.Add(CircuitTile.CircuitTileType.Tile_Start, StartTile);
        circuitTilePrefabs.Add(CircuitTile.CircuitTileType.Tile_End, EndTile);
        circuitTilePrefabs.Add(CircuitTile.CircuitTileType.Tile_Connector, ConnectorTile);
        circuitTilePrefabs.Add(CircuitTile.CircuitTileType.Tile_Straight, StraightTile);
        circuitTilePrefabs.Add(CircuitTile.CircuitTileType.Tile_Corner, CornerTile);
        circuitTilePrefabs.Add(CircuitTile.CircuitTileType.Tile_TIntersection, TIntersectionTile);
        circuitTilePrefabs.Add(CircuitTile.CircuitTileType.Tile_XIntersection, XIntersectionTile);
    }
}
