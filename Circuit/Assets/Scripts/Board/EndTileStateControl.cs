using UnityEngine;
using System.Collections;
using Circuit;

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
        CAudioControl.CreateAndPlayAudio( CAudio.AUDIO_EFFECT_CHIP_POWER, false, true, false, 0.3f );
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
