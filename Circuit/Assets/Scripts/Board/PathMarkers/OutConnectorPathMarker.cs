using UnityEngine;
using System.Collections;

public class OutConnectorPathMarker : PathMarker
{

    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(gameObject.transform.position + new Vector3(0f, 0.5f, 0f), new Vector3(1f, 1f, 1f));
        Gizmos.DrawIcon(gameObject.transform.position + new Vector3(0f, 1f, 0f), "OutIcon.png", true);
    }
}
