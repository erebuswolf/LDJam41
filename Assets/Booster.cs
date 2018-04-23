using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour {
    [SerializeField] private Vector3 boostDirection;

    [SerializeField] private float boostMag;
    
    [SerializeField] private bool FixedBoost;
    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    private void OnTriggerStay(Collider other) {
        // If other is the player have them set their last spawn point.
        Suspension car = other.GetComponent<Suspension>();
        if (car != null) {
            Debug.LogWarning("car applying boost");
            if(FixedBoost) {
                car.ApplyFixedBoost(boostDirection.normalized, boostMag);
            } else {
                car.ApplyFixedBoost(boostMag);
            }
        }
    }
}
