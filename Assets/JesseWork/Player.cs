using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField]
    private int lives;
    [SerializeField]
    private bool dead;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKey(KeyCode.Space)) {
            gameObject.transform.position = gameObject.transform.position + new Vector3(1, 0, 0);
        }

    }
}
