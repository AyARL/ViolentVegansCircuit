using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

[ExecuteInEditMode]
public class CircuitTile : MonoBehaviour
{
    [SerializeField] [HideInInspector]
    private static CircuitTilesSO prefabResource = null;
    public static CircuitTilesSO PrefabResource { get { return prefabResource; } set { prefabResource = value; } }

    public enum CircuitTileType 
    { 
        Tile_Empty, 
        Tile_Start, 
        Tile_End, 
        Tile_In_Connector,
        Tile_Out_Connector,
        Tile_Straight, 
        Tile_Corner_Left, 
        Tile_Corner_Right, 
        Tile_TIntersection, 
        Tile_XIntersection 
    }

    [SerializeField] [HideInInspector]
    private CircuitTileType tileType = CircuitTileType.Tile_Empty;
    public CircuitTileType TileType { get { return tileType; } }

    [SerializeField]
    private Directions.Direction tileFacingDirection = Directions.Direction.NORTH;
    public Directions.Direction TileFacingDirection { get { return tileFacingDirection; } }

    [SerializeField] [HideInInspector]
    private GameObject tileMesh;

#if UNITY_EDITOR
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
            tileMesh = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            tileMesh.transform.parent = gameObject.transform;
            tileMesh.transform.localPosition = Vector3.zero;

            Vector3 rotator;
            if (Directions.GetRotationForDirection(tileFacingDirection, out rotator))
            {
                tileMesh.transform.rotation = Quaternion.Euler(rotator);
            }

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

#endif
}
