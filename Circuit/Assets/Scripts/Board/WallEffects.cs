using UnityEngine;
using System.Collections;
using System.Linq;

public class WallEffects : MonoBehaviour
{
    [SerializeField]
    private GameObject wallPushEffect = null;

    private ParticleSystem[] particleSystems = null;

    private void Start()
    {
        particleSystems = wallPushEffect.GetComponentsInChildren<ParticleSystem>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            wallPushEffect.transform.position = collision.contacts[0].point;
            foreach (ParticleSystem ps in particleSystems)
            {
                ps.Play();
                VibrationManager.Vibrate(20);
            }
        }
    }



    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            foreach (ParticleSystem ps in particleSystems)
            {
                ps.Stop();
            }
        }
    }
}
