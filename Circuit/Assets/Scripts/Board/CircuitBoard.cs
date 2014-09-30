using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircuitBoard : MonoBehaviour
{
    [SerializeField]
    private List<CircuitTile> tiles = new List<CircuitTile>();

    int width = 7;
    int height = 5;

    float tileSize = 10f;

    public void Initialise()
    {
        CircuitTilesSO prefabResource = Resources.Load<CircuitTilesSO>("TilePrefabMappings");
        if (prefabResource != null)
        {
            GameObject emptyPrefab = null;
            prefabResource.CircuitTilePrefabs.TryGetValue(CircuitTilesSO.CircuitTileType.Tile_Empty, out emptyPrefab);

            if (emptyPrefab != null)
            {
                for (int i = 0; i < width * height; i++)
                {
                    GameObject tile = Instantiate(emptyPrefab, Vector3.zero, emptyPrefab.transform.rotation) as GameObject;
                    tile.transform.parent = gameObject.transform;

                    float x = (i % width) * tileSize;
                    float z = (i / width) * tileSize;

                    tile.transform.localPosition = new Vector3(x, 0, z);
                }
            }
        }
    }
}
