using UnityEngine;
using System.Collections;

public class BallShadow : MonoBehaviour
{
    public GameObject Ball { get; set; }

    [SerializeField]
    private float xMinBoundary = 0f;
    [SerializeField]
    private float zMinBoundary = 0f;
    [SerializeField]
    private float xMaxBoundary = 60f;
    [SerializeField]
    private float zMaxBoundary = 40f;

    Vector2 center;

    [SerializeField]
    private float maxOffset = 0.3f;

    [SerializeField]
    private float Yoffset = -0.5f;

    private void Start()
    {
        center = new Vector2((xMaxBoundary - xMinBoundary) / 2f, (zMaxBoundary - zMinBoundary) / 2f);
    }

    private void Update()
    {
        if (Ball != null)
        {
            Vector2 offsetDirection = new Vector2(Ball.transform.position.x, Ball.transform.position.z) - center;
            offsetDirection.Normalize();

            Vector2 offset = offsetDirection * maxOffset;

            gameObject.transform.position = Ball.transform.position + new Vector3(offset.x, Yoffset, offset.y);
        }
    }
}
