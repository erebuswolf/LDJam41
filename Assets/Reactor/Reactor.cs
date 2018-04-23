using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reactor : MonoBehaviour {
    [SerializeField]
    private float health;

    private float startingHealth;

    [SerializeField]
    private AudioSource reactorExplosion;

    [SerializeField] GameObject ParticleExplosion;

    [SerializeField] private List< AudioSource> hitReactionSound;

    [SerializeField] private List<Transform> explosions = new List<Transform>();

    // Use this for initialization
    void Start () {
        startingHealth = health;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public float GetHealthRatio() {
        return health / startingHealth;
    }

    public float GetHealth() {
        return health;
    }

    IEnumerator ReactionSound() {
        if (health == 7) {
            yield return new WaitForSeconds(1);
            hitReactionSound[0].Play();
        } else if (health == 4) {
            hitReactionSound[1].Play();
        }
    }

    IEnumerator ReactionExplosion() {
        foreach(Transform t in explosions) {
            GameObject g = Instantiate(ParticleExplosion, t);
            g.transform.localPosition = Vector3.zero;
            yield return new WaitForSeconds(.2f);
        }
    }

    public void RecieveHit(float healthLoss) {
        health -= healthLoss;
        // A game manager or something else should query the health state of this object.
        // And manage game loss.
        reactorExplosion.Play();
        // Instantiate(ParticleExplosion, );
        StartCoroutine(ReactionSound());
        StartCoroutine(ReactionExplosion());
        if (health <= 0) {
            FindObjectOfType<EndGameIU>().Lose();
        }
    }
}
