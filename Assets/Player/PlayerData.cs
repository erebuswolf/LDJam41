﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class to manage player information that should never be despawned or destroyed.
public class PlayerData : MonoBehaviour {
    [SerializeField]
    private SpawnPoint LastSpawnPoint;

    [SerializeField]
    private int resources;
    
    [SerializeField]
    private float speed;

    // Use this for initialization
    void Start() { 

    }

    public void AddResources(int resourcesAdded) {
        this.resources += resourcesAdded;
    }
    
    public int GetResources() {
        return resources;
    }

    public void SetSpeedForUI(float speed) {
        this.speed = speed;
    }

    // Returns a float 0-1 for the speed to set in the ui
    public float GetSpeedForUI() {
        return speed;
    }

    public void SpendResources(int resourcesSpent) {
        if (this.resources >= resourcesSpent) {
            this.resources -= resourcesSpent;
            return;
        }

        Debug.LogWarning("Spending Resources We don't have!!");
    }
	
    public void SetLastSpawnPoint(SpawnPoint Other) {
        LastSpawnPoint = Other;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
