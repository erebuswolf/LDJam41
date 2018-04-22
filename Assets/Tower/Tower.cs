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
    private int[] upgradeAmmount = new int[3];

    private bool shootingEnabled = false;

    private Turret turret;

    private MonsterSpawner monsterSpawner;

	// Use this for initialization
	void Start () {
        shootingRadius = radiusBase;
        mode = ShootingMode.ShootingModeNormal;
        shootingEnabled = true;
        for (int i = 0; i < upgradeAmmount.Length; i++) {
            upgradeAmmount[i] = 1;
        }
        turret = FindObjectOfType<Turret>();
        monsterSpawner = FindObjectOfType<MonsterSpawner>();
    }

    public void ChangeShootingMode(ShootingMode mode) {
        this.mode = mode;
        shootingEnabled = false;
        turret.SwitchShootingModeAnimation();
        shootingEnabled = true;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        Monster target = FindTarget();
        if (target) {
            turret.TrackTarget(target);
            ShootTarget(target);
        }
	}

    Monster FindTarget() {
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
            turret.ShootTargetAnimation(mode);
            Target.TakeDamage(mode, upgradeAmmount[(int)mode], ImpactDelay[(int)mode]);
        }
    }
}
