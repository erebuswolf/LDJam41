using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour {

    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float shakeMagnitude = 2.0f;

                            
    private IEnumerator ShakeCoroutine()
    {
        float elapsed = 0.0f;
        float orginalZ = transform.eulerAngles.z;

        while (elapsed < shakeDuration)
        {
            float z = Random.Range(-1f, 1f) * shakeMagnitude;
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, orginalZ + z);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            //CameraShaker.Instance.ShakeOnce(2.0f, 1.0f, 0.01f, 0.1f);
            StartCoroutine(ShakeCoroutine());
        }
    }
}
