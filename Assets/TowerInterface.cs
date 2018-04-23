using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerInterface : MonoBehaviour {

    [SerializeField] private List<Text> ActiveTexts = new List<Text>();
    [SerializeField] private List<Text> LevelTexts = new List<Text>();
    [SerializeField] private List<Button> Buttons = new List<Button>();

    [SerializeField] private GameObject Menu;

    private Tower activeTower;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (activeTower == null) {
            Menu.active = false;
        } else {
            Menu.active = true;
            SetActiveTexts();
            SetLevelTexts();

            int keyIndex = 0;
            if (Input.GetKey(KeyCode.LeftShift)|| Input.GetKey(KeyCode.RightShift)) {
                keyIndex += 3;
            }
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                Buttons[keyIndex].onClick.Invoke();
            } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
                Buttons[keyIndex + 1].onClick.Invoke();
            } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
                Buttons[keyIndex + 2].onClick.Invoke();
            }
        }
	}

    private void SetActiveTexts() {
        if (activeTower == null) {
            return;
        }
        foreach (Text t in ActiveTexts) {
            t.text = "INACTIVE";
        }
        if (!activeTower.getActivated()) {
            return;
        }
        string Mode = "ACTIVE";
        if (activeTower.NoActiveTurret()) {
            Mode = "RAISING";
        }
        ActiveTexts[(int)activeTower.GetShootingMode()].text = Mode;
    }

    private void SetLevelTexts() {
        if (activeTower == null) {
            return;
        }
        int[] upgradeStatus = activeTower.GetUpgradeStatus();
        int[] costs = activeTower.GetResourceCosts();
        for (int i = 0; i < LevelTexts.Count; i++) {
            string cost = "-";
            if (upgradeStatus[i] <4) {
                cost = costs[upgradeStatus[i]-1]+"";
            }
            LevelTexts[i].text = cost + " CR\nLevel\n" + (upgradeStatus[i] < 4 ? ""+(upgradeStatus[i]) : "MAX");
        }
    }

    public void SetActiveTower(Tower tower) {
        this.activeTower = tower;
    }

    public void ActivateGun(int mode) {
        if (activeTower != null) {
            activeTower.ChangeShootingMode((ShootingMode)mode);
        }
    }

    public void UpgradeGun(int mode) {
        if (activeTower != null) {
            activeTower.BuyUpgrade((ShootingMode)mode);
        }
    }
}
