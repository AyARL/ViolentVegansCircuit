using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class OverlayTile : MonoBehaviour
{
    public int X { get; set; }
    public int Y { get; set; }

    public void SetImage(Sprite image)
    {
        GetComponent<Image>().sprite = image;
    }

    public void SetColor(Color color)
    {
        GetComponent<Image>().color = color;
    }
}
