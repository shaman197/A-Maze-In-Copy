using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MultiplayerFinishLevel : MonoBehaviour {

    public MultiplayerPause pauseController;
    public EndLevelController endLevelController;
    public int playersNeededForFinish = 2;

    private int collisionCount;

    private void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.GetComponent<MultiplayerMovement> () != null) {
			collisionCount++;

			if (collisionCount >= playersNeededForFinish) {
				pauseController.FreezeGame ();
				endLevelController.ShowFinished ();
				TimerController.Instance.StopTimer ();
			}
		}
	}

    private void OnTriggerExit(Collider collider) {
        collisionCount--;
    }
}