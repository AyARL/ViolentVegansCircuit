using UnityEngine;
using System.Collections;

public class EndTileStateControl : MonoBehaviour
{
    public bool Activated { get; private set; }

    private void Start()
    {
        Activated = false;
    }

    public void TileActivated()
    {
        Activated = true;
        // Do the animation
    }
}
