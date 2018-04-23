using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameIU : MonoBehaviour {
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject lossScreen;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Lose() {
        lossScreen.active = true;
    }

    public void Win() {
        winScreen.active = true;
    }

    public void PlayAgain() {
        SceneManager.LoadScene(0);
    }
}
