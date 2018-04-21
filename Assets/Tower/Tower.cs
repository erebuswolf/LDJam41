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
        
	}
}
