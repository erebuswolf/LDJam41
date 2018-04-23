using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    bool exploded;
    public void Explode() {
        exploded = true;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (exploded) {
            float y = Mathf.MoveTowards(this.transform.position.y, 0, 50);
            this.transform.position = new Vector3(this.transform.position.x, y, this.transform.position.z);
        }
    }
}
