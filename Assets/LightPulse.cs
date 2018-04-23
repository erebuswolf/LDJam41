using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPulse : MonoBehaviour {
    [SerializeField] Light light;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator PulseRoutine() {
        while (light.intensity < 5) {
            light.intensity = Mathf.MoveTowards(light.intensity, 5, .5f);
            yield return null;
        }
        yield return new WaitForSeconds(.06f);
        while (light.intensity > 0) {
            light.intensity = Mathf.MoveTowards(light.intensity, 0, .2f);
            yield return null;
        }
    }

    public void Pulse () {
        StartCoroutine(PulseRoutine());
    }

    IEnumerator StrobeRoutine() {
        float startTime = Time.time;
        while (Time.time - startTime < 3) {
            light.intensity = 5;
            yield return new WaitForSeconds(.06f);
            light.intensity = 0;
            yield return new WaitForSeconds(.06f);
        }
        light.intensity = .97f;
    }
    public void Strobe() {

        StartCoroutine(StrobeRoutine());
    }

    public void StayOnAndFlicker() {

    }

}
