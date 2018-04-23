using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesUI : MonoBehaviour {
    private PlayerData playerData;
    [SerializeField] private Text text;
	// Use this for initialization
	void Start () {
        playerData = FindObjectOfType<PlayerData>();

    }
	
	// Update is called once per frame
	void Update () {
        text.text = ""+playerData.GetResources();

    }
}
