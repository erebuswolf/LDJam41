using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footstepper : MonoBehaviour {
    [SerializeField] private AudioSource Footstep;
    private Shaker PlayerCam;
    // Use this for initialization
    void Start () {
        PlayerCam = FindObjectOfType<Shaker>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CheckPlayerCameraShake() {
        float dist = (PlayerCam.transform.position - this.transform.position).magnitude;
        if (dist < 500) {
            PlayerCam.Shake(2, .1f * Mathf.Clamp(500/ dist, 0, 20)/3f);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == 10) {
            Footstep.Play();
            CheckPlayerCameraShake();
        }
    }
}
