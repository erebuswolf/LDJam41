using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    private bool switching;

    [SerializeField] private Monster trackedMonster;
    private const float lookSpeed = 300f;

    private Quaternion startRotation;

    private bool Activated;

    [SerializeField] AudioSource LaserShot;

	// Use this for initialization
	void Start () {
        startRotation = transform.rotation;
        switching = false;
        StartCoroutine(TrackMonster());
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

    public bool isSwitching() {
        return switching;
    }

    public void ActivateTurret() {
        Activated = true;
    }

    IEnumerator SwitchAnimation () {
        Vector3 raisedPos = this.gameObject.transform.position;
        Vector3 loweredPos = this.gameObject.transform.position + Vector3.down * 30; // Move it down
        float speed = 2;
        float delta = 0;
        Debug.LogFormat("Delta: {0}", delta);

        Vector3 move;
        // Lower the turret
        while (delta < 1)
        {
            delta += speed * Time.deltaTime;
            delta = Mathf.Clamp01(delta);

            move = Vector3.Lerp(raisedPos, loweredPos, delta);
            Debug.LogFormat("Delta: {0}, {1}, {2}, {3}", move , delta, raisedPos, loweredPos);
            this.gameObject.transform.position = move;
            yield return null;
        }

        yield return new WaitForSeconds(.5f);

        // Raise the turret back up
        while (delta > 0)
        {
            delta -= speed * Time.deltaTime;
            delta = Mathf.Clamp01(delta);
            move = Vector3.Lerp(raisedPos, loweredPos, delta);
            this.gameObject.transform.position = move;
            yield return null;
        }

        switching = false;
    }

    IEnumerator TrackMonster() {
        while(true) {
            float speed = Time.deltaTime * lookSpeed;
            Quaternion goalRotation = startRotation;
            Quaternion currentRotation = gameObject.transform.rotation;
            if (trackedMonster == null || !trackedMonster.isAlive() || !Activated) {
            } else {
                goalRotation = Quaternion.LookRotation(
                    trackedMonster.GetHeadPosition()
                    - gameObject.transform.position);

            }
            gameObject.transform.rotation = Quaternion.RotateTowards(currentRotation, goalRotation, speed);
            yield return null;
        }
    }

    public void TurnTurretAnimation () {
        // Animate to turn at a 90 degree angle
    }

    public void TrackTarget (Monster target) {
        trackedMonster = target;
    }

    public void ShootTargetAnimation(ShootingMode mode) {
        LaserShot.Play();
    }
}
