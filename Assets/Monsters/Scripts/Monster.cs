using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

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
    
    private void FixedUpdate()
    {
        UpdateMovement();
        UpdateSlow();
    }

    private void UpdateMovement() {
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

    public void ApplySlow() {
        Slowed = true;
        SlowedStartTime = Time.time;
    }

    public bool CanHit(ShootingMode type) {
        return DamageMultIndex[(int)type] > 0;
    }
    
    public void TakeDamage( ShootingMode type, int upgradeLevel, float impactDelay) {
        float damage = GetDamageValue(type, upgradeLevel);

        Health -= damage;
        if (Health <= 0) {
            DeathAnimation();
        }
    }

    public float GetDamageValue(ShootingMode type, int upgradeLevel) {
        return DamageMultIndex[(int)type] * upgradeLevel;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target.gameObject)
        {
            DeathAnimation();
            target.RecieveHit(Damage);
        }
    }

    private void DeathAnimation() {
        var effect = GameObject.Instantiate(particlesPrefab, transform.position, particlesPrefab.transform.rotation);
        effect.GetComponent<ParticleSystem>().Play();
        Destroy(effect, 5.0f);
        Destroy(gameObject);
    }
}
