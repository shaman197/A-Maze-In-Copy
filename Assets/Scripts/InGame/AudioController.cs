using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

	private static AudioController instance = null;
	public static AudioController Instance {
		get {
			return instance;
		}
	}

	public AudioClip openDoor;
	public AudioClip pressPlate;
	public AudioClip fallingTree;
	public AudioClip failingLevel;
	public AudioClip finishLevel;
	public AudioClip closeDoors;
	public AudioClip badgeFound;
	public AudioClip bambooFound;
	public AudioClip backgroundMusic;
	public AudioClip ambienceMusic;

	public GameObject singleSoundAudioSourcesParent;

	public AudioSource backgroundMusicSource;
	public AudioSource ambienceMusicSource;
    public AudioSource hunterWarningSource;

    private AudioSource[] singleSoundSources;
	private AudioSource currentSingleAudioSource;
	private int singleAudioSourceIndex = 0;

	private void Awake() {
		if (instance != null && instance != this) {
			Debug.Log ("Error - Er zijn op dit moment meerdere instanties van AudioController (No Singleton)");
		}
		else {
			instance = this;
		}
	}

	private void Start() {
		singleSoundSources = singleSoundAudioSourcesParent.GetComponents<AudioSource> ();

		currentSingleAudioSource = singleSoundSources [0];

		SetVolumes ();
	}

	public void SetVolumes() {
		backgroundMusicSource.volume = DataHolder.Instance.GetMusicVolume();
		ambienceMusicSource.volume = DataHolder.Instance.GetAmbianceVolume();
		float sfxVolume = DataHolder.Instance.GetSFXVolume();
		foreach (AudioSource source in singleSoundSources) {
			source.volume = sfxVolume;
		}
		// TODO: Add script to change volume in singlePlayer
		if (MultiplayerEnemyController.Instance != null) {
			MultiplayerEnemy[] enemies = MultiplayerEnemyController.Instance.GetListOfEnemies ();
			foreach (MultiplayerEnemy enemie in enemies) {
				enemie.transform.GetComponent<AudioSource> ().volume = sfxVolume;
			}
		}
	}

	public void PlaySound(Sounds sound) {
		StartCoroutine (PlaySoundOneTime (sound));
	}

	private IEnumerator PlaySoundOneTime(Sounds sound) {

		switch (sound) {
		case Sounds.BambooFound:
			currentSingleAudioSource.clip = bambooFound;
			currentSingleAudioSource.Play ();
			ChangeAudioSourcePitch ();
			break;
		case Sounds.BadgeFound:
			currentSingleAudioSource.clip = badgeFound;
			currentSingleAudioSource.Play ();
			ChangeAudioSourcePitch ();
			break;
		case Sounds.OpenDoor:
			ResetAudioSourcePitch ();
			currentSingleAudioSource.clip = openDoor;
			currentSingleAudioSource.Play ();
			break;
		case Sounds.PressPlate:
			ResetAudioSourcePitch ();
			currentSingleAudioSource.clip = pressPlate;
			currentSingleAudioSource.Play ();
			break;
		case Sounds.FallingTree:
			ResetAudioSourcePitch ();
			currentSingleAudioSource.clip = fallingTree;
			currentSingleAudioSource.Play ();
			break;
		case Sounds.FailingLevel:
			ResetAudioSourcePitch ();
			currentSingleAudioSource.clip = failingLevel;
			currentSingleAudioSource.Play ();
			break;
		case Sounds.FinishLevel:
			ResetAudioSourcePitch ();
			currentSingleAudioSource.clip = finishLevel;
			currentSingleAudioSource.Play ();
			break;
		case Sounds.CloseDoor:
			ResetAudioSourcePitch ();
			currentSingleAudioSource.clip = closeDoors;
			currentSingleAudioSource.Play ();
			break;
        default:
			ResetAudioSourcePitch ();
			currentSingleAudioSource.clip = bambooFound;
			currentSingleAudioSource.Play ();
			break;
		}

		yield return null;
	}

	private void ResetAudioSourcePitch() {
		StopAllCoroutines ();
		singleAudioSourceIndex = 0;
		currentSingleAudioSource = singleSoundSources [0];
	}

	private void ChangeAudioSourcePitch() {
		StopAllCoroutines ();
		StartCoroutine (SetHigherPitchBriefly ());
	}

	private IEnumerator SetHigherPitchBriefly() {
		if (singleAudioSourceIndex >= 4) {
			yield return new WaitForSecondsRealtime (1f);
			singleAudioSourceIndex = 0;
			currentSingleAudioSource = singleSoundSources [0];
			// Code for level designing
			if (Application.isEditor) {
				Debug.Log ("Last audioSource has been reached. Be careful with placing to many collectables in the same area!");
				Debug.LogWarning ("Last audioSource has been reached. Be careful with placing to many collectables in the same area!");
			}
		} 
		else {
			singleAudioSourceIndex++;
			currentSingleAudioSource = singleSoundSources [singleAudioSourceIndex];
			yield return new WaitForSecondsRealtime (1f);
			singleAudioSourceIndex = 0;
			currentSingleAudioSource = singleSoundSources [0];
		}
	}
    
    public void PlayHunterWarningSound()
    {
        hunterWarningSource.Play();
    }

    public void StopHunterWarningSound()
    {
        hunterWarningSource.Stop();
    }
}

