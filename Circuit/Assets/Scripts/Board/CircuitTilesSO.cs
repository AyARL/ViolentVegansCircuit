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

    public enum CircuitTileType { Tile_Empty, Tile_Start, Tile_End, Tile_Connector, Tile_Straight, Tile_Corner, Tile_TIntersection, Tile_XIntersection }

    Dictionary<CircuitTileType, GameObject> circuitTilePrefabs;
    public Dictionary<CircuitTileType, GameObject> CircuitTilePrefabs { get { UpdateTileMapping(); return circuitTilePrefabs; } }

    public void UpdateTileMapping()
    {

        circuitTilePrefabs = new Dictionary<CircuitTileType, GameObject>();

        circuitTilePrefabs.Add(CircuitTileType.Tile_Empty, EmptyTile);
        circuitTilePrefabs.Add(CircuitTileType.Tile_Start, StartTile);
        circuitTilePrefabs.Add(CircuitTileType.Tile_End, EndTile);
        circuitTilePrefabs.Add(CircuitTileType.Tile_Connector, ConnectorTile);
        circuitTilePrefabs.Add(CircuitTileType.Tile_Straight, StraightTile);
        circuitTilePrefabs.Add(CircuitTileType.Tile_Corner, CornerTile);
        circuitTilePrefabs.Add(CircuitTileType.Tile_TIntersection, TIntersectionTile);
        circuitTilePrefabs.Add(CircuitTileType.Tile_XIntersection, XIntersectionTile);
    }
}
