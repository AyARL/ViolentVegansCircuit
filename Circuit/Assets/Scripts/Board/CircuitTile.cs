using UnityEngine;
using System.Collections;

public class CircuitTile : MonoBehaviour
{
    public enum CircuitTileType { Tile_Empty, Tile_Start, Tile_End, Tile_Connector, Tile_Straight, Tile_Corner, Tile_TIntersection, Tile_XIntersection }

    [SerializeField]
    private CircuitTileType tileType = CircuitTileType.Tile_Empty;
    public CircuitTileType TileType { get { return tileType; } }

    private GameObject tileMesh = null;

    public void SpawnTileOfType(CircuitTileType tileType, CircuitTilesSO tileResource)
    {
        this.tileType = tileType;

        if (tileMesh != null)
        {
            DestroyImmediate(tileMesh);
        }

        GameObject prefab = null;
        tileResource.CircuitTilePrefabs.TryGetValue(tileType, out prefab);

        if(prefab != null)
        {
            tileMesh = Instantiate(prefab) as GameObject;
            tileMesh.transform.parent = gameObject.transform;
            tileMesh.transform.localPosition = Vector3.zero;
        }
        else
        {
            Debug.LogError(string.Format("No prefab found for tile type {0}", tileType.ToString()));
        }
    }
}
