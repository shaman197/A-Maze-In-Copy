using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerGameManager : Photon.PunBehaviour {

    public Vector3 playerPositionHost;
    public Vector3 playerPositionOther;
    public MultiplayerCamera playerCamera;
    public ZoomedMinimapCamera minimapCamera;
    public MultiplayerEnemyController enemyController;
    public MultiplayerPause pauzeController;
    public MultiplayerCheckpoint checkpoint;
    public int NeededPlayers = 2;
    public MultiplayerMarker markerPlayer1;
    public MultiplayerMarker markerPlayer2;

    private int playerCount = 0;
    private GameObject player;
    MultiplayerMovement[] players;

    private void Start() {
		PhotonNetwork.isMessageQueueRunning = true;

		if (PhotonNetwork.isMasterClient) {
			player = PhotonNetwork.Instantiate ("MultiplayerPanda1", playerPositionHost, Quaternion.identity, 0);
        } 
		else {
			player = PhotonNetwork.Instantiate ("MultiplayerPanda2", playerPositionOther, Quaternion.identity, 0);
        }

        playerCamera.SetPlayerMovement (player);
		playerCamera.transform.SetParent (player.transform, false);
		minimapCamera.player = player;

        pauzeController.setPlayerComponent(player);

		photonView.RPC ("AddPlayerCount", PhotonTargets.All);

		enemyController.SetPlayer (player);

		pauzeController.WaitingGame ();
    }

    [PunRPC]
    public void LoadArena() {
        PhotonNetwork.isMessageQueueRunning = false;
        PhotonNetwork.LoadLevel("Room for 2");
    }

    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
    }

    public void ResetLevel() {
        photonView.RPC("LoadArena", PhotonTargets.All);
    }

    [PunRPC]
    public void AddPlayerCount() {
        playerCount++;
    }

    public bool CheckAllPlayersInGame() {
		if (playerCount == NeededPlayers) {
            players = (MultiplayerMovement[])GameObject.FindObjectsOfType(typeof(MultiplayerMovement));
            return true;
		} 
		else {
			return false;
		}
	}

    public void RespawnAtCheckpoint()
    {
        checkpoint.Respawn();
        enemyController.Uncaught();
    }

    public void ConnectMarkersToPlayers()
    {
        foreach (MultiplayerMovement foundPlayer in players)
        {
            if(foundPlayer.CompareTag("Player"))
            {
                markerPlayer1.myPlayer = foundPlayer.gameObject;
                markerPlayer2.otherPlayer = foundPlayer.gameObject;
            }

            else
            {
                markerPlayer2.myPlayer = foundPlayer.gameObject;
                markerPlayer1.otherPlayer = foundPlayer.gameObject;
            }
        }
    }

    public void ConnectCheckpointsToPlayers()
    {
        foreach (MultiplayerMovement foundPlayer in players)
        {
            if (foundPlayer.CompareTag("Player"))
            {
                checkpoint.playerHost = foundPlayer.gameObject;
            }

            else
            {
                checkpoint.playerOther = foundPlayer.gameObject;
            }
        }
    }

    #region Photon override functions
    public override void OnLeftRoom() {
        SceneManager.LoadScene("StartMenu");
    }
    #endregion
}