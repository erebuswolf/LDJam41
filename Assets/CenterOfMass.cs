using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterOfMass : MonoBehaviour {
    [SerializeField]
    Rigidbody myRigidbody;

    [SerializeField]
    Vector3 centerOfMass;

	// Use this for initialization
	void Start () {
        myRigidbody.centerOfMass = centerOfMass;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
