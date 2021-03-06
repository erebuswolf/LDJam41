﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {
    
    [SerializeField] private bool Alive;
    [SerializeField] private float speed;
    [SerializeField] private Reactor target;
    [SerializeField] private ParticleSystem particlesPrefab;
    [SerializeField] private float Damage;
    [SerializeField] private float Health;
    [SerializeField] private bool Slowed;
    [SerializeField] private float SlowedStartTime;
    [SerializeField] private float SlowTimeLength;
    [SerializeField] private float SlowSpeedMult;
    [SerializeField] private float [] DamageMultIndex;
    [SerializeField] bool Rising;
    [SerializeField] private Transform HeadHeight;


    [SerializeField] private bool LastMonster;

    private Rigidbody myBody;
    private bool DealtDamage;
    private Animator animator;

    private void Start() {
        animator = GetComponentInChildren<Animator>();
        myBody = GetComponentInChildren<Rigidbody>();
    }

    private void FixedUpdate()
    {
        UpdateMovement();
        UpdateSlow();
    }

    public Vector3 GetHeadPosition() {
        return HeadHeight.position;
    }

    public void SetTarget(Reactor target) {
        this.target = target;
    }

    public void StartMovement(Vector3 intermediatTarget) {
        this.gameObject.SetActive(true);
        Rising = true;
        Alive = true;
        StartCoroutine(FakeRisingComplete(intermediatTarget));
    }

    // Coroutine to fake out the animation callback for the rising
    // animation completing.
    IEnumerator FakeRisingComplete(Vector3 intermediateTarget) {
        Vector3 toTargetDir = (intermediateTarget - transform.position);

        while(toTargetDir.magnitude > 1) {
            toTargetDir.Normalize();
            transform.position += speed * Time.deltaTime * toTargetDir * (Slowed ? SlowSpeedMult : 1f);
            yield return null;
            toTargetDir = (intermediateTarget - transform.position);
        }
        transform.position.Scale(new Vector3(1, 0, 1));
        Rising = false;
    }

    public void RisingComplete() {
        Rising = false;
    }

    private void UpdateMovement() {
        if(Rising || !Alive) {
            return;
        }

        Vector3 toTargetDir = (target.gameObject.transform.position - transform.position).normalized;
        toTargetDir.y = 0;
        toTargetDir.Normalize();
        transform.position += speed * Time.deltaTime * toTargetDir * (Slowed ? SlowSpeedMult : 1f);
    }

    private void UpdateSlow() {
        if (Slowed && Time.time - SlowedStartTime > SlowTimeLength) {
            Slowed = false;
        }
    }

    public bool isSlowed() {
        return Slowed;
    }

    public void ApplySlow(float mult) {
        Slowed = true;
        SlowSpeedMult = mult;
        SlowedStartTime = Time.time;
    }

    public bool isAlive() {
        return Alive;
    }

    public bool CanBeHit(ShootingMode type) {
        return DamageMultIndex[(int)type] > 0 && Alive;
    }
    
    public void TakeDamage( ShootingMode type, int upgradeLevel, float impactDelay) {
        float damage = GetDamageValue(type, upgradeLevel);

        Health -= damage;
        if (Health <= 0) {
            Alive = false;
            DeathAnimation(impactDelay);
        }
    }

    public float GetDamageValue(ShootingMode type, int upgradeLevel) {
        return DamageMultIndex[(int)type] * upgradeLevel;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (target == null) {
            Debug.LogWarning("TARGET NOT SET!!!");
            return;
        }
        if (other.gameObject == target.gameObject)
        {
            DeathAnimation();
            if (!DealtDamage) {
                DealtDamage = true;
                target.RecieveHit(Damage);
            }
        }
    }

    private void DeathAnimation(float delay) {
        StartCoroutine(DeathRoutine(delay));
    }

    IEnumerator DeathRoutine(float delay) {
        yield return new WaitForSeconds(delay);
        DeathAnimation();
        if (LastMonster) {
            yield break;
        }
        yield return new WaitForSeconds(4f);
        float startTime = Time.time;
        float dt = Time.time - startTime;
        Vector3 deadpos = this.transform.position;
        Vector3 goalpos = this.transform.position + Vector3.down*500;
        while (50 > dt) {
            dt = Time.time - startTime;
            this.transform.position = Vector3.Lerp(deadpos, goalpos, dt / 50f);
            yield return null;
        }
    }

    private void DeathAnimation() {
        if (animator == null ) {
            return;
        }
        Alive = false;
        animator.SetTrigger("AtReactor");
        //var effect = GameObject.Instantiate(particlesPrefab, transform.position, particlesPrefab.transform.rotation);
        //effect.GetComponent<ParticleSystem>().Play();
        //Destroy(effect, 5.0f);
        //gameObject.SetActive(false);
    }
}
