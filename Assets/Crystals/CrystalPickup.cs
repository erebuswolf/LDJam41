using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalPickup : MonoBehaviour {

    [SerializeField]
    private int Value;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        // If other is the player have them pick up the crystal.
        PlayerData data = other.GetComponent<PlayerData>();
        if (data != null) {
            data.AddResources(Value);
            Debug.LogFormat("Player now has {0} crystals", data.GetResources());
        }
    }

	private void OnTriggerExit(Collider other)
	{
		// Crystal should disappear once you leave it
	}
}
