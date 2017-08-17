using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Lean.Localization;

public class FacebookController : MonoBehaviour {
	
    //TODO: replace value with the real values
    public static string appId = "145634995501895";
    public static string pictureUrl = "https://www.wnf.nl/upload_mm/e/4/5/8266_fullimage_logo-vierkant-wnf-rangerclub_250x250.jpg";
    public static string linkUrl = "https://developers.facebook.com/docs/";
    public static string redirectUrl = "https://developers.facebook.com/tools/explorer";

    public Text title;
    public Text description;
    public Text levelName;

    private LeanLocalizedTextArgs llta;
    private JsonPlayerDataList playerList;

    private void Start() {
        llta = levelName.gameObject.GetComponent<LeanLocalizedTextArgs>();

        if(PlayerPrefs.HasKey(PlayerDataController.PlayerDataName))
        {
            playerList = JsonUtility.FromJson<JsonPlayerDataList>(PlayerPrefs.GetString(PlayerDataController.PlayerDataName));

            // Kids under 13 can't use Facebook
            if (playerList.list[0].age < 13)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void Share() {
		if (DataHolder.Instance != null) {
		
			llta.PhraseName = DataHolder.Instance.levelKey;
			description.text = string.Format (description.text, playerList.list [0].playerName, levelName.text);
		} 
		else {
			description.text = string.Format (description.text, "[insert name]", "[insert level]");
		}

		CreateSharePost (title.text, description.text);
	}

    private void CreateSharePost(string name, string description) {
        string facebookFormat = "https://www.facebook.com/dialog/feed?" +
            "app_id=" + appId + "&" +
            "display=popup&" +
            "link=" + WWW.EscapeURL(linkUrl) + " & " +
            "name=" + WWW.EscapeURL(name) + " & " +
            "description=" + WWW.EscapeURL(description) + " & " +
            "picture=" + WWW.EscapeURL(pictureUrl) + " & " +
            "redirect_uri=" + WWW.EscapeURL(redirectUrl);

        Application.OpenURL(facebookFormat);
    }
}