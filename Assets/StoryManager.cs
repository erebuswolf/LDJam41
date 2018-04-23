using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour {
    [SerializeField] List<AudioSource> Sounds = new List<AudioSource>();
    [SerializeField] Explosion explosion;
    [SerializeField] LightPulse lightPulse;

    [SerializeField] AudioSource IntroMusic;
    [SerializeField] AudioSource MusicLoop;


    IEnumerator MusicRoutine() {
        yield return new WaitForSeconds(32);
        IntroMusic.Play();
        MusicLoop.PlayScheduled(AudioSettings.dspTime + IntroMusic.clip.length);

    }

    int pulseCount = 0;
    // Use this for initialization
    void Start () {
        StartCoroutine(PlaySoundsInARow());
        StartCoroutine(MusicRoutine());
    }

    IEnumerator PlaySoundsInARow() {
        yield return new WaitForSeconds(1);
        foreach (AudioSource s in Sounds) {
            s.Play();
            if (s == Sounds[0]) {
                lightPulse.Pulse();
                pulseCount++;
            }

            while (s.isPlaying) {
                if (s == Sounds[0]) {

                    if (s.time > 6.76) {
                        explosion.Explode();
                        if (pulseCount == 3) {
                            lightPulse.Strobe();
                            pulseCount++;
                        }
                    }

                    if (s.time > 2.3 && pulseCount == 1) {
                        lightPulse.Pulse();
                        pulseCount++;
                    }
                    if (s.time > 4.4 && pulseCount == 2) {
                        lightPulse.Pulse();
                        pulseCount++;
                    }
                }
                yield return null;
            }
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
