using UnityEngine;
using System.Collections;

public class StarEventReceiver : MenuBase
{
    [SerializeField]
    private ParticleSystem particleEffect;

    public void StarDropped()
    {
        particleEffect.Play();
    }
}
