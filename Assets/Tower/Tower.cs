using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShootingMode {
    ShootingModeNormal = 0,
    ShootingModeSlow,
    ShootingModeAntiAir
}

public class Tower : MonoBehaviour {

    private const int radiusBase = 150;
    private const int radiusSlow = 175;
    private const int radiusAntiAir = 200;

    private int shootingRadius;
    private ShootingMode mode;

	// Use this for initialization
	void Start () {
        shootingRadius = radiusBase;
        mode = ShootingMode.ShootingModeNormal;
	}
	
	// Update is called once per frame
	void Update () {
        // Call FindTarget
	}

    // see if any monsters are within radius
    // if one is, send notification to Monster class that that monster has been shot
    // run shoot animation towards target monster

    Monster FindTarget() {
        // Check to see if any monsters have entered the shooting radius
        // Finds closest monster. TODO: figure out what to do if positions are the same
        // Allowed to return nil
    }
}

/* Placeholder code until we have a real Monster class */
public class Monster {
}
