using UnityEngine;
using System.Collections;

public class ConnectorEffects : MonoBehaviour
{
    private ParticleSystem[] particles = null;
    CircuitTileFlow tileFlow = null;

    private void Awake()
    {
        tileFlow = GetComponentInParent<CircuitTileFlow>();
        tileFlow.OnBallAttached += StartParticles;
        tileFlow.OnBallDetached += StopParticles;

        particles = GetComponentsInChildren<ParticleSystem>();
    }

    private void StartParticles()
    {
        foreach (ParticleSystem ps in particles)
        {
            ps.Play();
        }
    }

    private void StopParticles()
    {
        foreach (ParticleSystem ps in particles)
        {
            ps.Stop();
        }
    }

    private void OnDestroy()
    {
        tileFlow.OnBallAttached -= StartParticles;
        tileFlow.OnBallDetached -= StopParticles;
    }
}
