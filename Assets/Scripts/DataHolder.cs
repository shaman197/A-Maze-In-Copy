using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataHolder : MonoBehaviour {

	private static DataHolder instance = null;
	public static DataHolder Instance {
		get {
			return instance;
		}
	}

	public static string IndexNumber = "IndexNumber";
	public static string MusicVolume = "MusicVolume";
	public static string AmbianceVolume = "AmbianceVolume";
	public static string SFXVolume = "SFXVolume";
	public static string MouseSensitivity = "MouseSensitivity";
	public static string HeadBobbing = "HeadBobbing";
	public static string TileId = "TileId";
	public static string PlayerPositionX = "PlayerPositionX";
	public static string PlayerPositionY = "PlayerPositionY";
	public static string PlayerPositionZ = "PlayerPositionZ";

	public Vector3 playerPosition;
	public int storyId;
	public int tileId;
	public int startTileId = 2;
	public string levelSceneName;
	public int levelSystemIndexNumber;
	public int coins;
	public int maxCoins;
	public float achivementTimeInSec;
	public string time;
	public Achivement achivement;
	public bool finished;
	public int showMenuTutorial;
	public string levelKey;
	public bool headBobbing;
	public float musicVolume;
	public float ambianceVolume;
	public float sfxVolume;
	public float mouseSensitivity;

	private void Awake() {
		if (instance != null && instance != this) {
			Destroy(this.gameObject);
		}
		else {
			instance = this;
		}

		TryGetLocalStoredData ();
		DontDestroyOnLoad(this.transform);
	}

	public void UpdateHeadBobbingState(bool shouldBob) {
		headBobbing = shouldBob;
		PlayerPrefs.SetString(HeadBobbing, headBobbing.ToString());
		if (FirstPersonCameraController.Instance != null) {
			FirstPersonCameraController.Instance.ChangeHeadBobbingState (shouldBob);
		}
	}

	public bool GetToggleValue() {
		return headBobbing;
	}

	public void UpdateMusicVolume(float volumeValue) {
		musicVolume = volumeValue;
		PlayerPrefs.SetString(MusicVolume, musicVolume.ToString());

	}

	public void UpdateAmbianceVolume(float volumeValue) {
		ambianceVolume = volumeValue;
		PlayerPrefs.SetString(AmbianceVolume, ambianceVolume.ToString());
	}

	public void UpdateSFXVolume(float volumeValue) {
		sfxVolume = volumeValue;
		PlayerPrefs.SetString(SFXVolume, sfxVolume.ToString());
	}

	public void UpdateMouseSensitivity(float sensitivityValue) {
		mouseSensitivity = sensitivityValue;
		PlayerPrefs.SetString (MouseSensitivity, mouseSensitivity.ToString ());
	}

	public float GetMusicVolume() {
		return musicVolume;
	}

	public float GetAmbianceVolume() {
		return ambianceVolume;
	}

	public float GetSFXVolume() {
		return sfxVolume;
	}

	private void TryGetLocalStoredData() {
		if (PlayerPrefs.HasKey (MusicVolume)) {
			float.TryParse (PlayerPrefs.GetString (MusicVolume), out musicVolume);
		} 
		else {
			musicVolume = 0.10f;
		}

		if (PlayerPrefs.HasKey (AmbianceVolume)) {
			float.TryParse (PlayerPrefs.GetString (AmbianceVolume), out ambianceVolume);
		} 
		else {
			ambianceVolume = 0.10f;
		}

		if (PlayerPrefs.HasKey (SFXVolume)) {
			float.TryParse (PlayerPrefs.GetString (SFXVolume), out sfxVolume);
		} 
		else {
			sfxVolume = 0.3f;
		}

		if (PlayerPrefs.HasKey (HeadBobbing)) {
			bool.TryParse (PlayerPrefs.GetString (HeadBobbing), out headBobbing);
		} 
		else {
			headBobbing = false;
		}

		if (PlayerPrefs.HasKey (TileId)) {
			tileId = PlayerPrefs.GetInt (TileId);
		}  
		else {
			tileId = 2;
		}

		if (PlayerPrefs.HasKey (IndexNumber)) {
			levelSystemIndexNumber = PlayerPrefs.GetInt (IndexNumber);
		}
		else {
			levelSystemIndexNumber = 1;
		}

		if (PlayerPrefs.HasKey (MouseSensitivity)) {
			float.TryParse (PlayerPrefs.GetString (MouseSensitivity), out mouseSensitivity);
		} 
		else {
			mouseSensitivity = 5f;
		}

		if (PlayerPrefs.HasKey (PlayerPositionX)) {
			float tempX, tempY, tempZ;
			float.TryParse (PlayerPrefs.GetString (PlayerPositionX), out tempX);
			float.TryParse (PlayerPrefs.GetString (PlayerPositionY), out tempY);
			float.TryParse (PlayerPrefs.GetString (PlayerPositionZ), out tempZ);

			playerPosition = new Vector3 (tempX, tempY, tempZ);
		} 
		else {
			playerPosition = new Vector3(-0.9439999f, 0f, -1.284009f);
		}
	}

	public void SetPlayerPosition(Vector3 position) {
		playerPosition = position;

		PlayerPrefs.SetString(PlayerPositionX, position.x.ToString());
		PlayerPrefs.SetString(PlayerPositionY, position.y.ToString());
		PlayerPrefs.SetString(PlayerPositionZ, position.z.ToString());
	}

	public int GetGroundTileId() {
		return tileId;
	}

	public void SetStoryId(int id) {
		storyId = id;
	}

	public void SetLevelSceneName(string name) {
		levelSceneName = name;
	}

	public string GetLevelSystemSceneName() {
		return levelSceneName;
	}

	public void LevelSystemIndexNumberUp() {
		levelSystemIndexNumber++;
	}

	public void SetLocalIndexNumber() {
		PlayerPrefs.SetInt(IndexNumber, levelSystemIndexNumber);
	}

	public int GetLevelSystemIndexNumber() {
		return levelSystemIndexNumber;
	}

	public void SetLevelSystemIndexNumber(int levelNumber) {
		levelSystemIndexNumber = levelNumber;
	}

	public void SetTileId(int tile) {
		tileId = tile;
		PlayerPrefs.SetInt(TileId, tileId);
	}

	public void SetCoins(int coins) {
		this.coins = coins;
	}

	public void SetMaxCoins(int coins) {
		maxCoins = coins;
	}

	public void SetAchivementTimeInSec(float timeInSec) {
		this.achivementTimeInSec = timeInSec;
	}

	public void SetTime(string time) {
		this.time = time;
	}

	public void SetAchivements(Achivement achivements) {
		achivement = achivements;
	}

	public void setFinished(bool finish) {
		finished = finish;
	}

	public int GetShowMenuTutorial() {
		return showMenuTutorial;
	}

	public void SetShowMenuTutorial(int show) {
		showMenuTutorial = show;
	}

	public void SetHeadBobbing(bool shouldBob) {
		headBobbing = shouldBob;
	}

	public bool GetHeadBobbingBool() {
		return headBobbing;
	}

	public string GetLevelKey() {
		return levelKey;
	}

	public void SetLevelKey(string key) {
		levelKey = key;
	}
}