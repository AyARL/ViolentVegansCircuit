using UnityEngine;
using System.Collections;

public class StartPathMarker : PathMarker
{

    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(gameObject.transform.position, new Vector3(1f, 1f, 1f));
    }
}
