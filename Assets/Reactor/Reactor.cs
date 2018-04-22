﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reactor : MonoBehaviour {
    [SerializeField]
    private float health;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public float GetHealth() {
        return health;
    }

    public void RecieveHit(float healthLoss) {
        health -= healthLoss;
        // A game manager or something else should query the health state of this object.
        // And manage game loss.
        if (health <= 0) {
            Debug.LogWarning("Game is lost!");
        }
    }
}
