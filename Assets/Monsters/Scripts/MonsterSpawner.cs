using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour {

    [SerializeField]
    private List<Monster> MonsterPrefabs;

    // Number of monsters in each wave.
    [SerializeField]
    private int[] MonsterCounts;

    // Seconds Delay between monsters in each wave.
    [SerializeField]
    private float[] BetweenMonsterDelays;

    // Seconds Delay between waves.
    [SerializeField]
    private float[] BetweenWaveDelays;

    [SerializeField]
    private List<List<Monster>> Monsters = new List<List<Monster>>();

    [SerializeField]
    private int currentWave;

    [SerializeField]
    private Reactor target;

    [SerializeField]
    private AudioSource FirstWave;

    [SerializeField]
    private AudioSource FastWave;

    [SerializeField]
    private AudioSource AnotherWave;

    [SerializeField]
    private AudioSource AnotherWaveV2;

    [SerializeField]
    private AudioSource AntiairWave;

    [SerializeField]
    private AudioSource LastWave;

    [SerializeField]
    private AudioSource NoEndToThem;

    [SerializeField]
    private AudioSource NoEndToThemV2;
    
    [SerializeField]
    private Transform intermediateTarget;

    // Use this for initialization
    void Start() {
        currentWave = 0;
        SpawnAllMonsters();
        StartCoroutine(SpawnRoutine());
    }

    void SpawnAllMonsters() {
        Monsters.Clear();
        for (int i = 0; i < MonsterCounts.Length; i++) {
            Monsters.Add(new List<Monster>());
            for(int j = 0; j < MonsterCounts[i]; j++) {
                Monster monster = GameObject.Instantiate<Monster>(MonsterPrefabs[i]);
                monster.gameObject.transform.position = this.transform.position;
                monster.gameObject.SetActive(false);
                monster.SetTarget(target);  
                Monsters[i].Add(monster);
            }
        }
    }

    IEnumerator SpawnRoutine() {
        for (int waveNumber = 0; waveNumber < MonsterCounts.Length; waveNumber++) {
            if (waveNumber == 3) {
                if (Random.Range(0, 1) < .5) {
                    AnotherWave.Play();
                } else {
                    AnotherWaveV2.Play();
                }
            }

            // Wait the time between the waves
            if (waveNumber == 0) {
                yield return new WaitForSeconds(BetweenWaveDelays[waveNumber]);
            } else {
                yield return new WaitForSeconds(BetweenWaveDelays[waveNumber] / 2f);
            }

            switch(waveNumber) {
                case 0:
                    FirstWave.Play();
                    break;
                case 1:
                    if(Random.Range(0,1) <.5) {
                        AnotherWave.Play();
                    } else {
                        AnotherWaveV2.Play();
                    }
                    break;
                case 2:
                    AntiairWave.Play();
                    break;
                case 3:
                    LastWave.Play();
                    break;
                default:
                    break;
            }
            if (waveNumber != 0) {
                yield return new WaitForSeconds(BetweenWaveDelays[waveNumber] / 2f);
            }
            currentWave = waveNumber;

            for (int monsterNumber = 0; monsterNumber < MonsterCounts[waveNumber]; monsterNumber++) {
                // Wait the time between monsters
                yield return new WaitForSeconds(BetweenMonsterDelays[waveNumber]);
                Monsters[waveNumber][monsterNumber].StartMovement(intermediateTarget.position);
            }
            // don't spawn the next monster wave till the previous wave is eliminated.
            bool monstersAlive = true;
            while(monstersAlive) {
                foreach(Monster m in Monsters[waveNumber]) {
                    monstersAlive = false;
                    if (m.isAlive()) {
                        monstersAlive = true;
                        break;
                    }
                }
                yield return null;
            }
        }
        FindObjectOfType<EndGameIU>().Win();
    }

    public List<Monster> GetTargetableMonsters() {
        List<Monster> outputList = new List<Monster>();
        foreach (Monster m in Monsters[currentWave]) {
            if (m.gameObject.activeInHierarchy && m.isAlive()) {
                outputList.Add(m);
            }
        }
        return outputList;
    }
    
	// Update is called once per frame
	void Update () {
		
	}
}
