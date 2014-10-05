using UnityEngine;
using System.Collections;

public class StartPathMarker : PathMarker
{
    [SerializeField]
    private PathMarker nextMarker = null;
    public PathMarker NextMarker { get { return nextMarker; } }

    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(gameObject.transform.position + new Vector3(0f, 0.5f, 0f), new Vector3(1f, 1f, 1f));
    }
}
