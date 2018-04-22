using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    private bool switching;

	// Use this for initialization
	void Start () {
        this.gameObject.SetActive(true);
        switching = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SwitchShootingModeAnimation () {
        switching = true;
        StartCoroutine(SwitchAnimation());
    }

    IEnumerator SwitchAnimation () {
        Vector3 raisedPos = transform.InverseTransformPoint(this.gameObject.transform.position);
        Vector3 loweredPos = new Vector3(0, 0, 0); // Move it down
        float speed = 2;
        float delta = speed * Time.deltaTime;
        Debug.LogFormat("Delta: {0}", delta);

        Vector3 move;
        // Lower the turret
        while (delta < 1)
        {
            move = Vector3.MoveTowards(raisedPos, loweredPos, delta);
            this.gameObject.transform.position = transform.TransformPoint(move);
            yield return null;
        }

        //yield return new WaitForSeconds(3);

        // Raise the turret back up
        while (delta < 1)
        {
            move = Vector3.MoveTowards(loweredPos, raisedPos, delta);
            this.gameObject.transform.position = transform.TransformPoint(move);
            yield return null;
        }

        switching = false;
    }

    public void TurnTurretAnimation () {
        // Animate to turn at a 90 degree angle
    }

    public void TrackTarget (Monster target) {
        
    }

    public void ShootTargetAnimation(ShootingMode mode) {
        
    }
}
