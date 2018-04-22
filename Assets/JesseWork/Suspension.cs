using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompressionPoint {
    public float CompressionRatio;
    public Vector3 ImpactPoint;
    public Vector3 ImpactNormal;
    public CompressionPoint(float CompressionRatio, Vector3 ImpactPoint, Vector3 ImpactNormal) {
        this.CompressionRatio = CompressionRatio;
        this.ImpactPoint = ImpactPoint;
        this.ImpactNormal = ImpactNormal;
    }
}

public class Suspension : MonoBehaviour {

    [SerializeField]  Rigidbody myBody;

    [SerializeField] PlayerData playerData;
    
    [Header("Physics")]

    [SerializeField] private float SkidDamping = 40.0f;

    [SerializeField] private float SuspensionDist = 2.0f;
    [SerializeField] private float SpringConst = 1.0f;
    [SerializeField] private float MaxSpringForce = 40.0f;

    [SerializeField] private List<Transform> ShockLocations;

    [SerializeField] private float SideSuspensionDist = 2.0f;
    [SerializeField] private float SideSpringConst = 1.0f;
    [SerializeField] private float SideMaxSpringForce = 40.0f;

    [SerializeField] private float ForceMultiplier = 40f;
    [SerializeField] private float TorqMultiplier = 3f;
    [SerializeField] private float MinTorqSupresser = .7f;

    [SerializeField] private Transform LeftSideShock;
    [SerializeField] private Transform RightSideShock;
    
    private List<CompressionPoint> lastCompressionPoints = new List<CompressionPoint>();

    private float GetGroundCollision(Vector3 position, out CompressionPoint compressionPoint) {
        Vector3 collisionPoint = Vector3.zero;
        float minDistance = float.MaxValue;

        Vector3 localDown = transform.localToWorldMatrix * Vector3.down;
        Debug.DrawLine(position, position + SuspensionDist * localDown, Color.yellow);

        RaycastHit bestHit = new RaycastHit();
        foreach (var hit in Physics.RaycastAll(position, SuspensionDist * localDown, maxDistance: SuspensionDist)) {
            if (hit.collider.gameObject != gameObject && hit.collider.gameObject.layer != 8) {
                if (hit.distance < minDistance) {
                    bestHit = hit;
                    minDistance = hit.distance;
                }
            }
        }

        compressionPoint = new CompressionPoint(bestHit.distance / SuspensionDist, bestHit.point, bestHit.normal);
        return minDistance;
    }

    // Use this for initialization
    void Start () {
		
	}

    private float SideRayCast(Vector3 position, Vector3 direction) {
        float minDistance = float.MaxValue;
        foreach (var hit in Physics.RaycastAll(position, SideSuspensionDist * direction.normalized, maxDistance: SideSuspensionDist)) {
            if (hit.collider.gameObject != gameObject && hit.collider.gameObject.layer != 8) {
                if (hit.distance < minDistance) {
                    minDistance = hit.distance;
                }
            }
        }
        return minDistance;
    }

    private void UpdateSideShocks() {
        float leftDist = SideRayCast(LeftSideShock.position, transform.localToWorldMatrix * Vector3.left);
        float rightDist = SideRayCast(RightSideShock.position, transform.localToWorldMatrix * Vector3.right);

        // Add a force pointed right for the left spring force;
        float springForce = Mathf.Min(SideSpringConst / (leftDist * leftDist), SideMaxSpringForce);
        myBody.AddForceAtPosition(springForce * (transform.localToWorldMatrix * Vector3.right), LeftSideShock.position);

        Vector3 pointingRight = (transform.localToWorldMatrix * Vector3.right) * SideSuspensionDist;

        Vector3 pointingLeft = (transform.localToWorldMatrix * Vector3.left) * SideSuspensionDist;

        Debug.DrawLine(LeftSideShock.position, LeftSideShock.position + pointingLeft, Color.yellow);
        Debug.DrawLine(RightSideShock.position, RightSideShock.position + pointingRight, Color.yellow);

        // Add a force pointed left for the right spring force;
        springForce = Mathf.Min(SideSpringConst / (rightDist * rightDist), SideMaxSpringForce);
        myBody.AddForceAtPosition(springForce * (transform.localToWorldMatrix * Vector3.left), RightSideShock.position);
    }

    private void UpdateShocks() {
        lastCompressionPoints.Clear();
        foreach (Transform t in ShockLocations) {
            CompressionPoint cP;
            float dist = GetGroundCollision(t.position, out cP);
            if (dist == float.MaxValue) {
                continue;
            }
            lastCompressionPoints.Add(cP);
            float springForce = Mathf.Min(SpringConst / (dist * dist), MaxSpringForce);
            myBody.AddForceAtPosition(springForce * (transform.localToWorldMatrix * Vector3.up), t.position);
        }
    }

    private Vector3 CalcAvgNormal() {
        Vector3 avgNormal = Vector3.zero;
       // Vector3 avgPoint = Vector3.zero;

        foreach (CompressionPoint c in lastCompressionPoints) {
            avgNormal += c.ImpactNormal;
           // avgPoint += c.ImpactPoint;
        }
        avgNormal /= lastCompressionPoints.Count;
        //avgPoint /= lastCompressionPoints.Count;

        return avgNormal;
    }
	
    private void HandleInput() {
        // Input & State
        float linearInput = Input.GetAxis("Vertical");
        float angularInput = Input.GetAxis("Horizontal");
        bool isHandbraking = Input.GetKey(KeyCode.Space);

        Vector3 ForceVector = transform.localToWorldMatrix * Vector3.forward;
        ForceVector = Vector3.ProjectOnPlane(ForceVector, CalcAvgNormal());
        ForceVector.Normalize();

        Vector3 localVel = transform.worldToLocalMatrix * myBody.velocity;
        // If wheels aren't touching the ground we can't drive.
        bool applyNoForces = lastCompressionPoints.Count == 0;

        float force = ForceMultiplier * linearInput;
        ForceVector *= force;
        if (linearInput != 0 && !applyNoForces) {
            //Debug.LogWarningFormat("vel mult {0} {1}", myBody.velocity.magnitude, 1 - (Mathf.Clamp(myBody.velocity.sqrMagnitude, 0, maxVel * maxVel) / (maxVel * maxVel)));
            myBody.AddForce(ForceVector, ForceMode.Acceleration);
        }
        float normVel = Mathf.Abs(localVel.z / 50f);
        playerData.SetSpeedForUI(normVel + Random.Range(normVel-(normVel*.02f), normVel + (normVel * .02f))-normVel);

        if (angularInput != 0 && !applyNoForces) {
            if ((transform.worldToLocalMatrix * myBody.velocity).z < -5) {
                angularInput = -angularInput;
            }
            float torque = angularInput* TorqMultiplier;
            myBody.AddRelativeTorque(new Vector3(0, torque * Mathf.Max(MinTorqSupresser, 1/(1+ Mathf.Abs(localVel.z))), 0), ForceMode.Acceleration);
        }
        if (!applyNoForces) {
            Vector3 rightVel = localVel;
            rightVel.Scale(Vector3.right);
            myBody.AddRelativeForce(-rightVel * SkidDamping);
        }
    }

    private void OnCollisionEnter(Collision collision) {
      //  Debug.LogWarningFormat("impulse {0}", collision.impulse);

        foreach (ContactPoint contact in collision.contacts) {
            //print(contact.thisCollider.name + " hit " + contact.otherCollider.name);
           // Debug.DrawRay(contact.point, contact.normal, Color.white);
 //           myBody.AddForceAtPosition(contact.normal * collision.impulse)
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        UpdateShocks();
        //UpdateSideShocks();
        HandleInput();
    }
}
