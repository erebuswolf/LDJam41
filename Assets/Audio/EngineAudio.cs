using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class EngineAudio : MonoBehaviour
{
    [SerializeField] private AudioSource jetSound;

    [SerializeField] float LowPitch = .1f;
    [SerializeField] float HighPitch = 4.0f;
    [SerializeField] float SpeedToRevs = .1f;

    Vector3 myVelocity;
    Rigidbody carRigidbody;

    float startVolume;

    void Awake()
    {
        carRigidbody = GetComponent<Rigidbody>();
        startVolume = jetSound.volume;
    }

    float LastRev = 0;
    private void FixedUpdate()
    {
        myVelocity = carRigidbody.velocity;
        float forwardSpeed = transform.InverseTransformDirection(carRigidbody.velocity).z;
        float engineRevs = Mathf.Abs(forwardSpeed) * SpeedToRevs;
        if (Mathf.Abs(forwardSpeed) < .1) {
            jetSound.volume = 0;
        } else {
            jetSound.volume = startVolume;
        }
        engineRevs = Mathf.Clamp(engineRevs, LowPitch, HighPitch);

        LastRev = Mathf.MoveTowards(LastRev, engineRevs, .1f);
        jetSound.pitch = LastRev;
    }

}