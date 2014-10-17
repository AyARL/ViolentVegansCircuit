using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

#if UNITY_EDITOR
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
#endif

    public IEnumerable<CircuitTile> GetStartTiles()
    {
       return tiles.Where(t => t.TileType == CircuitTile.CircuitTileType.Tile_Start);
    }

    public IEnumerable<CircuitTile> GetEndTiles()
    {
        return tiles.Where(t => t.TileType == CircuitTile.CircuitTileType.Tile_End);
    }

    public int GetEndTilesCount()
    {
        return GetEndTiles().ToArray().Length;
    }

    public CircuitTile GetTileInDirection(CircuitTile origin, Directions.Direction boardDirection)
    {
        int x, y;
        int index = -1;
        if(IndexToXY(tiles.IndexOf(origin), out x, out y))
        {
            switch(boardDirection)
            {
                case Directions.Direction.NORTH:
                    y += 1;
                    break;
                case Directions.Direction.EAST:
                    x += 1;
                    break;
                case Directions.Direction.SOUTH:
                    y -= 1;
                    break;
                case Directions.Direction.WEST:
                    x -= 1;
                    break;
                default:
                    return null;
            }
        }

        if (XYtoIndex(x, y, out index))
        {
            return tiles[index];
        }
        else
        {
            return null;
        }
    }

    private bool IndexToXY(int index, out int x, out int y)
    {
        x = index % width;
        y = index / width;

        return index >= 0 && index < tiles.Count;
    }

    private bool XYtoIndex(int x, int y, out int index)
    {
        index = y * width + x;
        return index >= 0 && index < tiles.Count;
    }
}
