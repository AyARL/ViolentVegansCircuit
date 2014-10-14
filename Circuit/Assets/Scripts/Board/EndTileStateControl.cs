using UnityEngine;
using System.Collections;

public class EndTileStateControl : MonoBehaviour
{
    [SerializeField]
    private float duration = 1f;

    public bool Activated { get; private set; }


    private void Start()
    {
        Activated = false;
    }

    public void TileActivated()
    {
        Activated = true;
        StartCoroutine(AnimateTile());
    }

    private IEnumerator AnimateTile()
    {
        float step = 0f;

        while(Activated)
        {
            renderer.material.SetFloat("_Blend", step);
            step = Mathf.PingPong(Time.time, duration);
            yield return null;
        }
    }
}
