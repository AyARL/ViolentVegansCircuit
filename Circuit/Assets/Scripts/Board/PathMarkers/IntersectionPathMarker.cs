using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IntersectionPathMarker : PathMarker
{
    [SerializeField]
    private PathMarker[] nextMarkers;
    public IEnumerable<PathMarker> IntersectionExits { get { return nextMarkers; } }

    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(gameObject.transform.position + new Vector3(0f, 0.5f, 0f), new Vector3(1f, 1f, 1f));
    }
}
