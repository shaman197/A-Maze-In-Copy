using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerPause : MonoBehaviour {

    public GameObject player;
    public MultiplayerEnemyController enemyController;
    public TimerController timerController;
    public MultiplayerCamera fpvCamera;
    public GameObject pauzeMenu;
    public MultiplayerGameManager gameManager;
    public GameObject waitingPanel;

    private bool pauze;
    private MultiplayerMovement playerMovement;

    private void Update() {
		if (Input.GetButtonDown ("Pauze")) {
			pauze = !pauze;

			if (pauze) {
				PauzeGame ();
			} 
			else {
				ResumeGame ();
			}
		}
	}

    public void FreezeGame() {
        Cursor.visible = true;
        //playerMovement.SetRegenBool(false);
        playerMovement.SetWalkable(false);
        enemyController.FreezeMovement();
        fpvCamera.SetPauzed(true);
        timerController.StopTimer();
    }

    public void UnFreezeGame() {
        Cursor.visible = false;
        //playerMovement.SetRegenBool(true);
        playerMovement.SetWalkable(true);
        enemyController.UnFreezeMovement();
        fpvCamera.SetPauzed(false);
        timerController.StartTimer();
    }

    public void PauzeGame()
    {
        pauze = true;
        pauzeMenu.SetActive(true);
        FreezeGame();
    }

    public void ResumeGame()
    {
        pauze = false;
        pauzeMenu.SetActive(false);
        UnFreezeGame();
    }

    public void WaitingGame()
    {
        StartCoroutine(FreezeGameTillBothPlayersAreHere());
    }

    private IEnumerator FreezeGameTillBothPlayersAreHere()
    {
        if(playerMovement == null)
        {
            playerMovement = player.GetComponent<MultiplayerMovement>();
        }

        FreezeGame();

        yield return new WaitUntil(() => gameManager.CheckAllPlayersInGame());

        waitingPanel.SetActive(false);
        gameManager.ConnectMarkersToPlayers();
        gameManager.ConnectCheckpointsToPlayers();

        UnFreezeGame();

        yield return null;
    }

    public void setPlayerComponent(GameObject myPlayer)
    {
        player = myPlayer;
        playerMovement = player.GetComponent<MultiplayerMovement>();
    }
}