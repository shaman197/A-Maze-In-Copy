using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerEnemyController : MonoBehaviour {

	private static MultiplayerEnemyController instance = null;
	public static MultiplayerEnemyController Instance {
		get {
			return instance;
		}
	}

    private MultiplayerEnemy[] enemies;

    private void Awake() {
		if (instance != null && instance != this) {
			Debug.Log ("Error - Er zijn op dit moment meerdere instanties van MultiplayerEnemyController (No Singleton)");
		}
		else {
			instance = this;
		}

        StartCoroutine(FindEnemies());
    }

    private IEnumerator FindEnemies() {
        enemies = (MultiplayerEnemy[])GameObject.FindObjectsOfType(typeof(MultiplayerEnemy));
        yield return null;
    }

    public void FreezeMovement() {
		foreach (MultiplayerEnemy enemy in enemies) {
			enemy.FreezeMovement ();
		}
	}

    public void UnFreezeMovement() {
		foreach (MultiplayerEnemy enemy in enemies) {
			enemy.UnFreezeMovement ();
		}
	}

    public void SetPlayer(GameObject player) {
		foreach (MultiplayerEnemy enemy in enemies) {
			enemy.player = player;
		}
	}

    public void Uncaught()
    {
        foreach (MultiplayerEnemy enemy in enemies)
        {
            enemy.PlayerUnCaughtForAll();
        }
    }

    public MultiplayerEnemy[] GetListOfEnemies() {
		return enemies;
	}

	public void SetSoundVolumeForAllEnemies(float volume) {
		foreach (MultiplayerEnemy enemy in enemies) {
			enemy.SetAudioVolume (volume);
		}
	}
}