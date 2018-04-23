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

    private float[] ImpactDelay = new float[] { .3f, .3f, .3f };

    private float[] ShotDelay = new float[] { 5f, 5f, 3f };

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

    [SerializeField] private AudioSource ChargeUpSound;

    private bool chargePlayed = false;

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
            StartCoroutine(PlayChargeUp(0f));
            Activated = true;
        }
        turretController.SwitchShootingModeAnimation(mode);
    }

    public int[] GetUpgradeStatus() {
        return upgradeAmmount;
    }

    IEnumerator PlayChargeUp(float delay) {
        yield return new WaitForSeconds(delay);
        ChargeUpSound.Play();
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
        Target.TakeDamage(mode, upgradeAmmount[(int)mode], ImpactDelay[(int)mode]);
        chargePlayed = false;
        StartCoroutine(PlayChargeUp(3f));
        lastTimeShot = Time.time;
    }

    private void OnTriggerEnter(Collider other) {
        PlayerData data = other.GetComponent<PlayerData>();
        if (data) {
            towerInterface.SetActiveTower(this);
        }
    }

    private void OnTriggerExit(Collider other) {
        PlayerData data = other.GetComponent<PlayerData>();
        
        if (data) {
            towerInterface.SetActiveTower(null);
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
            Debug.LogFormat("Player bought upgrade for {0} crystals and upgrade amt is now {1}", cost, upgradeAmmount[(int)modeToUpgrade]);
        } else {
            Debug.LogFormat("Player did not have enough money, cost {0} only had {1}", cost, resources);

        }
    }
}
