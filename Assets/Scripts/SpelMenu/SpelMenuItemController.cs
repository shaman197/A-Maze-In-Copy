using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpelMenuItemController : MonoBehaviour {

	private static SpelMenuItemController instance = null;
	public static SpelMenuItemController Instance {
		get {
			return instance;
		}
	}

    public static string SuperBadge = "SuperBadge";
    public static string RangerBadge = "RangerBadge";

    public Text superBadgeText;
    public Text rangerBadgeText;
    public int amountOfSuperBadges = 0;
	public int amountOfRangerBadges = 0;

	private void Awake() {
		if (instance != null && instance != this) {
			Debug.Log ("Error - Er zijn op dit moment meerdere instanties van SpelMenuItemController (No Singleton)");
		}
		else {
			instance = this;
		}
    }

	private void Start() {
		if (PlayerPrefs.HasKey(RangerBadge))
			amountOfRangerBadges =  PlayerPrefs.GetInt(RangerBadge);

		rangerBadgeText.text = amountOfRangerBadges.ToString();
	}

	public int GetAmountOfSuperBadges() {
		return amountOfSuperBadges;
	}
		
	public int GetAmountOfRangerBadges() {
		return amountOfRangerBadges;
	}

    public void AddAmountOfRangerBadges(int amount) {
        amountOfRangerBadges = amountOfRangerBadges + amount;
        rangerBadgeText.text = amountOfRangerBadges.ToString();
        PlayerPrefs.SetInt(RangerBadge, amountOfRangerBadges);
    }

    public void SubtractAmountOfRangerBadges(int amount) {
        amountOfRangerBadges = amountOfRangerBadges - amount;
        rangerBadgeText.text = amountOfRangerBadges.ToString();
        PlayerPrefs.SetInt(RangerBadge, amountOfRangerBadges);
    }
}
