using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCar : MonoBehaviour {

    [SerializeField] private Rigidbody myBody;

    private float maxVel = 50.0f;
    private float maxAVel = 5.0f;

    [SerializeField] private float maxForwardSpeed = 90.0f;
    [SerializeField] private float maxReverseSpeed = 90.0f;

    [SerializeField] private float maxBrakingForce = 25.0f;
    [SerializeField] private float brakingSpeedCoeff = 1.0f;

    [SerializeField] private float airFrictionCoeff = 0.2f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        // Input & State
        float linearInput = Input.GetAxis("Vertical");
        float angularInput = Input.GetAxis("Horizontal");
        bool isHandbraking = Input.GetKey(KeyCode.Space);

        Vector3 localVel = transform.InverseTransformDirection(myBody.velocity);

        if (linearInput != 0) {
            //Debug.LogWarningFormat("vel mult {0} {1}", myBody.velocity.magnitude, 1 - (Mathf.Clamp(myBody.velocity.sqrMagnitude, 0, maxVel * maxVel) / (maxVel * maxVel)));
            myBody.AddRelativeForce(0, 0,40 * linearInput * (1 - (Mathf.Clamp(myBody.velocity.sqrMagnitude, 0, maxVel * maxVel) / (maxVel * maxVel))), ForceMode.Acceleration);
        }

        
        if (angularInput != 0) {
            /*if (localVel.z < 0) {
                angularInput *= -1;
            }*/
            //myBody.AddRelativeForce(5 * angularInput, 0, 0, ForceMode.Acceleration);
            //Debug.LogWarningFormat("angle mult {0} {1}", myBody.angularVelocity.magnitude, 1 - (Mathf.Clamp(myBody.velocity.sqrMagnitude, 0, maxVel * maxVel) / (maxVel * maxVel)));
            myBody.AddRelativeForce(40 * linearInput * (1 - (Mathf.Clamp(myBody.velocity.sqrMagnitude, 0, maxVel * maxVel) / (maxVel * maxVel))), 0,0, ForceMode.Acceleration);

            //myBody.AddRelativeTorque(new Vector3(0, angularInput * 15 * (1 - (Mathf.Clamp(myBody.angularVelocity.sqrMagnitude, 0, maxAVel * maxAVel) / (maxAVel * maxAVel))) , 0), ForceMode.Acceleration);
            //myBody.angularVelocity = ;
        }

        float dist = 10000;
        foreach (RaycastHit hit in Physics.RaycastAll(new Ray(this.transform.position, Vector3.down))) {
            if (hit.distance < dist) {
                dist = hit.distance;
            }
        }
        if (dist < 10000 && dist > 2) {
            myBody.AddRelativeForce(new Vector3(0 ,-3 *dist*dist, 0));
        }

        Vector3 newVel = localVel;

       // newVel.z += newVel.x * .6f;
        newVel.x = newVel.x * .2f;

        myBody.velocity = transform.localToWorldMatrix*newVel;
      //  myBody.velocity = myBody.velocity.normalized * Mathf.Clamp(myBody.velocity.magnitude, 0, 70);

    }
}
