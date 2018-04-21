using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    [SerializeField] private float speed;
    [SerializeField] private Reactor target;
    [SerializeField] private ParticleSystem particlesPrefab;
    [SerializeField] private int Damage;

    private void Update()
    {
        Vector3 toTargetDir = (target.gameObject.transform.position - transform.position).normalized;
        toTargetDir.y = 0;
        transform.position += speed * Time.deltaTime * toTargetDir;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target.gameObject)
        {
            var effect = GameObject.Instantiate(particlesPrefab, transform.position, particlesPrefab.transform.rotation);
            effect.GetComponent<ParticleSystem>().Play();
            Destroy(effect, 5.0f);
            Destroy(gameObject);
            target.RecieveHit(Damage);
        }
    }
}
