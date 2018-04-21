using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour {
    [SerializeField]
    GameObject Monster1;
    [SerializeField]
    GameObject Monster2;
    [SerializeField]
    GameObject Monster3;
    [SerializeField]
    GameObject Monster4;

    // Number of monsters in each wave.
    [SerializeField]
    int[] MonsterCounts;
    
    // Seconds Delay between monsters in each wave.
    [SerializeField]
    float[] BetweenMonsterDelays;

    // Seconds Delay between waves.
    [SerializeField]
    float[] BetweenWaveDelays;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
