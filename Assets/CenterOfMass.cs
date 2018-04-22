using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterOfMass : MonoBehaviour {
    [SerializeField]
    private Rigidbody myRigidbody;

    [SerializeField]
    private Vector3 centerOfMass;

	// Use this for initialization
	void Start () {
        myRigidbody.centerOfMass = centerOfMass;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
