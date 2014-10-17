using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class TutorialOverlay : MonoBehaviour
{
    private GameObject grid = null;
    private OverlayTile[] overlay = null;

    [SerializeField]
    private int width = 8;
    [SerializeField]
    private int height = 5;

    [SerializeField]
    private Sprite overlayImage = null;

    [SerializeField]
    private Color hideColor = Color.black;
    [SerializeField]
    private Color highlightColor = Color.white;

    private OverlayTile[] highlights = null;

    private void Start()
    {
        Recalculate();
    }

    public void DetectOverlay()
    {
        if (overlay == null)
        {
            grid = GetComponentInChildren<GridLayoutGroup>().gameObject;
            if (grid != null)
            {
                overlay = new OverlayTile[width * height];
                for (int i = 0; i < width * height; i++)
                {
                    int x = (i % width);
                    int y = (i / width);

                    GameObject panel = grid.transform.GetChild(i).gameObject;
                    panel.name = string.Format("Overlay ({0}, {1})", x, y);

                    OverlayTile overlayTile = panel.GetComponent<OverlayTile>();
                    if (overlayTile == null)
                    {
                        overlayTile = panel.AddComponent<OverlayTile>();
                    }

                    overlayTile.X = x;
                    overlayTile.Y = y;
                    overlayTile.SetImage(overlayImage);
                    overlayTile.SetColor(hideColor);

                    overlay[i] = overlayTile;
                }
            }
        }
    }

    public void Recalculate()
    {
        overlay = null;
        DetectOverlay();
    }

    public void SetOverlayHighlights(Vector2Int[] tilesToHighlight)
    {
        if(highlights != null)
        {
            foreach(OverlayTile tile in highlights)
            {
                tile.SetColor(hideColor);
            }
            highlights = null;
        }

        highlights = new OverlayTile[tilesToHighlight.Length];
        int i = 0;
        foreach(Vector2Int coord in tilesToHighlight)
        {
            var tile = overlay.FirstOrDefault(ot => ot.X == coord.X && ot.Y == coord.Y);
            if(tile != null)
            {
                tile.SetColor(highlightColor);
                highlights[i] = tile;
                i++;
            }
        }
    }


}
