using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishedLevelManager : MonoBehaviour {

    public EndLevelController endLevelController;
    //public float achivementTime = 60.0f;
    public ItemController itemController;

    //public Text timeLimitText;
    //public Text maxCoinsText;
//
//    public SuperBadge finish;
//    public SuperBadge time;
//    public SuperBadge grabBadges;

	public Sprite noneMedal;
	public Sprite goldMedal;

	public Transform medalMission1;
	private Image img1;
	public GameObject medalMission2;
	private Image img2;
	public GameObject medalMission3;
	private Image img3;

	public Sprite checkBoxWithMark;
	public Sprite emptyCheckBox;

	public Text mission1Text;
	public Text mission2Text;
	public Text mission3Text;

	public Image mission1Box;
	public Image mission2Box;
	public Image mission3Box;

    public int tutorialNumber = 0;

    private Achivement achivement;

    private void Start() {
		if (DataHolder.Instance != null) {
			achivement = DataHolder.Instance.achivement;
			achivement.mission1_accomplished = false;
			achivement.mission2_accomplished = false;
			achivement.mission3_accomplished = false;
		} 
		else {
			achivement = new Achivement ();
		}
			
		img1 = medalMission1.GetComponent<Image>();
		img2 = medalMission2.GetComponent<Image>();
		img3 = medalMission3.GetComponent<Image>();
	}

    private void OnCollisionEnter(Collision collision) {
		if (collision.gameObject == InGamePlayerController.Instance.gameObject) {
			AudioController.Instance.PlaySound (Sounds.FinishLevel);
			PauseGameController.Instance.FreezeGame ();
			MissionController.Instance.PlayerReachedFinish ();

			endLevelController.ShowFinished (); // show UI

			bool[] missionResults = MissionController.Instance.CheckForAccomplishedMissions ();

			// Fill medals on endUI
			ShowEarnedMedals (missionResults);

			// Place checkMarks at accomplished missions
			ShowCheckMarks(missionResults);

			// Set description of missions
			MissionController.Instance.ShowAllMissionTexts();

			if (missionResults[0]) {
				achivement.mission1_accomplished = true;
			}
			if (missionResults[1]) {
				achivement.mission2_accomplished = true;
			}
			if (missionResults[2]) {
				achivement.mission3_accomplished = true;
			}

			DataHolder.Instance.SetCoins (ItemController.Instance.GetCollectedRangerBadge ());
			DataHolder.Instance.SetTime (TimerController.Instance.timeText.text);
			DataHolder.Instance.SetAchivements (achivement);
			DataHolder.Instance.setFinished (true);
			DataHolder.Instance.SetShowMenuTutorial (tutorialNumber);
		}
    }

	private void ShowCheckMarks(bool[] missionResults) {
		if (missionResults[0]) {
			mission1Box.sprite = checkBoxWithMark;
		} 
//		else {
//			mission1Box.sprite = emptyCheckBox;
//		}

		if (missionResults[1]) {
			mission2Box.sprite = checkBoxWithMark;
		} 
//		else {
//			mission2Box.sprite = emptyCheckBox;
//		}

		if (missionResults[2]) {
			mission3Box.sprite = checkBoxWithMark;
		} 
//		else {
//			mission3Box.sprite = emptyCheckBox;
//		}
	}
		
	private void ShowEarnedMedals(bool[] missionResults) {
		int amountOfAccomplishedMissions = 0;
		foreach (bool result in missionResults) {
			if (result == true) {
				amountOfAccomplishedMissions++;
			}
		}

		if (amountOfAccomplishedMissions >= 1) {
			MakeMedalGold (img1);
		}
		if (amountOfAccomplishedMissions >= 2) {
			MakeMedalGold (img2);
		}
		if (amountOfAccomplishedMissions == 3) {
			MakeMedalGold (img3);
		}
	}

	private void MakeMedalGold (Image img){
		img.color = new Color(1f, 1f, 1f, 1f);
		img.sprite = goldMedal;
	}
}