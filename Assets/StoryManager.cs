using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour {
    [SerializeField] List<AudioSource> Sounds = new List<AudioSource>();
    // Use this for initialization
    void Start () {
        StartCoroutine(PlaySoundsInARow());
    }

    IEnumerator PlaySoundsInARow() {
        foreach (AudioSource s in Sounds) {
            s.Play();
            while (s.isPlaying) {
                yield return null;
            }
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
