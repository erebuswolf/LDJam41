using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    private bool switching;

	// Use this for initialization
	void Start () {
        switching = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SwitchShootingModeAnimation() {
        if (switching) {
            return;
        }
        switching = true;
        StartCoroutine(SwitchAnimation());
    }

    IEnumerator SwitchAnimation () {
        Vector3 raisedPos = this.gameObject.transform.localPosition;
        Vector3 loweredPos = new Vector3(0, 0, 0); // Move it down
        float speed = 2;
        float delta = 0;
        Debug.LogFormat("Delta: {0}", delta);

        Vector3 move;
        // Lower the turret
        while (delta < 1)
        {
            delta += speed * Time.deltaTime;
            delta = Mathf.Clamp01(delta);

            move = Vector3.MoveTowards(raisedPos, loweredPos, delta);
            this.gameObject.transform.localPosition = move;
            yield return null;
        }

        yield return new WaitForSeconds(.5f);

        // Raise the turret back up
        while (delta > 0)
        {
            delta -= speed * Time.deltaTime;
            delta = Mathf.Clamp01(delta);
            move = Vector3.MoveTowards(raisedPos, loweredPos, delta);
            this.gameObject.transform.localPosition = move;
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
