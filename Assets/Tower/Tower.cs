﻿using System.Collections;
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

    private float[] ImpactDelay = new float[] { .3f, .3f, .3f };

    private float[] ShotDelay = new float[] { 5f, 5f, 5f };

    private int[] ResourceCosts = new int[] { 1, 2, 3 };

    private int[] upgradeAmmount = new int[3] { 0, 0, 0 };
    private int upgradeMax = 3;

    private float lastTimeShot = 0;

    private float shootingRadius;
    private ShootingMode mode;

    private bool Activated;

    private bool CarInteracting;

    [SerializeField]
    private TurretController turret;


    private MonsterSpawner monsterSpawner;

    // Use this for initialization
    void Start() {
        shootingRadius = radiusBase;
        mode = ShootingMode.ShootingModeNormal;
        monsterSpawner = FindObjectOfType<MonsterSpawner>();

        //TODO: take this function call out so they start out disabled;
        ActivateTower();
    }

    public void ChangeShootingMode(ShootingMode mode) {
        this.mode = mode;
        turret.SwitchShootingModeAnimation(mode);
    }

    void Update() {
        HandleCarInteractions();
    }

    // Update is called once per frame
    void FixedUpdate() {
        Monster target = FindTarget();
        Turret t = turret.getActiveTurret();
        if (t == null) {
            return;
        }

        t.TrackTarget(target);
        if (target) {
            ShootTarget(target);
        }
    }

    public void ActivateTower() {
        Activated = true;
    }

    Monster FindTarget() {
        List<Monster> monsters = monsterSpawner.GetTargetableMonsters();
        float bestDist = -1;
        Monster target = null;
        foreach (Monster m in monsters) {
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
        Turret t = turret.getActiveTurret();

        if (Time.time - lastTimeShot < ShotDelay[(int)mode] ||
            !Target.CanBeHit(mode) || turret.isSwitching() || t == null) {
            return;
        }
        
        t.ShootTargetAnimation(mode);
        Target.TakeDamage(mode, upgradeAmmount[(int)mode], ImpactDelay[(int)mode]);
        lastTimeShot = Time.time;
    }

    private void OnTriggerEnter(Collider other) {
        PlayerData data = other.GetComponent<PlayerData>();
        if (data) {
            CarInteracting = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        PlayerData data = other.GetComponent<PlayerData>();
        
        if (data) {
            CarInteracting = false;
        }
    }

    private bool isShiftHeld() {
        return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    }

    private void BuyUpgrade (ShootingMode modeToUpgrade) {
        PlayerData data = FindObjectOfType<PlayerData>();
        int cost = ResourceCosts[(int)modeToUpgrade];
        int resources = data.GetResources();
        if ((resources >= cost) && (upgradeAmmount[(int)modeToUpgrade] < upgradeMax)) {
            upgradeAmmount[(int)modeToUpgrade]++;
            data.SpendResources(cost);
        }
    }

    private void HandleCarInteractions() {
        if (!CarInteracting) {
            return;
        }

        ShootingMode modeToModify;

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            modeToModify = ShootingMode.ShootingModeNormal;
            if (isShiftHeld()) {
                BuyUpgrade(modeToModify);
            } else {
                ChangeShootingMode(modeToModify);
            }
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            modeToModify = ShootingMode.ShootingModeSlow;
            if (isShiftHeld()) {
                BuyUpgrade(modeToModify);
            } else {
                ChangeShootingMode(modeToModify);
            }
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            modeToModify = ShootingMode.ShootingModeAntiAir;
            if (isShiftHeld()) {
                BuyUpgrade(modeToModify);
            } else {
                ChangeShootingMode(modeToModify);
            }
        }
    }
}
