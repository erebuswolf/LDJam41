using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour {
    [SerializeField] private Vector3 boostDirection;

    [SerializeField] private float boostMag;
    
    [SerializeField] private bool FixedBoost;


    [SerializeField] private bool ForceMiddle;

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
                car.ApplyBoost(boostMag);
            }
            if (ForceMiddle) {
                float xOffset = (transform.worldToLocalMatrix * car.gameObject.transform.position).x;
                car.ApplyFixedBoost(transform.localToWorldMatrix * Vector3.right, (boostMag / 100) * (xOffset / 10));
            }
        }
    }
}
