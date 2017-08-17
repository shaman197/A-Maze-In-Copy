using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ItemController : MonoBehaviour {

    private static ItemController instance = null;
    public static ItemController Instance {
        get
        {
            return instance;
        }
    }

    public Transform rangerBadges;
    public Transform bamboos;

    //public Text displayRangerBadges;

    public int collectedRangerBadges = 0;

    private int maxRangerBadges = 10;

	private void Awake() {
		if (instance != null && instance != this) {
			Debug.Log ("Error - Er zijn op dit moment meerdere instanties van ItemController (No Singleton)");
		} 
		else {
			instance = this;
		}
	}

    private void Start() {
		if (DataHolder.Instance != null) {
			maxRangerBadges = DataHolder.Instance.maxCoins;
		}

		if (rangerBadges != null) {
			StartCoroutine (DefineRangerBadgeTrigger ());
		}

		if (bamboos != null) {
			StartCoroutine (DefineBambooTrigger ());
		}
	}

    private IEnumerator DefineRangerBadgeTrigger() {

		foreach (Transform child in rangerBadges) {
			BaseItem badge = child.transform.GetComponent<BaseItem> ();
			badge.TriggerEnterDelegate += (Collider c) => {
				AddRangerBadge();
                AudioController.Instance.PlaySound(Sounds.BadgeFound);

                if (MissionController.Instance != null)
                {
                    StartCoroutine(MissionController.Instance.PickedUpRangerBadge());
                }
            };
		}

		yield return null;
	}

    private IEnumerator DefineBambooTrigger() {

		foreach (Transform child in bamboos) {
			BaseItem bamboo = child.transform.GetComponent<BaseItem> ();
			bamboo.TriggerEnterDelegate += (Collider c) => {
				EnergyBarController.Instance.AddEnergy (EnergyBarController.Instance.amountOfEnergyForBamboo);
                AudioController.Instance.PlaySound(Sounds.BambooFound);

                if (MissionController.Instance != null)
                {
                    StartCoroutine(MissionController.Instance.PickedUpBambooStick());
                }
            };
		}

		yield return null;
	}

    public void AddRangerBadge() {
        collectedRangerBadges++;
	}

    public void SetCollectedRangerBadge(int rangerBadges) {
        collectedRangerBadges = rangerBadges;
    }

    public int GetCollectedRangerBadge() {
        return collectedRangerBadges;
    }

    public int GetMaxRangerBadge() {
        return maxRangerBadges;
    }

    private void UpdatePowerUp(int amount, Text itemLabel) {
        itemLabel.text = amount.ToString();
    }
}
