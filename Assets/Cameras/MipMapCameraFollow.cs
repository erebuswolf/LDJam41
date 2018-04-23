using UnityEngine;
using System.Collections;

public class MipMapCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    private Vector3 initialOffset;

    void Start()
    {
        initialOffset = transform.position - target.position;
    }

    void FixedUpdate()
    {
        var desiredPosition = target.position + initialOffset;
        transform.position = desiredPosition;
    }
}