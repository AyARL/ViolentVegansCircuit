using UnityEngine;
using System.Collections;

public class StarEventReceiver : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particleEffect;

    public void StarDropped()
    {
        particleEffect.Play();
    }
}
