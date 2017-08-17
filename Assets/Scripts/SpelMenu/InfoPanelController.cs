using Lean.Localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanelController : MonoBehaviour {

	private static InfoPanelController instance = null;
	public static InfoPanelController Instance {
		get {
			return instance;
		}
	}

	public LeanLocalizedTextArgs groundTileNameText;
	public Text groundTileCoinsText;
	public Text groundTileTimerText;

    public SuperBadge finish;
    public SuperBadge time;
    public SuperBadge grabCoins;

	public Sprite noneMedal;
	public Sprite goldMedal;

	public Transform medalMission1;
	private Image img1;
	public GameObject medalMission2;
	private Image img2;
	public GameObject medalMission3;
	private Image img3;

	public Animation anim;

    private GroundTile groundTile;

    private void Awake() {
		if (instance != null && instance != this) {
			Debug.Log ("Error - Er zijn op dit moment meerdere instanties van InfoPanelController (No Singleton)");
		}
		else {
			instance = this;
		}

        img1 = medalMission1.GetComponent<Image>();
        img2 = medalMission2.GetComponent<Image>();
        img3 = medalMission3.GetComponent<Image>();
    }

	private void Start() {
		ShowEarnedMedals(TileManager.Instance.getTileById(DataHolder.Instance.tileId));

		anim = transform.GetComponent<Animation> ();
	}

	public void StartLevelChangeAnimation() {
		anim.Play ();
	}

	public void ShowEarnedMedals(GroundTile ground) {
		groundTile = ground;
		
		groundTileNameText.PhraseName = ground.levelKey;

		Achivement achievedMissions = ground.achivements;

		int counter = 0;
		if (achievedMissions.mission1_accomplished == true) {
			counter++;
		}
		if (achievedMissions.mission2_accomplished == true) {
			counter++;
		}
		if (achievedMissions.mission3_accomplished == true) {
			counter++;
		}

		ShowAmountOfBadges (counter);
	}

	private void ShowAmountOfBadges(int amount) {
		if (amount >= 1) {
			SwitchMedalGold (img1);
		} else {
			SwitchMedalNone (img1);
		}
		if (amount >= 2) {
			SwitchMedalGold (img2);
		} else {
			SwitchMedalNone (img2);
		}
		if (amount == 3) {
			SwitchMedalGold (img3);
		} else {
			SwitchMedalNone (img3);
		}
	}

	private void SwitchMedalGold (Image img){
		img.color = new Color(1f, 1f, 1f, 1f);
		img.sprite = goldMedal;
	}

	private void SwitchMedalNone (Image img){
		img.color = new Color(1f, 1f, 1f, 0.15f);
		img.sprite = noneMedal;
	}

//	public void SetNewFloorInfo(GroundTile ground) {
//		groundTile = ground;
//
//		groundTileNameText.text = ground.levelName;
//
//        if (ground.achivements != null)
//        {
//			if (ground.achivements.mission1_accomplished)
//                finish.SwitchToGold();
//            else
//                finish.SwitchToNone();
//
//			if (ground.achivements.mission2_accomplished)
//                time.SwitchToGold();
//            else
//                time.SwitchToNone();
//
//			if (ground.achivements.mission3_accomplished)
//                grabCoins.SwitchToGold();
//            else
//                grabCoins.SwitchToNone();
//        }
//
//        else
//        {
//            finish.SwitchToNone();
//            time.SwitchToNone();
//            grabCoins.SwitchToNone();
//        }
//	}

	public int GetStoryId() {
		return groundTile.storyId;
	}
}