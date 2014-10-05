using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircuitBoard : MonoBehaviour
{
    [SerializeField] [HideInInspector]
    private List<CircuitTile> tiles = new List<CircuitTile>();
    public List<CircuitTile> Tiles { get { return tiles; } }

    [SerializeField] [HideInInspector]
    private int width = 7;
    [SerializeField] [HideInInspector]
    private int height = 5;

    float tileSize = 10f;

    public void Initialise()
    {
        for (int i = 0; i < width * height; i++)
        {
            float x = (i % width) * tileSize;
            float z = (i / width) * tileSize;

            GameObject tile = new GameObject(string.Format("Tile ({0}, {1})", x / 10, z / 10));
            tile.transform.parent = gameObject.transform;
            tile.transform.localPosition = new Vector3(x, 0, z);

            CircuitTile tileComp = tile.AddComponent<CircuitTile>();
            tileComp.SpawnTileOfType(CircuitTile.CircuitTileType.Tile_Empty);

            CircuitTileFlow flowComp = tile.AddComponent<CircuitTileFlow>();
            flowComp.Initialise();

            tiles.Add(tileComp);
        }
    }
}
