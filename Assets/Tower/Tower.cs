using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShootingMode {
    ShootingModeNormal = 0,
    ShootingModeSlow,
    ShootingModeAntiAir
}

public class Tower : MonoBehaviour {
    private const int radiusBase = 150;
    private const int radiusSlow = 175;
    private const int radiusAntiAir = 200;

    private float [] ImpactDelay = new float[] {.3f, .3f, .3f};

    private float[] ShotDelay = new float[] { .3f, .3f, .3f };

    private float shootingRadius;
    private ShootingMode mode;
    private int[] upgradAmmount = new int[3];

    private bool shootingEnabled = false;

    [SerializeField] private List<Turret> TurretPrefabs;
    private List<Turret> Turrets = new List<Turret>();

    private MonsterSpawner monsterSpawner;

	// Use this for initialization
	void Start () {
        shootingRadius = radiusBase;
        mode = ShootingMode.ShootingModeNormal;
        shootingEnabled = true;
        for (int i = 0; i < upgradAmmount.Length; i++) {
            upgradAmmount[i] = 1;
        }

        monsterSpawner = FindObjectOfType<MonsterSpawner>();

        // create turrets
        for (int i = 0; i < Turrets.Count; i++) {
            Turret turret = GameObject.Instantiate<Turret>(TurretPrefabs[i]);
            turret.gameObject.transform.position = this.transform.position + new Vector3(10, 10, 10);
            Turrets.Add(turret);
        }

    }

    public void ChangeShootingMode(ShootingMode mode) {
        this.mode = mode;
        shootingEnabled = false;

        // animation to lower current type of turret and then raise another one
        // need to know current active turrent (equiv to curr mode)
        // turrents are associated with mode type

        shootingEnabled = true;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        Monster target = FindTarget();
        if (target) {
            ShootTarget(target);
        }
	}

    // see if any monsters are within radius
    // if one is, send notification to Monster class that that monster has been shot
    // run shoot animation towards target monster

    Monster FindTarget() {
        // Check to see if any monsters have entered the shooting radius
        // Finds closest monster. TODO: figure out what to do if positions are the same
        // Allowed to return nil
        List<Monster> monsters = monsterSpawner.GetTargetableMonsters();
        float bestDist = -1;
        Monster target = null;
        foreach(Monster m in monsters) {
            // Logic here to find best monster;
            Vector3 thisPosition = this.transform.position;
            thisPosition.y = 0;

            Vector3 monsterPosition = m.transform.position;
            monsterPosition.y = 0;
            float dist = (monsterPosition - thisPosition).magnitude;
            if (dist <= shootingRadius && bestDist == -1 || dist < bestDist) {
                target = m;
                bestDist = dist;
            }
        }
        if (bestDist == -1) {
            return null;
        }
        return target;
    }

    void ShootTarget(Monster Target) {
        
        if (Target.CanBeHit(mode) && shootingEnabled) {
            Target.TakeDamage(mode, upgradAmmount[(int)mode], ImpactDelay[(int)mode]);
        }
    }
}
