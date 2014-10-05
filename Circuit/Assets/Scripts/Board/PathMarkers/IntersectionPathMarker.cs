using UnityEngine;
using System.Collections;

public class IntersectionPathMarker : PathMarker
{
    [SerializeField]
    private PathMarker[] nextMarkers;

    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(gameObject.transform.position + new Vector3(0f, 0.5f, 0f), new Vector3(1f, 1f, 1f));
    }
}
