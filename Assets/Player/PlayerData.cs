using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class to manage player information that should never be despawned or destroyed.
public class PlayerData : MonoBehaviour {
    [SerializeField]
    private SpawnPoint LastSpawnPoint;

    [SerializeField]
    private float resources;
    
    [SerializeField]
    private float speed;

    // Use this for initialization
    void Start() { 

    }

    public void AddResources(float resourcesAdded) {
        this.resources += resourcesAdded;
    }
    
    public float GetResources() {
        return resources;
    }

    // Returns a float 0-1 for the speed to set in the ui
    public float GetSpeedForUI() {
        return speed;
    }

    public void SpendResources(float resourcesSpent) {
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
