using UnityEngine;
using System.Collections;

public class ButtonParticles : MonoBehaviour
{
    ParticleSystem[] particleSystems = null;

    void Awake()
    {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
    }
    
    public void Play()
    {
        foreach(ParticleSystem ps in particleSystems)
        {
            ps.Play();
        }
    }
}
