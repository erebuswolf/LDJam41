using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalAnim : MonoBehaviour {

    [SerializeField] private float floatAmp = 0.7f;
    [SerializeField] private float floatFreq = 0.3f;
    [SerializeField] private float spinFreq = 10.0f;
    [SerializeField] private float glowFreq = 0.5f;
    [SerializeField] private Color glowColor = new Color(155.0f / 255.0f, 155.0f / 255.0f, 110.0f / 255.0f); // faint dark-yellow

    private Material mat;
    private Vector3 originalPosition;
    private float tStart;

    private void Start()
    {
        mat = transform.GetChild(0).GetComponent<MeshRenderer>().material;
        originalPosition = transform.position + 1.5f * floatAmp * Vector3.up;
        tStart = Time.time;
    }

    private void Update ()
    {
        transform.position = originalPosition + floatAmp * Mathf.Sin(floatFreq * (Time.time - tStart)) * Vector3.up;
        transform.Rotate(Vector3.up, spinFreq * Time.deltaTime);
        mat.SetColor("_EmissionColor", Color.Lerp(Color.black, glowColor, Mathf.PingPong(glowFreq * Time.time, 1.0f)));
    }
}
