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
                //ps.loop = true;
                ps.Play();
            }
        }
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    if (collision.transform.tag == "Player")
    //    {
    //        wallPushEffect.transform.position = collision.contacts[0].point;

    //        //if(!particleSystems.Any(p => p.isPlaying))
    //        //{
    //        //    foreach (ParticleSystem ps in particleSystems)
    //        //    {
    //        //        ps.Play();
    //        //    }
    //        //}
    //    }
    //}

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
