using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean.Localization;

public class MissionController : MonoBehaviour {

	private static MissionController instance = null;
	public static MissionController Instance {
		get {
			return instance;
		}
	}

	public Missions mission1;
	public Missions mission2;
	public Missions mission3;

	private bool gameIsPaused = true;

	// Mission booleans \\
	private bool isFinishedInTime = false;
	private bool hasJumped = false;
	private bool hasSprintedXSeconds = false;
	private bool reachedAmountOfBambooInTime = false;
	private bool reachedAmountOfBadgesInTime = false;
	private bool keptEnergyAboveLevel = true;
	private bool openedAmountOfDoors = false;

	// Mission-Specific fields \\
	public float M1_findExitTime;
	private float M1_findExitTimer;
	public float M3_findBambooTime;
	public int M3_desiredAmountOfBamboos;
	private int M3_amountOfBambooInTime = 0;
	private int M3_maxBambooAmount = 0;
	public float M4_findBadgeTime;
	public int M4_desiredAmountOfBadges;
	private int M4_amountOfBadgesInTime = 0;
	private int M4_maxBadgeAmount = 0;
	public int M6_minimumEnergyLevel;
	public int M8_desiredAmountOfOpenedDoors;
	private int M8_amountOfOpenedDoors;
	public int M10_minimumSprintDistanceInTime = 0;
	private float M10_startSprintTime = 0f;
	private float M10_sprintTime = 0f;
	private bool M10_isRunning = false;

	public Sprite checkBoxWithMark;
	public Sprite emptyCheckBox;

	public Text mission1Text;
	public Text mission2Text;
	public Text mission3Text;

	private LeanLocalizedTextArgs llta1;
	private LeanLocalizedTextArgs llta2;
	private LeanLocalizedTextArgs llta3;

	public GameObject missionMap;

	private Image missionPanelCheckBox1;
	private Image missionPanelCheckBox2;
	private Image missionPanelCheckBox3;

	private LeanLocalizedTextArgs missionPanelMission1TextTa;
	private LeanLocalizedTextArgs missionPanelMission2TextTa;
	private LeanLocalizedTextArgs missionPanelMission3TextTa;

	private Text missionPanelMission1Text;
	private Text missionPanelMission2Text;
	private Text missionPanelMission3Text;

	private void Awake() {
		if (instance != null && instance != this) {
			Debug.Log ("Error - Er zijn op dit moment meerdere instanties van MissionController (No Singleton)");
		}
		else {
			instance = this;
		}
	}

	private void Start() {
		CheckForDuplicateMission ();

		llta1 = mission1Text.GetComponent<LeanLocalizedTextArgs> ();
		llta2 = mission2Text.GetComponent<LeanLocalizedTextArgs> ();
		llta3 = mission3Text.GetComponent<LeanLocalizedTextArgs> ();

		//TODO: Clean up this mess..
		missionPanelCheckBox1 = GameObject.Find ("InGameCanvas/MissionMap/Mission1Box").GetComponent<Image>();
		missionPanelCheckBox2 = GameObject.Find ("InGameCanvas/MissionMap/Mission2Box").GetComponent<Image>();
		missionPanelCheckBox3 = GameObject.Find ("InGameCanvas/MissionMap/Mission3Box").GetComponent<Image>();
		missionPanelMission1Text = GameObject.Find ("InGameCanvas/MissionMap/Mission1Text").GetComponent<Text>();
		missionPanelMission2Text = GameObject.Find ("InGameCanvas/MissionMap/Mission2Text").GetComponent<Text>();
		missionPanelMission3Text = GameObject.Find ("InGameCanvas/MissionMap/Mission3Text").GetComponent<Text>();
		missionPanelMission1TextTa = missionPanelMission1Text.GetComponent<LeanLocalizedTextArgs>();
		missionPanelMission2TextTa = missionPanelMission2Text.GetComponent<LeanLocalizedTextArgs>();
		missionPanelMission3TextTa = missionPanelMission3Text.GetComponent<LeanLocalizedTextArgs>();

		ShowAllMissionTexts ();

		M1_findExitTimer = M1_findExitTime;
	}

	private void FixedUpdate() {
		//TODO: Link this with the ingame timer (so this fixedupdate-code leaves)
		if (!gameIsPaused) {
			float elapsedTime = Time.deltaTime;
			M1_findExitTimer -= elapsedTime;
		}
	}

	public void CheckEnergyLevelAboveLevel(int level) {
		if (M6_minimumEnergyLevel > level) {
			keptEnergyAboveLevel = false;
			StartCoroutine(CheckAllMissionPanelCheckBoxes ());
		}
	}

	public void PlayerOpenedNewDoor() {
		M8_amountOfOpenedDoors++;

		if (M8_amountOfOpenedDoors == M8_desiredAmountOfOpenedDoors) {
			openedAmountOfDoors = true;
			StartCoroutine(CheckAllMissionPanelCheckBoxes ());
		}
	}

	public void ShowAllMissionTexts() {
		ShowMissionText (mission1, llta1, mission1Text, missionPanelMission1TextTa, missionPanelMission1Text);
		ShowMissionText (mission2, llta2, mission2Text, missionPanelMission2TextTa, missionPanelMission2Text);
		ShowMissionText (mission3, llta3, mission3Text, missionPanelMission3TextTa, missionPanelMission3Text);
	}

	public IEnumerator CheckAllMissionPanelCheckBoxes() {
		UpdateMissionPanelCheckBoxes (mission1, missionPanelCheckBox1);
		UpdateMissionPanelCheckBoxes (mission2, missionPanelCheckBox2);
		UpdateMissionPanelCheckBoxes (mission3, missionPanelCheckBox3);

		yield return null;
	}

	private void UpdateMissionPanelCheckBoxes(Missions mission, Image img) {
		switch (mission) {
		case Missions.M1_FindExitInTime:
			if (isFinishedInTime) {
				img.sprite = checkBoxWithMark;
			}
			break;
		case Missions.M2_JumpInAGame:
			if (hasJumped) {
				img.sprite = checkBoxWithMark;
			}
			break;
		case Missions.M3_BambooInTime:
			if (reachedAmountOfBambooInTime) {
				img.sprite = checkBoxWithMark;
			}
			break;
		case Missions.M4_BadgesInTime:
			if (reachedAmountOfBadgesInTime) {
				img.sprite = checkBoxWithMark;
			}
			break;
		case Missions.M6_KeepEnergyLevel:
			if (keptEnergyAboveLevel) {
				img.sprite = checkBoxWithMark;
			} 
			else {
				img.sprite = emptyCheckBox;
			}
			break;
		case Missions.M8_OpenAmountOfDoors:
			if (openedAmountOfDoors) {
				img.sprite = checkBoxWithMark;
			}
			break;
		case Missions.M9_FREE_BADGE_FOR_TESTING:
			if (hasJumped) {
				img.sprite = checkBoxWithMark;
			}
			break;
		case Missions.M10_SprintForXSeconds:
			if (hasSprintedXSeconds) {
				img.sprite = checkBoxWithMark;
			}
			break;
		default:
			img.sprite = emptyCheckBox;
			break;
		}
	}

	private void ShowMissionText(Missions mission, LeanLocalizedTextArgs llta, Text text, LeanLocalizedTextArgs missionPanelMissionTextTa, Text missionPanelText) {
		
		switch (mission) {
		case Missions.M1_FindExitInTime:
			llta.PhraseName = "Mission-FindExitInTime";
			text.text = string.Format(text.text, M1_findExitTime);
			missionPanelMissionTextTa.PhraseName = "Mission-FindExitInTime";
			missionPanelText.text = string.Format(text.text, M1_findExitTime);
			break;
		case Missions.M2_JumpInAGame:
			llta.PhraseName = "Mission-JumpInAgame";
			text.text = string.Format(text.text);
			missionPanelMissionTextTa.PhraseName = "Mission-JumpInAgame";
			missionPanelText.text = string.Format(text.text);
			break;
		case Missions.M3_BambooInTime:
			llta.PhraseName = "Mission-BambooInTime";
			text.text = string.Format(text.text, M3_desiredAmountOfBamboos, M3_findBambooTime);
			missionPanelMissionTextTa.PhraseName = "Mission-FindExitInTime";
			missionPanelText.text = string.Format(text.text, M3_desiredAmountOfBamboos, M3_findBambooTime);
			break;
		case Missions.M4_BadgesInTime:
			llta.PhraseName = "Mission-BadgesInTime";
			text.text = string.Format(text.text, M4_desiredAmountOfBadges, M4_findBadgeTime);
			missionPanelMissionTextTa.PhraseName = "Mission-BadgesInTime";
			missionPanelText.text = string.Format(text.text, M4_desiredAmountOfBadges, M4_findBadgeTime);
			break;
		case Missions.M6_KeepEnergyLevel:
			llta.PhraseName = "Mission-KeepEnergyAboveSpecificLevel";
			text.text = string.Format(text.text, M6_minimumEnergyLevel);
			missionPanelMissionTextTa.PhraseName = "Mission-KeepEnergyAboveSpecificLevel";
			missionPanelText.text = string.Format(text.text, M6_minimumEnergyLevel);
			break;
		case Missions.M8_OpenAmountOfDoors:
			llta.PhraseName = "Mission-DoorsOpened";
			text.text = string.Format(text.text, M8_desiredAmountOfOpenedDoors);
			missionPanelMissionTextTa.PhraseName = "Mission-DoorsOpened";
			missionPanelText.text = string.Format(text.text, M8_desiredAmountOfOpenedDoors);
			break;
		case Missions.M9_FREE_BADGE_FOR_TESTING:
			llta.PhraseName = "Mission-TestForDebug";
			text.text = string.Format(text.text);
			missionPanelMissionTextTa.PhraseName = "Mission-TestForDebug";
			missionPanelText.text = string.Format(text.text);
			break;
		case Missions.M10_SprintForXSeconds:
			llta.PhraseName = "Mission-SprintXDistance";
			text.text = string.Format(text.text, M10_minimumSprintDistanceInTime);
			missionPanelMissionTextTa.PhraseName = "Mission-SprintXDistance";
			missionPanelText.text = string.Format(text.text, M10_minimumSprintDistanceInTime);
			break;
		default:
			llta.PhraseName = "Mission-TestForDebug";
			text.text = string.Format(text.text);
			missionPanelMissionTextTa.PhraseName = "Mission-TestForDebug";
			missionPanelText.text = string.Format(text.text);
			break;
		}
	}

	public IEnumerator PickedUpBambooStick() {
		M3_amountOfBambooInTime++;
		CheckForMaxBambooAmount ();

		yield return new WaitForSecondsRealtime (M3_findBambooTime);
		M3_amountOfBambooInTime--;

		yield return null;
	}

	public IEnumerator PickedUpRangerBadge() {
		M4_amountOfBadgesInTime++;
		CheckForMaxBadgeAmount ();

		yield return new WaitForSecondsRealtime (M4_findBadgeTime);
		M4_amountOfBadgesInTime--;

		yield return null;
	}

	private void CheckForMaxBambooAmount() {
		if (M3_maxBambooAmount < M3_amountOfBambooInTime) {
			M3_maxBambooAmount = M3_amountOfBambooInTime;

			if (M3_maxBambooAmount >= M3_desiredAmountOfBamboos) {
				reachedAmountOfBambooInTime = true;
				StartCoroutine(CheckAllMissionPanelCheckBoxes ());
			}
		}
	}

	private void CheckForMaxBadgeAmount() {
		if (M4_maxBadgeAmount < M4_amountOfBadgesInTime) {
			M4_maxBadgeAmount = M4_amountOfBadgesInTime;

			if (M4_maxBadgeAmount >= M4_desiredAmountOfBadges) {
				reachedAmountOfBadgesInTime = true;
				StartCoroutine(CheckAllMissionPanelCheckBoxes ());
			}
		}
	}

	public void PlayerReachedFinish() {
		if (M1_findExitTimer >= -0.9f) {
			isFinishedInTime = true;
			StartCoroutine(CheckAllMissionPanelCheckBoxes ());
		}
	}

	public void PlayerHasJumped() {
		hasJumped = true;
		StartCoroutine(CheckAllMissionPanelCheckBoxes ());
	}

	private void CheckForDuplicateMission() {
		if (mission1 == mission2 || mission2 == mission3 || mission1 == mission3) {
			Debug.Log ("WARNING: Duplicate missions!");
		}
	}

	private bool CheckMissionStatus(Missions mission) {
		switch (mission) {
		case Missions.M1_FindExitInTime:
			return isFinishedInTime;
		case Missions.M2_JumpInAGame:
			return hasJumped;
		case Missions.M3_BambooInTime:
			return reachedAmountOfBambooInTime;
		case Missions.M4_BadgesInTime:
			return reachedAmountOfBadgesInTime;
		case Missions.M6_KeepEnergyLevel:
			return keptEnergyAboveLevel;
		case Missions.M8_OpenAmountOfDoors:
			return openedAmountOfDoors;
		case Missions.M9_FREE_BADGE_FOR_TESTING:
			return true;
		case Missions.M10_SprintForXSeconds:
			return hasSprintedXSeconds;
		default:
			return false;
		}
	}

	public bool[] CheckForAccomplishedMissions() {
		bool[] tempAchivements = new bool[3];

		tempAchivements [0] = CheckMissionStatus (mission1);
		tempAchivements [1] = CheckMissionStatus (mission2);
		tempAchivements [2] = CheckMissionStatus (mission3);

		return tempAchivements;
	}

	public void PauseGame(bool isPaused) {
		gameIsPaused = isPaused;
	}

	public void PlayerIsRunning() {
		if (!hasSprintedXSeconds) {
			if (!M10_isRunning) {
				M10_startSprintTime = Time.time;
				M10_isRunning = true;
			}
			M10_sprintTime = Time.time;
			if (M10_sprintTime - M10_startSprintTime >= M10_minimumSprintDistanceInTime) {
				hasSprintedXSeconds = true;
				StartCoroutine(CheckAllMissionPanelCheckBoxes ());
			}
		}
	}

	public void PlayerDoesNotRun() {
		M10_isRunning = false;
	}
}