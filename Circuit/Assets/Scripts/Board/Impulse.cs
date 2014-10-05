using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Impulse : MonoBehaviour
{
    private PathMarker currentOrigin = null;
    public PathMarker CurrentOrigin { get { return currentOrigin; } }

    private PathMarker currentTarget = null;
    public PathMarker CurrentTarget { get { return currentTarget; } set { currentTarget = value; } }

    public UnityAction<PathMarker> OnMarkerReached { get; set; }

    public float Speed { get; set; }

    public void PutOnSegment(PathMarker origin, PathMarker target)
    {
        currentOrigin = origin;
        currentTarget = target;

        gameObject.transform.position = origin.transform.position;
    }

    public void RunImpulse()
    {
        StartCoroutine(MoveImpulse());
    }

    private IEnumerator MoveImpulse()
    { 
        while(Vector3.Distance(gameObject.transform.position, currentTarget.transform.position) >= 0.1)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, currentTarget.transform.position, Speed * Time.deltaTime);
            yield return null;
        }
        
        if(OnMarkerReached != null)
        {
            OnMarkerReached(currentTarget);
        }
    }
}
