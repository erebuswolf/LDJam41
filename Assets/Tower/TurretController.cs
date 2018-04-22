using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ControllerState { IDLE = 0, RAISING, LOWERING};
public class TurretController : MonoBehaviour {

    private bool switching;
    
    [SerializeField] private List<Turret> turrets;
    
    [SerializeField] private float stowTurretHeight;

    [SerializeField] private int activeTurretIndex;
    [SerializeField] private int nextTurretIndex;

    [SerializeField] private ControllerState controllerState;

    float speed = 40;

    // Use this for initialization
    void Start () {
        switching = false;
        activeTurretIndex = -1;
        controllerState = ControllerState.IDLE;
    }
	
    private void loweringTurret() {
        if(activeTurretIndex == -1) {
            return;
        }
        turrets[activeTurretIndex].Lower();
        Vector3 startPos = turrets[activeTurretIndex].gameObject.transform.localPosition;
        Vector3 endPos = startPos;
        endPos.y = stowTurretHeight;
        
        float moveDst = speed * Time.deltaTime;

        Vector3 move = Vector3.MoveTowards(startPos, endPos, moveDst);
        turrets[activeTurretIndex].gameObject.transform.localPosition = move;
        
        if (Mathf.Approximately(0, (move - endPos).sqrMagnitude)) {
            // We have reached our goal state
            controllerState = ControllerState.RAISING;
            activeTurretIndex = nextTurretIndex;
            nextTurretIndex = -1;
        } else {

        }
    }

    private void raisingTurret() {
        Vector3 startPos = turrets[activeTurretIndex].gameObject.transform.localPosition;
        Vector3 endPos = startPos;
        endPos.y = 0;

        Vector3 move;
        float moveDst = speed * Time.deltaTime;

        move = Vector3.MoveTowards(startPos, endPos, moveDst);
        turrets[activeTurretIndex].gameObject.transform.localPosition = move;
        
        if (Mathf.Approximately(0, (move - endPos).sqrMagnitude)) {
            // We have reached our goal state
            controllerState = ControllerState.IDLE;
            turrets[activeTurretIndex].Raised();
            switching = false;
        } else {

        }
    }

    // Update is called once per frame
    void Update () {
        switch (controllerState) {
            case ControllerState.LOWERING:
                loweringTurret();
                break;
            case ControllerState.RAISING:
                raisingTurret();
                break;
            case ControllerState.IDLE:
                switching = false;
                break;
        }
    }

    public Turret getActiveTurret() {
        if (switching || activeTurretIndex == -1) {
            return null;
        }
        return turrets[activeTurretIndex];
    }

    public void SwitchShootingModeAnimation(ShootingMode mode) {
        if ((int)mode == activeTurretIndex) {
            return;
        }

        switching = true;
        if (activeTurretIndex == -1) {
            activeTurretIndex = (int)mode;
            controllerState = ControllerState.RAISING;
        } else {
            nextTurretIndex = (int)mode;
            switch (controllerState) {
                case ControllerState.RAISING:
                case ControllerState.IDLE:
                    controllerState = ControllerState.LOWERING;
                    break;
                default:
                    break;
            }
        }
    }

    public bool isSwitching() {
        return switching;
    }
    
}
