using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBarController : MonoBehaviour {

	private static EnergyBarController instance = null;
	public static EnergyBarController Instance {
		get {
			return instance;
		}
	}

	public Text energyLevelText;
	public Slider slider;
    public int amountOfEnergyForBamboo = 5;
    public int addEnergyOnWait = 1;
    //public int decreaseEnergyPerDistance = 1;

    private void Awake() {
		if (instance != null && instance != this) {
			Debug.Log ("Error - Er zijn op dit moment meerdere instanties van EnergyBarController (No Singleton)");
		}
		else {
			instance = this;
		}
	}

	private void Start() {
		EnergyUpdate ();
	}

    public void EnergyUpdate() {
		energyLevelText.text = (int)slider.value + " / 100";

        if(MissionController.Instance != null) {
            MissionController.Instance.CheckEnergyLevelAboveLevel((int)slider.value);
        }
    }

	public void AddEnergy(int amount) {
        slider.value += amount;
        EnergyUpdate();
    }

	public void DecreaseEnergy(float amount) {
		slider.value -= amount;
        EnergyUpdate();
    }

    public float GetEnergyValue() {
        return slider.value;
    }
}
