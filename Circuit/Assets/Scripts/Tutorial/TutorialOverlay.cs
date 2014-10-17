using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialOverlay : MonoBehaviour
{
    private GameObject grid = null;
    private Image[] overlay = null;

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

    public void DetectOverlay()
    {
        if (overlay == null)
        {
            grid = GetComponentInChildren<GridLayoutGroup>().gameObject;
            if (grid != null)
            {
                overlay = new Image[width * height];
                for (int i = 0; i < width * height; i++)
                {
                    int x = (i % width);
                    int y = (i / width);

                    GameObject panel = grid.transform.GetChild(i).gameObject;
                    panel.name = string.Format("Overlay ({0}, {1})", x, y);

                    OverlayTile overlayTile = panel.GetComponent<OverlayTile>();
                    if(overlayTile == null)
                    {
                        overlayTile = panel.AddComponent<OverlayTile>();
                    }

                    overlayTile.X = x;
                    overlayTile.Y = y;
                    overlayTile.SetImage(overlayImage);
                    overlayTile.SetColor(hideColor);
                    
                }
            }
        }
    }

    public void Recalculate()
    {
        overlay = null;
        DetectOverlay();
    }
}
