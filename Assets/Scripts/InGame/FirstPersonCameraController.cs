using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCameraController : MonoBehaviour {

	private static FirstPersonCameraController instance = null;
	public static FirstPersonCameraController Instance {
		get
		{
			return instance;
		}
	}

    public Transform player;
    public Vector3 offset;
    public float speed = 1.0f;
	public float rotateMultiplier = 50f;

	public float restingBobbingSpeed = 0.05f;
	public float movingBobbingSpeed = 0.2f;
	public float bobbingHeight = 0.05f;

	private float upRotationLimit = 300f;
	private float downRotationLimit = 70f;
	private bool bobbingEnabled;
	private Vector3 bobbingToAdd;
	private float timer = 0.0f;
	private float midHeightPoint = 0f;
	private float mouseSensitivity = 7.5f;

	private void Awake() {
		if (instance != null && instance != this) {
			Debug.Log ("Error - Er zijn op dit moment meerdere instanties van FirstPersonCameraController (No Singleton)");
		} else {
			instance = this;
		}
	}

	private void Start() {
		StartCoroutine(MoveCamera());
		if (DataHolder.Instance != null) {
			bobbingEnabled = DataHolder.Instance.GetHeadBobbingBool ();
		}
	}

	public void ChangeHeadBobbingState(bool shouldBob) {
		bobbingEnabled = shouldBob;
	}

    private IEnumerator MoveCamera() {
        while (true) {
			if (InGamePlayerController.Instance.CanPlayerWalk()) {
				if (bobbingEnabled) {
					CalculateVerticalBobbingPosition ();
				}

				transform.position = Vector3.Lerp(transform.position, (player.position + offset) + bobbingToAdd, Time.deltaTime * speed);
				transform.rotation = Quaternion.Lerp(transform.rotation, CalculateWatchingRotation(), Time.deltaTime * speed);
			}

			yield return null;
        }
    }

	private Quaternion CalculateWatchingRotation() {
		Vector3 rotEuler = transform.eulerAngles;
		rotEuler.x = ClampAngle(rotEuler.x - GetVerticalRotationInput(), upRotationLimit, downRotationLimit);
		rotEuler.y = InGamePlayerController.Instance.GetHorizontalRotation ();
		rotEuler.z = 0f;

		Mathf.Clamp (rotEuler.x, 2f, 6f);
			
		return Quaternion.Euler(rotEuler);
	}

	public void UpdateMouseSensitivitySetting() {
		mouseSensitivity = DataHolder.Instance.mouseSensitivity;
	}
		
	private float GetVerticalRotationInput() {
		return Input.GetAxis("Mouse Y") * mouseSensitivity;
	}

	private void CalculateVerticalBobbingPosition() {
		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");

		float bobbingSpeed;
		if (horizontal == 0 && vertical == 0) {
			bobbingSpeed = restingBobbingSpeed;
		} 
		else {
			bobbingSpeed = movingBobbingSpeed;
		}

		float wavePoint = Mathf.Sin (timer);
		timer = timer + bobbingSpeed;
		if (timer > Mathf.PI * 2) {
			timer = timer - (Mathf.PI * 2);
		}

		float heightToBob = wavePoint * bobbingHeight;
		bobbingToAdd.y = midHeightPoint + heightToBob;
	}

    float ClampAngle(float angle, float from, float to) {
		if (angle > 180) {
			angle = Mathf.Clamp (angle, from, 361);
		} 
		else {
			angle = Mathf.Clamp (angle, -1, to);
		}

		return angle;
	}
}