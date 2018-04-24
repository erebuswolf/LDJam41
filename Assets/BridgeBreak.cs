using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeBreak : MonoBehaviour {

    [SerializeField] List<GameObject> BridgePieces = new List<GameObject>();

    [SerializeField] GameObject hiddenBridge;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer == 9) {
            BreakBridge();
        }
    }

    public void BreakBridge() {
        hiddenBridge.active = true;
        foreach(GameObject g in BridgePieces) {
            g.active = false;
        }
    }
}
