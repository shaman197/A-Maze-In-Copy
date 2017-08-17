using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionToDataHolderLink : MonoBehaviour {

	public Slider slider1;
	public Slider slider2;
	public Slider slider3;

	public Toggle headBobbingToggle;

	public Slider mouseSensitivitySider;

	private DataHolder dataHolder;

	void Start () {
		dataHolder = DataHolder.Instance;
		UpdateSliderValues ();
		UpdateToggleValue ();
		UpdateSensitivitySetting ();
	}

	public void ChangeMusicVolume() {
		DataHolder.Instance.UpdateMusicVolume (slider1.value);
		// TODO: now updates ALL soundvolumes. Must be changed to a single volume setting
		UpdateSoundInGame ();
	}

	public void ChangeAmbianceVolume() {
		DataHolder.Instance.UpdateAmbianceVolume (slider2.value);
		// TODO: now updates ALL soundvolumes. Must be changed to a single volume setting
		UpdateSoundInGame ();
	}

	public void ChangeSFXVolume() {
		DataHolder.Instance.UpdateSFXVolume (slider3.value);
		// TODO: now updates ALL soundvolumes. Must be changed to a single volume setting
		UpdateSoundInGame ();
	}

	public void ChangeHeadbobbingState() {
		DataHolder.Instance.UpdateHeadBobbingState (headBobbingToggle.isOn);
	}

	public void ChangeMouseSensitivity() {
		DataHolder.Instance.UpdateMouseSensitivity (mouseSensitivitySider.value);

		if (FirstPersonCameraController.Instance != null) {
			FirstPersonCameraController.Instance.UpdateMouseSensitivitySetting ();
		}
		if (MultiplayerCamera.Instance != null) {
			MultiplayerCamera.Instance.UpdateMouseSensitivitySetting ();
		}
		if (InGamePlayerController.Instance != null) {
			InGamePlayerController.Instance.UpdateMouseSensitivitySetting ();
		}
		if (MultiplayerMovement.Instance != null) {
			MultiplayerMovement.Instance.UpdateMouseSensitivitySetting ();
		}
	}

	private void UpdateSoundInGame() {
		if (AudioController.Instance != null) {
			AudioController.Instance.SetVolumes ();
		}
		if (EnemyController.Instance != null) {
			EnemyController.Instance.SetSoundVolumeForAllEnemies (slider3.value);
		}
		if (MultiplayerEnemyController.Instance != null) {
			MultiplayerEnemyController.Instance.SetSoundVolumeForAllEnemies (slider3.value);
		}
	}

	public void UpdateSliderValues() {
		slider1.value = dataHolder.GetMusicVolume ();
		slider2.value = dataHolder.GetAmbianceVolume ();
		slider3.value = dataHolder.GetSFXVolume ();
	}

	public void UpdateToggleValue() {
		headBobbingToggle.isOn = dataHolder.GetToggleValue ();
	}

	public void UpdateSensitivitySetting() {
		mouseSensitivitySider.value = dataHolder.mouseSensitivity;
	}
}