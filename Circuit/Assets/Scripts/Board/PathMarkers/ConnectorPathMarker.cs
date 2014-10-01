using UnityEngine;
using System.Collections;

public class ConnectorPathMarker : PathMarker
{

    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(gameObject.transform.position + new Vector3(0f, 0.5f, 0f), new Vector3(1f, 1f, 1f));
    }
}
