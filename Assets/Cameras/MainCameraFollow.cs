using UnityEngine;
using System.Collections;

public class MainCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothness;

    private Vector3 initialOffset;
    private Vector3 vel;

    void Start()
    {
        initialOffset = transform.position - target.position;
    }

    void Update()
    {
        var desiredPosition = target.position + target.TransformVector(initialOffset);
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref vel, smoothness);
        transform.LookAt(target, target.up);
    }
}