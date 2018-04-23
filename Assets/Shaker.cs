using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour {

    private float orginalZ;

    private void Start() {
        orginalZ = transform.eulerAngles.z;
    }

    public void Shake(float duration, float mag) {
        StopAllCoroutines();
        StartCoroutine(ShakeCoroutine(duration ,mag));
    }

    public IEnumerator ShakeCoroutine(float shakeDuration, float shakeMagnitude)
    {
        float elapsed = 0.0f;
        while (elapsed < shakeDuration)
        {
            float z = Random.Range(-1f, 1f) * shakeMagnitude;
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, orginalZ + z);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
