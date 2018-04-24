using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBulletAt : MonoBehaviour {

    IEnumerator ShootAtRoutine(Vector3 target) {
        while((this.transform.position - target).magnitude > 10) {
            yield return null;
            this.transform.position = Vector3.MoveTowards(this.transform.position, target, 10);
            this.transform.LookAt(target);
        }

        // Fire particle impact here.
        this.transform.localPosition = Vector3.zero;
        this.transform.localRotation = Quaternion.identity;
    }

    public void ShootAt(Vector3 target) {
        StopAllCoroutines();
        StartCoroutine(ShootAtRoutine(target));
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
