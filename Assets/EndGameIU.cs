using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameIU : MonoBehaviour {
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject lossScreen;

    [SerializeField] private AudioSource winSound;
    [SerializeField] private AudioSource loseSound;

    bool lost = false;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Lose() {
        lost = true;
        lossScreen.active = true;
        loseSound.Play();
    }

    IEnumerator winRoutine() {
        yield return new WaitForSeconds(.02f);
        if (lost) {
            yield break;
        }
        winScreen.active = true;
        winSound.Play();
    }

    public void Win() {
        StartCoroutine(winRoutine());
    }

    public void PlayAgain() {
        SceneManager.LoadScene(0);
    }
}
