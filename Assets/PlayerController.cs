using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public const float maxA = 20.0f;
    public const float minA = 12.0f;

    public const float maxV = 200.0f;
    public const float minV = 120.0f;

    public const float posFriction = 1.1f * (maxA / maxV);
    public const float negFriction = 1.1f * (minA / minV);
    public const float minDrag = 2.5f;

    public const float LowTurnAngularSpeed = 2.0f;
    public const float HighTurnAngularSpeed = 1.0f;
    public const float LowTurnSpeed = 20.0f;
    public const float HighTurnSpeed = 65.0f;
    public const float minSpeedForTurn = 5.0f;

    public const float minSkidSpeed = 35.0f;
    public const float skidCoeff = 9.0f;
    public const float skidEffectSec = 0.3f;

    public float a;
    public float v;

    public Vector3 prevForward;
    public bool didSkid;
    public float stopSkiddingTime;

    // Update is called once per frame
    void Update () {

        // Move
        var thrust = maxA * Input.GetAxis("Vertical");
        if (thrust < 0.0f)
        {
            thrust *= minA / maxA;
        }

        bool isBraking = (Input.GetKey(KeyCode.Space) || (v * thrust < 0.0f));
        
        var brake = -Mathf.Sign(v) * (Mathf.Max(Mathf.Abs(v / 1.0f), 30.0f)) * (isBraking ? 1.0f : 0.0f);
        var drag = -Mathf.Sign(v) * Mathf.Max((thrust == 0.0f)? minDrag :0.0f, ((v > 0.0f)? posFriction : negFriction) * Mathf.Abs(v));

        a = thrust + brake + drag;
        v = Mathf.Clamp(v + Time.deltaTime * a, -minV, maxV);

        transform.position += (v * Time.deltaTime + 0.5f * a * Time.deltaTime * Time.deltaTime) * transform.forward;

        // Turn
        var turn = Input.GetAxis("Horizontal");
        var turnCoeff = 0.0f;
        var absV = Mathf.Abs(v);
        if (absV < minSpeedForTurn)
        {
            turnCoeff = 0.0f;
        }
        else if ((minSpeedForTurn < absV) && (absV < LowTurnSpeed))
        {
            turnCoeff = LowTurnAngularSpeed * ((absV - minSpeedForTurn) / LowTurnSpeed);
        }
        else if ((LowTurnSpeed < absV) && (absV < HighTurnSpeed))
        {
            turnCoeff = LowTurnAngularSpeed + (HighTurnAngularSpeed - LowTurnAngularSpeed) * (absV - LowTurnSpeed) / (HighTurnSpeed - LowTurnAngularSpeed);
        }
        else
        {
            turnCoeff = HighTurnAngularSpeed;
        }
        transform.Rotate(new Vector3(0.0f, turnCoeff * turn, 0.0f));


        // Skid
        bool isTurningFast = (Mathf.Abs(turn) > 0.8f) && (Mathf.Abs(v) > 1.4f * HighTurnSpeed);
        var skid = !(isBraking || isTurningFast) ? 0.0f : Mathf.Max(0.0f, skidCoeff * (v - minSkidSpeed));
        var shouldApplySkidding = ((Time.time - stopSkiddingTime) < skidEffectSec);
        if (isBraking || isTurningFast)
        {
            if (!didSkid)
            {
                if (!shouldApplySkidding)
                {
                    prevForward = transform.forward;
                    didSkid = true;
                }
            }
            else
            {
                shouldApplySkidding = true;                
            }
        } else
        {
            if (didSkid)
            {
                stopSkiddingTime = Time.time;
            }
            didSkid = false;
        }

        if (shouldApplySkidding)
        {
            transform.position += (0.5f * skid * Time.deltaTime * Time.deltaTime) * prevForward;
        }
        Debug.Log(shouldApplySkidding + " " + skid);
    }
}
