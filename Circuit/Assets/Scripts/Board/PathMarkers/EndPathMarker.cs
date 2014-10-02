using UnityEngine;
using System.Collections;

public class EndPathMarker : PathMarker
{
    [SerializeField]
    private Directions.Direction exitDirection = Directions.Direction.NORTH;

    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(gameObject.transform.position + new Vector3(0f, 0.5f, 0f), new Vector3(1f, 1f, 1f));
    }
}
