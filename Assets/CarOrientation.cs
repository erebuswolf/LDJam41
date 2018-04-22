using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarOrientation : MonoBehaviour {

    [Header("Car Dimensions")]
    [SerializeField] private float carLength = 4.0f;
    [SerializeField] private float carWidth = 2.0f;
    [SerializeField] private float carHeight= 1.0f;

    [Header("Physics")]
    [SerializeField] private float rayLength = 5.0f;
    [SerializeField] private float floatDistance = 1.0f;

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
        collisionPoint = Vector3.zero;
        float minDistance = float.MaxValue;

        Debug.DrawLine(position + carHeight * Vector3.up, position + rayLength * Vector3.down, Color.yellow);
        foreach (var hit in Physics.RaycastAll(position + carHeight * Vector3.up, rayLength * Vector3.down, maxDistance: rayLength))
        {
            if (hit.collider.gameObject != gameObject)
            {
                if (hit.distance <  minDistance)
                {
                    collisionPoint = hit.point;
                    minDistance = hit.distance;
                }
            }
        }
        return (collisionPoint != Vector3.zero);
    }

    // TODO: move to util extension class
    private static int BoolToInt(bool b)
    {
        return b ? 1 : 0;
    }

    private void UseGround()
    {

        Debug.DrawLine(transform.position + carHeight * Vector3.up, transform.position + rayLength * Vector3.down, Color.yellow);
        foreach (var hit in Physics.RaycastAll(transform.position + carHeight * Vector3.up, rayLength * Vector3.down, maxDistance: rayLength))
        {
            if (hit.collider.gameObject != gameObject)
            {
                transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
                transform.position = hit.point + floatDistance * Vector3.up;
                return;
            }
        }
        transform.rotation = Quaternion.FromToRotation(transform.up, Vector3.up) * transform.rotation;
    }

    private void OrientAndSnapToGround(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 anchorP3)
    {
        Vector3 normal = Vector3.Cross(p1 - p3, p2 - p3);
        Debug.DrawRay((p1 + p2 + p3) / 3.0f, normal, Color.cyan);
        if (Vector3.Dot(Vector3.up, normal) < 0.0f)
        {
            normal *= -1.0f;
        }

        transform.position = transform.position + (p3 - anchorP3) + floatDistance * Vector3.up;
        if (Vector3.Dot(transform.up, normal) > Mathf.Cos(45.0f * Mathf.Deg2Rad))
        {
            transform.rotation = Quaternion.FromToRotation(transform.up, normal) * transform.rotation;
        }
    }
    
    private float fallSpeed = 0.0f;

    private void FixedUpdate() {

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
            return;
        }
        if (isFrontLeft && isFrontRight && isRearRight)
        {
            OrientAndSnapToGround(rearLeft, frontRight, frontLeft, frontLeftPosition);
            return;
        }

        if (BoolToInt(isRearLeft) + BoolToInt(isRearRight) + BoolToInt(isFrontLeft) + BoolToInt(isFrontRight) < 3)
        {
            fallSpeed += 40.0f * Time.deltaTime;
        } else
        {
            fallSpeed = 0.0f;
        }

        transform.position += fallSpeed * Time.deltaTime * Vector3.down;
        UseGround();
    }
}
