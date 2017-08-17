using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean.Localization;

public class StoryBoardController : MonoBehaviour {

	public GameObject storyImage;
	public GameObject storyText;
	public StoryManager.StoryParts[] storyParts;
	public StoryManager storyManager;
    public TimerController timer;

	private int storyPartIndex = 0;

	public Text mission1Text;
	public Text mission2Text;
	public Text mission3Text;

	private LeanLocalizedTextArgs llta1;
	private LeanLocalizedTextArgs llta2;
	private LeanLocalizedTextArgs llta3;

	// Use this for initialization
	void Start () {
		storyManager = GameObject.Find("GameController").GetComponent<StoryManager> ();
		InGamePlayerController.Instance.StopPlayerWalking ();

		if (mission1Text != null) {
			llta1 = mission1Text.GetComponent<LeanLocalizedTextArgs> ();
			llta2 = mission2Text.GetComponent<LeanLocalizedTextArgs> ();
			llta3 = mission3Text.GetComponent<LeanLocalizedTextArgs> ();

			ShowMissionTexts ();
		}
	}

	public void ShowMissionTexts() {
		Missions mission1 = MissionController.Instance.mission1;
		Missions mission2 = MissionController.Instance.mission2;
		Missions mission3 = MissionController.Instance.mission3;

		float M1_findExitTime = MissionController.Instance.M1_findExitTime;
		int M3_desiredAmountOfBamboos = MissionController.Instance.M3_desiredAmountOfBamboos;
		float M3_findBambooTime = MissionController.Instance.M3_findBambooTime;
		int M4_desiredAmountOfBadges = MissionController.Instance.M4_desiredAmountOfBadges;
		float M4_findBadgeTime = MissionController.Instance.M4_findBadgeTime;
		int M6_minimumEnergyLevel = MissionController.Instance.M6_minimumEnergyLevel;
		int M8_desiredAmountOfOpenedDoors = MissionController.Instance.M8_desiredAmountOfOpenedDoors;
		int M10_minimumSprintDistanceInTime = MissionController.Instance.M10_minimumSprintDistanceInTime;

		switch (mission1) {
		case Missions.M1_FindExitInTime:
			llta1.PhraseName = "Mission-FindExitInTime";
			mission1Text.text = string.Format (mission1Text.text, M1_findExitTime);
			break;
		case Missions.M2_JumpInAGame:
			llta1.PhraseName = "Mission-JumpInAgame";
			mission1Text.text = string.Format(mission1Text.text);
			break;
		case Missions.M3_BambooInTime:
			llta1.PhraseName = "Mission-BambooInTime";
			mission1Text.text = string.Format(mission1Text.text, M3_desiredAmountOfBamboos, M3_findBambooTime);
			break;
		case Missions.M4_BadgesInTime:
			llta1.PhraseName = "Mission-BadgesInTime";
			mission1Text.text = string.Format(mission1Text.text, M4_desiredAmountOfBadges, M4_findBadgeTime);
			break;
		case Missions.M6_KeepEnergyLevel:
			llta1.PhraseName = "Mission-KeepEnergyAboveSpecificLevel";
			mission1Text.text = string.Format(mission1Text.text, M6_minimumEnergyLevel);
			break;
		case Missions.M8_OpenAmountOfDoors:
			llta1.PhraseName = "Mission-DoorsOpened";
			mission1Text.text = string.Format(mission1Text.text, M8_desiredAmountOfOpenedDoors);
			break;
		case Missions.M9_FREE_BADGE_FOR_TESTING:
			llta1.PhraseName = "Mission-TestForDebug";
			mission1Text.text = string.Format(mission1Text.text);
			break;
		case Missions.M10_SprintForXSeconds:
			llta1.PhraseName = "Mission-SprintXDistance";
			mission1Text.text = string.Format(mission1Text.text, M10_minimumSprintDistanceInTime);
			break;
		default:

			break;
		}
		Destroy (llta1);

		switch (mission2) {
		case Missions.M1_FindExitInTime:
			llta2.PhraseName = "Mission-FindExitInTime";
			mission2Text.text = string.Format(mission2Text.text, M1_findExitTime);
			break;
		case Missions.M2_JumpInAGame:
			llta2.PhraseName = "Mission-JumpInAgame";
			mission2Text.text = string.Format(mission2Text.text);
			break;
		case Missions.M3_BambooInTime:
			llta2.PhraseName = "Mission-BambooInTime";
			mission2Text.text = string.Format(mission2Text.text, M3_desiredAmountOfBamboos, M3_findBambooTime);
			break;
		case Missions.M4_BadgesInTime:
			llta2.PhraseName = "Mission-BadgesInTime";
			mission2Text.text = string.Format(mission2Text.text, M4_desiredAmountOfBadges, M4_findBadgeTime);
			break;
		case Missions.M6_KeepEnergyLevel:
			llta2.PhraseName = "Mission-KeepEnergyAboveSpecificLevel";
			mission2Text.text = string.Format(mission2Text.text, M6_minimumEnergyLevel);
			break;
		case Missions.M8_OpenAmountOfDoors:
			llta2.PhraseName = "Mission-DoorsOpened";
			mission2Text.text = string.Format(mission2Text.text, M8_desiredAmountOfOpenedDoors);
			break;
		case Missions.M9_FREE_BADGE_FOR_TESTING:
			llta2.PhraseName = "Mission-TestForDebug";
			mission2Text.text = string.Format(mission2Text.text);
			break;
		case Missions.M10_SprintForXSeconds:
			llta2.PhraseName = "Mission-SprintXDistance";
			mission2Text.text = string.Format(mission2Text.text, M10_minimumSprintDistanceInTime);
			break;
		default:

			break;
		}
		Destroy (llta2);

		switch (mission3) {
		case Missions.M1_FindExitInTime:
			llta3.PhraseName = "Mission-FindExitInTime";
			mission3Text.text = string.Format(mission3Text.text, M1_findExitTime);
			break;
		case Missions.M2_JumpInAGame:
			llta3.PhraseName = "Mission-JumpInAgame";
			mission3Text.text = string.Format(mission3Text.text);
			break;
		case Missions.M3_BambooInTime:
			llta3.PhraseName = "Mission-BambooInTime";
			mission3Text.text = string.Format(mission3Text.text, M3_desiredAmountOfBamboos, M3_findBambooTime);
			break;
		case Missions.M4_BadgesInTime:
			llta3.PhraseName = "Mission-BadgesInTime";
			mission3Text.text = string.Format(mission3Text.text, M4_desiredAmountOfBadges, M4_findBadgeTime);
			Destroy (llta3);
			break;
		case Missions.M6_KeepEnergyLevel:
			llta3.PhraseName = "Mission-KeepEnergyAboveSpecificLevel";
			mission3Text.text = string.Format(mission3Text.text, M6_minimumEnergyLevel);
			break;
		case Missions.M8_OpenAmountOfDoors:
			llta3.PhraseName = "Mission-DoorsOpened";
			mission3Text.text = string.Format(mission3Text.text, M8_desiredAmountOfOpenedDoors);
			break;
		case Missions.M9_FREE_BADGE_FOR_TESTING:
			llta3.PhraseName = "Mission-TestForDebug";
			mission3Text.text = string.Format(mission3Text.text);
			break;
		case Missions.M10_SprintForXSeconds:
			llta3.PhraseName = "Mission-SprintXDistance";
			mission3Text.text = string.Format(mission3Text.text, M10_minimumSprintDistanceInTime);
			break;
		default:

			break;
		}
		Destroy (llta3);
	}

	public void ShowNewStoryPart() {

		if (storyParts.Length > storyPartIndex) {
            transform.GetChild(0).GetComponent<Text>().text = storyParts[storyPartIndex].storyPartText;
            transform.GetChild(1).GetComponent<Image> ().sprite = storyParts [storyPartIndex].image;

			storyPartIndex++;
		} 
		else {
			CloseStoryPanel ();
		}
	}

	public void ShowPreviousStoryPart() {

		if (storyPartIndex > 1) {
			storyPartIndex -= 2;
            transform.GetChild(0).GetComponent<Text>().text = storyParts[storyPartIndex].storyPartText;
            transform.GetChild(1).GetComponent<Image>().sprite = storyParts [storyPartIndex].image;
			storyPartIndex++;
		}
	}

	public void LoadStory(int storyId) {
		storyParts = storyManager.themes [storyId].storyParts;
		ShowNewStoryPart();
	}

	public void CloseStoryPanel() {
		transform.gameObject.SetActive(false);
        PauseGameController.Instance.UnFreezeGame();
        timer.StartTimer();
    }
}
