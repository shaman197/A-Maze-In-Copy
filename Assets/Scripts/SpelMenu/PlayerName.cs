using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerName : MonoBehaviour {

    public Text nameText;

	private void Start () {

        if (PlayerPrefs.HasKey(PlayerDataController.PlayerDataName))
        {
            string json = PlayerPrefs.GetString(PlayerDataController.PlayerDataName);
            JsonPlayerDataList jsonList = JsonUtility.FromJson<JsonPlayerDataList>(json);
            PlayerData player = jsonList.list[0];

            nameText.text = player.playerName;
        }
	}
}
