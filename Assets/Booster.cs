using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour {
    [SerializeField] private Vector3 boostDirection;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    private void OnTriggerEnter(Collider other) {
        // If other is the player have them set their last spawn point.
        Suspension car = other.GetComponent<Suspension>();
        if (car != null) {

        }
    }
}
