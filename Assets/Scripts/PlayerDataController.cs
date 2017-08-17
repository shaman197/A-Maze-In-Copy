using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
using Lean.Localization;

public class PlayerDataController : MonoBehaviour {

    public static string PlayerDataName = "PlayerData";

    public GameObject panel;
    public Text nameText;
    public Text ageText;
    public LeanLocalizedTextArgs lltaBoy;
    public LeanLocalizedTextArgs lltaGirl;
    public Toggle genderBoy;
    public Toggle genderGirl;
    public Button submitName;
    public Button submitAge;
    public Button submitGender;

    private PlayerData myPlayerData = new PlayerData();

    private void Start() {
		if (PlayerPrefs.HasKey (PlayerDataName)) {
			panel.SetActive (false);
		} else {
			panel.SetActive (true);
		}

        genderBoy.onValueChanged.AddListener ((valueChecked) => {
			if (valueChecked) {
				SetGender (Gender.Male);
			}
		});

		genderGirl.onValueChanged.AddListener ((valueChecked) => {
			if (valueChecked) {
				SetGender (Gender.Female);
			}
		});
	}

    public void SetName() {
		if (nameText.text != "") {
			myPlayerData.playerName = nameText.text;
		}

        canSubmitName();
    }

    public void SetAge() {
		if (ageText.text != "" && int.TryParse (ageText.text, out myPlayerData.age)) { // TryParse change the value in the out if possible

            if (myPlayerData.age >= 18)
            {
                lltaBoy.PhraseName = "Man";
                lltaGirl.PhraseName = "Woman";
            }

            else
            {
                lltaBoy.PhraseName = "Boy";
                lltaGirl.PhraseName = "Girl";
            }
        }

        canSubmitAge();
    }

    public void SetGender(Gender gender) {
        myPlayerData.gender = gender;
        canSubmitGender();
    }

    public void SavePlayerData() {
        PhotonNetwork.playerName = myPlayerData.playerName;

        UpdatePlayerLocal();
        SendPlayerDataToAnalytics();
        panel.SetActive(false);
    }

    private void canSubmitName() {
		if (nameText.text != "") {
			submitName.interactable = true;
		}

        else
        {
            submitName.interactable = false;
        }
	}

    private void canSubmitAge()
    {
        if (myPlayerData.age > 0)
        {
            submitAge.interactable = true;
        }

        else
        {
            submitAge.interactable = false;
        }
    }

    private void canSubmitGender()
    {
        if (myPlayerData.gender != Gender.Unknown)
        {
            submitGender.interactable = true;
        }

        else
        {
            submitGender.interactable = false;
        }
    }

    public void UpdatePlayerLocal() {
        JsonPlayerDataList jsonList = new JsonPlayerDataList(myPlayerData);
        string json = JsonUtility.ToJson(jsonList);
        PlayerPrefs.SetString(PlayerDataName, json);
    }

    private void SendPlayerDataToAnalytics() {
        int year = int.Parse(System.DateTime.Now.ToString("yyyy"));

        Analytics.SetUserId(SystemInfo.deviceUniqueIdentifier);
        Analytics.SetUserGender(myPlayerData.gender);
        Analytics.SetUserBirthYear(year - myPlayerData.age);
    }
}