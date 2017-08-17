using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGameController : MonoBehaviour {

	private static PauseGameController instance = null;
	public static PauseGameController Instance {
		get {
			return instance;
		}
	}

	public EnemyController enemyController;
	public TimerController timerController;
    public GameObject pauzeMenu;
    public GameObject storyboard;

    private bool pauze;

    private void Awake() {
		if (instance != null && instance != this) {
			Debug.Log ("Error - Er zijn op dit moment meerdere instanties van PauseGameController (No Singleton)");
		}
		else {
			instance = this;
		}
	}

    private void Update() {
		if (Input.GetButtonDown ("Pauze")) {
			pauze = !pauze;

			if (pauze) {
				PauzeGame ();
			} else {
				ResumeGame ();
			}
		}
	}

    public void FreezeGame() {
        Cursor.visible = true;
		InGamePlayerController.Instance.SetRegenBool(false);
		InGamePlayerController.Instance.StopPlayerWalking();
        enemyController.FreezeMovement();
		TimerController.Instance.StopTimer();
		MissionController.Instance.PauseGame (true);
	}

	public void UnFreezeGame() {
        Cursor.visible = false;
        InGamePlayerController.Instance.SetRegenBool(true);
		InGamePlayerController.Instance.SetPlayerWalkable();
        enemyController.UnFreezeMovement();
		TimerController.Instance.StartTimer();
		MissionController.Instance.PauseGame (false);
    }

    public void PauzeGame() {
        pauze = true;
        pauzeMenu.SetActive(true);
        FreezeGame();
    }

    public void ResumeGame(){
		pauze = false;
		pauzeMenu.SetActive (false);

		if (storyboard.activeSelf == false) {
			UnFreezeGame ();
		}
	}
}