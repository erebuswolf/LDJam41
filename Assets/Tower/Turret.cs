using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    [SerializeField] private Monster trackedMonster;
    private const float lookSpeed = 600f;
    [SerializeField] private bool lowered;

    [SerializeField] AudioSource LaserShot;
    FireBulletAt bullet;

    // Use this for initialization
    void Start () {
        bullet = GetComponentInChildren<FireBulletAt>();
        StartCoroutine(UpdateLookAtRoutine());
        lowered = true;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Lower() {
        lowered = true;
    }

    public void Raised() {  
        lowered = false;
    }

    IEnumerator UpdateLookAtRoutine() {
        while(true) {
            float speed = Time.deltaTime * lookSpeed;
            Quaternion goalRotation = (gameObject.transform.position.x > 0) ? Quaternion.Euler(0, -90, 0) : Quaternion.Euler(0, 90, 0);
            Quaternion currentRotation = gameObject.transform.rotation;
            if (trackedMonster == null || !trackedMonster.isAlive() || lowered) {
                if (lowered) {
                    goalRotation = Quaternion.Euler(-90,0,0);
                }
            } else {
                goalRotation = Quaternion.LookRotation(
                    trackedMonster.GetHeadPosition()
                    - gameObject.transform.position);
            }
            gameObject.transform.rotation = Quaternion.RotateTowards(currentRotation, goalRotation, speed);
            yield return null;
        }
    }

    public void TrackTarget (Monster target) {
        trackedMonster = target;
    }

    public void ShootTargetAnimation(ShootingMode mode) {
        bullet.ShootAt(trackedMonster.GetHeadPosition());
        LaserShot.Play();
    }
}
