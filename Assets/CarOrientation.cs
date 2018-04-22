using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarOrientation : MonoBehaviour {

    [SerializeField] private float carLength = 4.0f;
    [SerializeField] private float carWidth = 2.0f;
    [SerializeField] private float carHeight= 1.0f;

    [SerializeField] private float rayLength = 10.0f;

    [SerializeField] private float epsilon = 1.0f;

    private Vector3 rearLeft;
    private Vector3 rearRight;
    private Vector3 frontLeft;
    private Vector3 frontRight;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (rearLeft != Vector3.zero) Gizmos.DrawWireSphere(rearLeft, 0.2f);
        if (rearRight != Vector3.zero) Gizmos.DrawWireSphere(rearRight, 0.2f);
        if (frontLeft != Vector3.zero) Gizmos.DrawWireSphere(frontLeft, 0.2f);
        if (frontRight != Vector3.zero) Gizmos.DrawWireSphere(frontRight, 0.2f);
    }

    private bool GetGroundCollision(Vector3 position, out Vector3 collisionPoint)
    {
        RaycastHit hit;        
        Debug.DrawLine(position + carHeight * Vector3.up, position + rayLength * Vector3.down, Color.yellow);
        if (Physics.Raycast(position + carHeight * Vector3.up, rayLength * Vector3.down , out hit, maxDistance: rayLength))
        {
            collisionPoint = hit.point;
            return true;
        }
        collisionPoint = Vector3.zero;
        return false;
    }

    private void OrientAndSnapToGround(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 anchorP3)
    {
        Vector3 normal = Vector3.Cross(p1 - p3, p2 - p3);
        Debug.LogWarningFormat("normal {0}", normal);
        Debug.DrawRay((p1 + p2 + p3) / 3.0f, normal, Color.cyan);
        if (Vector3.Dot(Vector3.up, normal) > 0.0f)
        {
            transform.position = transform.position + (p3 - anchorP3) + epsilon * Vector3.up;
            transform.rotation = Quaternion.FromToRotation(transform.up, normal) * transform.rotation;
        }
    }

    private void FixedUpdate () {

        var zDelta = carLength / 2.0f;
        var yDelta = carHeight / 2.0f;
        var xDelta = carWidth / 2.0f;

        Vector3 rearLeftPosition = transform.position + transform.TransformVector(new Vector3(-xDelta, -yDelta, -zDelta));
        bool isRearLeft = GetGroundCollision(rearLeftPosition, out rearLeft);
        Vector3 rearRightPosition = transform.position + transform.TransformVector(new Vector3(-xDelta, -yDelta, zDelta));
        bool isRearRight = GetGroundCollision(rearRightPosition, out rearRight);
        Vector3 frontLeftPosition = transform.position + transform.TransformVector(new Vector3(xDelta, -yDelta, -zDelta));
        bool isFrontLeft = GetGroundCollision(frontLeftPosition, out frontLeft);
        Vector3 frontRightPosition = transform.position + transform.TransformVector(new Vector3(xDelta, -yDelta, zDelta));
        bool isFrontRight = GetGroundCollision(frontRightPosition, out frontRight);

        if (isRearLeft && isRearRight && isFrontLeft)
        {
            OrientAndSnapToGround(rearLeft, rearRight, frontLeft, frontLeftPosition);
        }
        else if (isFrontLeft && isFrontRight && isRearRight)
        {
            OrientAndSnapToGround(rearLeft, frontRight, frontLeft, frontLeftPosition);
        }
    }
}
