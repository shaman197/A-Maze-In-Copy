using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	private static EnemyController instance = null;
	public static EnemyController Instance {
		get {
			return instance;
		}
	}

    public Enemy[] enemies;

	private void Awake() {
		if (instance != null && instance != this) {
			Debug.Log ("Error - Er zijn op dit moment meerdere instanties van EnemyController (No Singleton)");
		}
		else {
			instance = this;
		}
	}
		
	private void Start () {
        StartCoroutine(FindEnemies());
    }

    private IEnumerator FindEnemies() {
        enemies = (Enemy[]) GameObject.FindObjectsOfType(typeof(Enemy));
        yield return null;
    }

    public void FreezeMovement() {
		foreach (Enemy enemy in enemies) {
			enemy.FreezeMovement ();
		}
	}

    public void UnFreezeMovement() {
		foreach (Enemy enemy in enemies) {
			enemy.UnFreezeMovement ();
		}
	}

	public void SetSoundVolumeForAllEnemies(float volume) {
		foreach (Enemy enemy in enemies) {
			enemy.SetAudioVolume (volume);
		}
	}
}
