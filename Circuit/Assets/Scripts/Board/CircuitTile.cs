using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CircuitTile : MonoBehaviour
{
    private static CircuitTilesSO prefabResource = null;
    public static CircuitTilesSO PrefabResource { get { return prefabResource; } set { prefabResource = value; } }

    public enum CircuitTileType { Tile_Empty, Tile_Start, Tile_End, Tile_Connector, Tile_Straight, Tile_Corner, Tile_TIntersection, Tile_XIntersection }

    [SerializeField]
    private CircuitTileType tileType = CircuitTileType.Tile_Empty;
    public CircuitTileType TileType { get { return tileType; } }

    private Directions.Direction tileFacingDirection = Directions.Direction.NORTH;
    public Directions.Direction TileFacingDirection { get { return tileFacingDirection; } }

    private GameObject tileMesh = null;

    public void SpawnTileOfType(CircuitTileType tileType)
    {
        if (prefabResource == null)
        {
            prefabResource = Resources.Load<CircuitTilesSO>("TilePrefabMappings");
            if (prefabResource == null)
            {
                Debug.LogError("No prefab resource set, cannot continue");
                return;
            }
        }

        this.tileType = tileType;

        if (tileMesh != null)
        {
            DestroyImmediate(tileMesh);
        }

        GameObject prefab = null;
        prefabResource.CircuitTilePrefabs.TryGetValue(tileType, out prefab);

        if (prefab != null)
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

    public void RotateClockwise()
    {
        Directions.Direction newDirection = Directions.GetNextDirectionClockwise(tileFacingDirection);
        Vector3 rotator;
        if (Directions.GetRotationForDirection(newDirection, out rotator))
        {
            tileMesh.transform.rotation = Quaternion.Euler(rotator);
            tileFacingDirection = newDirection;
        }
    }

    public void RotateCounterClockwise()
    {
        Directions.Direction newDirection = Directions.GetNextDirectionCounterClockwise(tileFacingDirection);
        Vector3 rotator;
        if (Directions.GetRotationForDirection(newDirection, out rotator))
        {
            tileMesh.transform.rotation = Quaternion.Euler(rotator);
            tileFacingDirection = newDirection;
        }
    }
}
