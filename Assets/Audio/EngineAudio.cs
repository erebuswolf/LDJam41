using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class EngineAudio : MonoBehaviour
{
    [SerializeField] private AudioSource jetSound;

    [SerializeField] float LowPitch = .2f;
    [SerializeField] float HighPitch = 2.0f;
    [SerializeField] float SpeedToRevs = .1f;

    Vector3 myVelocity;
    Rigidbody carRigidbody;

    void Awake()
    {
        carRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        myVelocity = carRigidbody.velocity;
        float forwardSpeed = transform.InverseTransformDirection(carRigidbody.velocity).z;
        float engineRevs = Mathf.Abs(forwardSpeed) * SpeedToRevs;
        jetSound.pitch = Mathf.Clamp(engineRevs, LowPitch, HighPitch);
    }

}