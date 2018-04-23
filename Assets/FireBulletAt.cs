using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBulletAt : MonoBehaviour {

    IEnumerator ShootAtRoutine(Transform target) {
        while((this.transform.position - target.transform.position).magnitude > 10) {
            yield return null;
            this.transform.position = Vector3.MoveTowards(this.transform.position, target.transform.position, 10);
            this.transform.LookAt(target.transform.position);
        }

        // Fire particle impact here.
        this.transform.localPosition = Vector3.zero;
    }

    public void ShootAt(Transform target) {
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
