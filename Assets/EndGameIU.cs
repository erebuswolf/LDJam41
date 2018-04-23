using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameIU : MonoBehaviour {
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject lossScreen;

    [SerializeField] private AudioSource winSound;
    [SerializeField] private AudioSource loseSound;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Lose() {
        lossScreen.active = true;
        loseSound.Play();
    }

    public void Win() {
        winScreen.active = true;
        winSound.Play();
    }

    public void PlayAgain() {
        SceneManager.LoadScene(0);
    }
}
