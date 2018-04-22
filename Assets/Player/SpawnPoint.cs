using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        // If other is the player have them set their last spawn point.
        PlayerData data = other.GetComponent<PlayerData>();
        if (data != null) {
            data.SetLastSpawnPoint(this);
        }
    }
}
