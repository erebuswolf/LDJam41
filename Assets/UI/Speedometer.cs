using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour {
    private PlayerData data;

    [SerializeField] Image needle;
    float minRot = 94.05f;
    float maxRot = -90.01f;
    // Use this for initialization
    void Start () {
        data = FindObjectOfType<PlayerData>();
	}
	
	// Update is called once per frame
	void Update () {
        float speed = data.GetSpeedForUI();
        if (speed > 1 || speed < 0) {
            Debug.LogWarning("INVALIDE SPEED GIVEN TO UI");
        }
        float rot = minRot - (minRot - maxRot) * speed;
        needle.gameObject.transform.localRotation =  Quaternion.Euler(0,0,rot);
	}
}
