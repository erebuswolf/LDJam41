using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("Linear Movement - Thrust + Braking")]

    [SerializeField] private float maxForwardAcceleration = 25.0f;
    [SerializeField] private float maxReverseAcceleration = 20.0f;

    [SerializeField] private float maxForwardSpeed = 120.0f;
    [SerializeField] private float maxReverseSpeed = 90.0f;

    [SerializeField] private float maxBrakingForce = 30.0f;
    [SerializeField] private float brakingSpeedCoeff = 1.0f;

    [SerializeField] private float airFrictionCoeff = 0.2f;

    private float forwardAirFriction;
    private float reverseAirFriction;

    [Header("Angular Movement - Steering")]

    // More steering control when speed is lower
    [SerializeField] private float LowTurnAngularSpeed = 2.3f;
    [SerializeField] private float HighTurnAngularSpeed = 0.8f;

    // Defines the shape of the steering control graph
    [SerializeField] private float deadTurnSpeed = 5.0f;
    [SerializeField] private float LowTurnSpeed = 15.0f;
    [SerializeField] private float HighTurnSpeed = 40.0f;

    [Header("Angular Movement - Steering")]

    [SerializeField] private float minSkidSpeed = 45.0f; // Higher than HighTurnSpeed
    [SerializeField] private float skidSpeedCoeff = 20.0f;
    [SerializeField] private float skiddingDurationSec = 3.0f;

    // Skid effect across frames
    private float startToSkidTime;
    private Vector3 skidDir;
    private bool didSkid;

    #region Speedometer & Physics Simulation

    public float acceleration { get; private set; }
    public float speed { get; private set; }

    private float GetBrakingForce(bool isBraking)
    {
        return BoolToFloat(isBraking) * (-Mathf.Sign(speed) * (Mathf.Max(Mathf.Abs(speed / brakingSpeedCoeff), maxBrakingForce)));
    }

    #endregion

    // TODO: move to util extension class
    private static float BoolToFloat(bool b)
    {
        return b ? 1.0f : 0.0f;
    }

    // TODO: move to util extension class
    private static bool FloatToBool(float f)
    {
        return !Mathf.Approximately(0.0f, f);
    }

    private void FixedUpdate ()
    {
        // Input & State
        float linearInput = Input.GetAxis("Vertical");
        float angularInput = Input.GetAxis("Horizontal");
        bool isHandbraking = Input.GetKey(KeyCode.Space);

        bool isBraking = (isHandbraking || (speed * linearInput < 0.0f));
        bool isTurningFast = (Mathf.Abs(angularInput) > 0.75f) && (Mathf.Abs(speed) > minSkidSpeed);
        bool isSkidding = isBraking || isTurningFast;

        float absSpeed = Mathf.Abs(speed);
        float dt = Time.deltaTime;
        float sqrdt = dt * dt;

        // Linear Movement
        float thrust = linearInput * ((linearInput >= 0.0f) ? maxForwardAcceleration : maxReverseAcceleration);
        float brake =  GetBrakingForce(isBraking);
        float drag = -airFrictionCoeff * speed;

        // Angular Movement
        var turnCoeff = 0.0f;
        if (absSpeed < deadTurnSpeed)
        {
            turnCoeff = 0.0f;
        }
        else if ((deadTurnSpeed < absSpeed) && (absSpeed < LowTurnSpeed))
        {
            turnCoeff = LowTurnAngularSpeed * ((absSpeed - deadTurnSpeed) / LowTurnSpeed);
        }
        else if ((LowTurnSpeed < absSpeed) && (absSpeed < HighTurnSpeed))
        {
            turnCoeff = LowTurnAngularSpeed + (HighTurnAngularSpeed - LowTurnAngularSpeed) * (absSpeed - LowTurnSpeed) / (HighTurnSpeed - LowTurnAngularSpeed);
        }
        else
        {
            turnCoeff = HighTurnAngularSpeed;
        }

        // Skid Movement       
        float skid = 0.0f;
        if (isSkidding && !didSkid)
        {
            skidDir = Mathf.Sign(speed) * transform.forward;
            startToSkidTime = Time.time;
            didSkid = true;

        }
        if ((Time.time - startToSkidTime) > skiddingDurationSec)
        {
            didSkid = false;
        }
        
        if (didSkid) // True also if skidding right now
        {
            skid = Mathf.Max(0.0f, skidSpeedCoeff * (absSpeed - minSkidSpeed));
        }

        // Update
        acceleration = thrust + brake + drag;
        speed = Mathf.Clamp(speed + dt * acceleration, -maxReverseSpeed, maxForwardSpeed);
        transform.position += (speed * dt + 0.5f * acceleration * sqrdt) * transform.forward + (0.5f * skid * sqrdt) * skidDir;
        transform.Rotate(new Vector3(0.0f, turnCoeff * angularInput, 0.0f));
    }
}
