using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerLaucher : Photon.PunBehaviour {

    public PhotonLogLevel logLevel = PhotonLogLevel.Informational;
    public byte maxPlayersPerRoom = 2;

    public Text roomNameInputFieldText;
    public GameObject multiplayerPanel;
    public Transform roomListPanel;
    public GameObject roomListItem;
    public GameObject roomPanel;
    public Transform playerListPanel;
    public GameObject playerListItem;
    public Vector2 offsetListPerItem = new Vector2(0f, -100f);
    public Text roomNameTitleText;
    public Button startGameButton;
    public GameObject HostPanel;
    public GameObject RegularPanel;

    private static string GameVersion = "v4.2";
    private bool isConnecting;

    private PlayerData player;
    private Dictionary<string, GameObject> roomList;
    private Dictionary<string, GameObject> playerList;
    private string currentActiveRoomName;

    private void Awake() {
		PhotonNetwork.autoJoinLobby = true;
		PhotonNetwork.automaticallySyncScene = true;
		PhotonNetwork.logLevel = logLevel;
		PhotonNetwork.autoCleanUpPlayerObjects = true;

		if (PlayerPrefs.HasKey (PlayerDataController.PlayerDataName)) {
			string json = PlayerPrefs.GetString (PlayerDataController.PlayerDataName);
			JsonPlayerDataList jsonList = JsonUtility.FromJson<JsonPlayerDataList> (json);
			player = jsonList.list [0];

			PhotonNetwork.playerName = player.playerName;
		}

		roomList = new Dictionary<string, GameObject> ();
		playerList = new Dictionary<string, GameObject> ();
	}

    public void Connect() {
		isConnecting = true;

		if (!PhotonNetwork.connected) {
			// #Critical, we must first and foremost connect to Photon Online Server.
			PhotonNetwork.ConnectUsingSettings (GameVersion);
		}

		LoadAllRooms ();
	}

    public void CreateAndJoinRoom() {
		if (roomNameInputFieldText.text != "") {
			PhotonNetwork.CreateRoom (roomNameInputFieldText.text, new RoomOptions () { MaxPlayers = maxPlayersPerRoom }, TypedLobby.Default);
		} 
		else {
			PhotonNetwork.CreateRoom (PhotonNetwork.playerName + "'s room", new RoomOptions () { MaxPlayers = maxPlayersPerRoom }, TypedLobby.Default);
		}
	}

    public void LoadAllRooms() {
		// Delete current rooms in case there are
		DeleteCurrentRooms ();

		RoomInfo[] rooms = PhotonNetwork.GetRoomList ();
		foreach (RoomInfo room in rooms) {
			GameObject item = Instantiate (roomListItem);
			item = SetRoomInfo (item, room);
			item.name = room.Name;
			item.transform.SetParent (roomListPanel, false);

			RectTransform position = item.GetComponent<RectTransform> ();
			position.anchoredPosition = position.anchoredPosition + (offsetListPerItem * roomList.Count);

			roomList.Add (item.name, item);
		}

		//// If you need more rooms for testing use this code
		//for (int x = 0; x < 10; x++)
		//{
		//    GameObject item = Instantiate(roomListItem);
		//    item = SetRoomInfoTest(item, x);
		//    item.name = "Room " + x;
		//    item.transform.SetParent(roomListPanel, false);

		//    RectTransform position = item.GetComponent<RectTransform>();
		//    position.anchoredPosition = position.anchoredPosition + (offsetListPerItem * roomList.Count);

		//    roomList.Add(item.name, item);
		//}

		RectTransform listPanelSize = roomListPanel.GetComponent<RectTransform> ();
		listPanelSize.sizeDelta = listPanelSize.sizeDelta - (offsetListPerItem * roomList.Count);
	}

    public GameObject SetRoomInfoTest(GameObject listItem, int number) {
		Text roomText = listItem.transform.GetChild (0).GetComponent<Text> ();
		roomText.text = "Room " + number;

		Text playerCountText = listItem.transform.GetChild (1).GetComponent<Text> ();
		playerCountText.text = string.Format ("{0}/{1}", 0, maxPlayersPerRoom);

		Toggle toggle = listItem.transform.GetComponent<Toggle> ();
		toggle.onValueChanged.AddListener ((bool valueSelected) => {
			if (valueSelected) {
				currentActiveRoomName = roomText.text;
			}
		});

		return listItem;
	}

    public GameObject SetRoomInfo(GameObject listItem, RoomInfo room) {
		Text roomText = listItem.transform.GetChild (0).GetComponent<Text> ();
		roomText.text = room.Name;

		Text playerCountText = listItem.transform.GetChild (1).GetComponent<Text> ();
		playerCountText.text = string.Format ("{0}/{1}", room.PlayerCount, room.MaxPlayers);

		Toggle toggle = listItem.transform.GetComponent<Toggle> ();
		toggle.onValueChanged.AddListener ((bool valueSelected) => {
			if (valueSelected) {
				if (currentActiveRoomName == roomText.text) {
					JoinRoom ();
				} 
				else {
					currentActiveRoomName = roomText.text;
				}
			}
		});

		return listItem;
	}

    private void DeleteCurrentRooms() {
		foreach (KeyValuePair<string, GameObject> roomItem in roomList) {
			Destroy (roomItem.Value);
		}

		roomList.Clear ();
	}

    public void SearchRoom() {
		if (roomNameInputFieldText.text != "") {
			PhotonNetwork.JoinRoom (roomNameInputFieldText.text);
		}
	}

    public void JoinRoom() {
		// TODO: Error handeling on OnPhotonRandomJoinFailed().
		if (currentActiveRoomName != null) {
			PhotonNetwork.JoinRoom (currentActiveRoomName);
		}
	}

    public void JoinRandomRoom() {
        // #Critical we need at this point to attempt joining a random Room. If it fails, we'll get notified in OnPhotonRandomJoinFailed().
        PhotonNetwork.JoinRandomRoom();
    }

    public void LeaveLobby() {
        PhotonNetwork.LeaveLobby();

        multiplayerPanel.SetActive(false);
    }

    public void LoadAllPlayers() {
		PhotonPlayer[] players = PhotonNetwork.playerList;
		foreach (PhotonPlayer roomPlayer in players) {
			AddPlayerToList (roomPlayer);
		}

		RectTransform listPanelSize = playerListPanel.GetComponent<RectTransform> ();
		listPanelSize.sizeDelta = listPanelSize.sizeDelta - (offsetListPerItem * playerList.Count);
	}

    public void AddPlayerToList(PhotonPlayer roomPlayer) {
        GameObject item = Instantiate(playerListItem);
        item = SetPlayerInfo(item, roomPlayer);
        item.name = roomPlayer.NickName;
        item.transform.SetParent(playerListPanel, false);

        RectTransform position = item.GetComponent<RectTransform>();
        position.anchoredPosition = position.anchoredPosition + (offsetListPerItem * playerList.Count);

        playerList.Add(item.name, item);
    }

    public void RemovePlayerFromList(PhotonPlayer roomPlayer) {
        Destroy(playerList[roomPlayer.NickName]);
        playerList.Remove(roomPlayer.NickName);
    }

    private void DeleteCurrentPlayers() {
        foreach(KeyValuePair<string, GameObject> player in playerList)
        {
            Destroy(player.Value);
        }

        playerList.Clear();
    }

    public GameObject SetPlayerInfo(GameObject listItem, PhotonPlayer roomPlayer) {
        Text roomText = listItem.transform.GetChild(0).GetComponent<Text>();
        roomText.text = roomPlayer.NickName;

        return listItem;
    }

    public void StartGame() {
        PhotonNetwork.room.IsOpen = false;
        PhotonNetwork.LoadLevel("Room for 2");
    }

    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();

        roomPanel.SetActive(false);
        multiplayerPanel.SetActive(true);
    }

    public void KickPlayerFromRoom() {
        //TODO: Make working kick function
        //PhotonNetwork.CloseConnection(PhotonPlayer kickPlayer);
    }

    #region Photon override functions 
    public override void OnConnectedToMaster() {
        // If you're in game and leave the scene this function is called
        multiplayerPanel.SetActive(true);
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg) {
        //TODO: Error handeling on OnPhotonRandomJoinFailed()
    }

    public override void OnDisconnectedFromPhoton() {
        // If you have to do something if you disconnect
    }

    public override void OnReceivedRoomListUpdate() {
        LoadAllRooms();
    }

    public override void OnJoinedRoom() {
		roomPanel.SetActive (true);
		multiplayerPanel.SetActive (false);

		// Delete current players in case there are
		DeleteCurrentPlayers ();

		roomNameTitleText.text = PhotonNetwork.room.Name;

		LoadAllPlayers ();

		if (PhotonNetwork.isMasterClient) {
			HostPanel.SetActive (true);
			RegularPanel.SetActive (false);
		} 
		else {
			HostPanel.SetActive (false);
			RegularPanel.SetActive (true);
		}
	}

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer) {
        AddPlayerToList(newPlayer);
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer) {
        RemovePlayerFromList(otherPlayer);
    }
    #endregion
}