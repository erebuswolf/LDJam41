using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShitPlayer : MonoBehaviour {
    [SerializeField] private Rigidbody myBody;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        myBody.AddForce( new Vector3(Input.GetAxis("Horizontal")*10,
            0, Input.GetAxis("Vertical") * 10));
        

    }
}
