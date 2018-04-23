using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShootingMode {
    ShootingModeNormal = 0,
    ShootingModeSlow,
    ShootingModeAntiAir
}

public class Tower : MonoBehaviour {
    private const int radiusBase = 180;
    private const int radiusSlow = 150;
    private const int radiusAntiAir = 300;

    private float[] ImpactDelay = new float[] { .01f, .01f, .01f };

    private float[] ShotDelay = new float[] { 5f, 2f, 3f };

    private int[] ResourceCosts = new int[] { 100, 300, 500 };

    private int[] upgradeAmmount = new int[3] { 1, 1, 1 };
    private int upgradeMax = 4;

    private float lastTimeShot = 0;

    private bool Activated = false;

    private float shootingRadius;
    private ShootingMode mode;

    private TurretController turretController;

    private TowerInterface towerInterface;

    private MonsterSpawner monsterSpawner;

    [SerializeField] private AudioSource upgradeSound;
    [SerializeField] private AudioSource interactSound;
    [SerializeField] private AudioSource leaveSound;

    // Use this for initialization
    void Start() {
        shootingRadius = radiusBase;
        mode = ShootingMode.ShootingModeNormal;
        monsterSpawner = FindObjectOfType<MonsterSpawner>();
        towerInterface = FindObjectOfType<TowerInterface>();
        turretController = GetComponentInChildren<TurretController>();
    }

    public bool getActivated() {
        return Activated;
    }

    public int[] GetResourceCosts() {
        return ResourceCosts;
    }

    public void ChangeShootingMode(ShootingMode mode) {
        this.mode = mode;
        if (!Activated) {
            Activated = true;
        }
        turretController.SwitchShootingModeAnimation(mode);
    }

    public int[] GetUpgradeStatus() {
        return upgradeAmmount;
    }
    
    void Update() {
    }

    // Update is called once per frame
    void FixedUpdate() {
        Monster target = FindTarget();
        Turret t = turretController.getActiveTurret();
        if (t == null) {
            return;
        }

        t.TrackTarget(target);
        if (target) {
            ShootTarget(target);
        }
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
            if (dist <= shootingRadius) {

                if (mode == ShootingMode.ShootingModeSlow && m.isSlowed()) {
                    dist += 500;
                }
                if(bestDist == -1 || dist < bestDist) {
                    target = m;
                    bestDist = dist;
                }
            }
        }
        if (bestDist == -1) {
            return null;
        }
        return target;
    }

    public bool NoActiveTurret() {
        return turretController.getActiveTurret() == null;
    }

    public ShootingMode GetShootingMode() {
        return mode;
    }

    void ShootTarget(Monster Target) {
        Turret t = turretController.getActiveTurret();

        if (Time.time - lastTimeShot < ShotDelay[(int)mode] ||
            !Target.CanBeHit(mode) || turretController.isSwitching() || t == null) {
            return;
        }
        
        t.ShootTargetAnimation(mode);
        if (mode == ShootingMode.ShootingModeSlow) {
            float slowMult = .75f;
            switch (upgradeAmmount[(int)ShootingMode.ShootingModeSlow]) {
                case 1:
                    slowMult = .5f;
                    break;
                case 2:
                    slowMult = .3f;
                    break;
                case 3:
                    slowMult = .2f;
                    break;
                case 4:
                    slowMult = .1f;
                    break;
            }
            Target.ApplySlow(slowMult);
        }
        Target.TakeDamage(mode, upgradeAmmount[(int)mode], ImpactDelay[(int)mode]);
        lastTimeShot = Time.time;
    }

    private void OnTriggerEnter(Collider other) {
        PlayerData data = other.GetComponent<PlayerData>();
        if (data) {
            towerInterface.SetActiveTower(this);
            interactSound.Play();
        }
    }

    private void OnTriggerExit(Collider other) {
        PlayerData data = other.GetComponent<PlayerData>();
        
        if (data) {
            towerInterface.SetActiveTower(null);
            leaveSound.Play();
        }
    }
    
    public void BuyUpgrade (ShootingMode modeToUpgrade) {
        PlayerData data = FindObjectOfType<PlayerData>();
        if (upgradeAmmount[(int)modeToUpgrade] >= upgradeMax) {
            return;
        }
        
        int cost = ResourceCosts[upgradeAmmount[(int)modeToUpgrade] - 1];
        int resources = data.GetResources();
        if (resources >= cost) {
            upgradeAmmount[(int)modeToUpgrade]++;
            data.SpendResources(cost);
            upgradeSound.Play();
            //   Debug.LogFormat("Player bought upgrade for {0} crystals and upgrade amt is now {1}", cost, upgradeAmmount[(int)modeToUpgrade]);
        } else {
           // Debug.LogFormat("Player did not have enough money, cost {0} only had {1}", cost, resources);

        }
    }
}
