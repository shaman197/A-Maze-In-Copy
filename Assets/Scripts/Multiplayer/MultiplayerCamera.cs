using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerCamera : MonoBehaviour {

	private static MultiplayerCamera instance = null;
	public static MultiplayerCamera Instance {
		get
		{
			return instance;
		}
	}

    public Vector3 offset;
    public float speed = 1.0f;
    public float rotateMultiplier = 50f;
	public float mouseSensitivity = 7.5f;
    private float upRotationLimit = 300f;
    private float downRotationLimit = 70f;

    private MultiplayerMovement playerMovement;
    private bool paused;
    private Camera myCamera;

	private void Awake() {
		if (instance != null && instance != this) {
			Debug.Log ("Error - Er zijn op dit moment meerdere instanties van MultiplayerCamera (No Singleton)");
		} else {
			instance = this;
		}
	}

    private void Start() {
		myCamera = GetComponentInParent<Camera> ();
		if (PhotonNetwork.isMasterClient) {
			myCamera.cullingMask &= ~(1 << LayerMask.NameToLayer ("Player"));
		} 
		else {
			myCamera.cullingMask &= ~(1 << LayerMask.NameToLayer ("Player2"));
		}
	}

    private void FixedUpdate() {
        transform.rotation = Quaternion.Lerp(transform.rotation, CalculateWatchingRotation(), Time.deltaTime * speed);
    }

    private Quaternion CalculateWatchingRotation() {
        Vector3 rotEuler = transform.eulerAngles;
        rotEuler.x = ClampAngle(rotEuler.x - GetVerticalRotationInput(), upRotationLimit, downRotationLimit);
        rotEuler.y = playerMovement.GetHorizontalRotation();
        rotEuler.z = 0f;

        return Quaternion.Euler(rotEuler);
    }

	public void UpdateMouseSensitivitySetting() {
		mouseSensitivity = DataHolder.Instance.mouseSensitivity;
	}

    private float GetVerticalRotationInput() {
		if (paused) {
			return 0;
		}

		return Input.GetAxis ("Mouse Y") * mouseSensitivity;
	}

    public void SetPauzed(bool value) {
        paused = value;
    }

    public void SetPlayerMovement(GameObject player) {
        playerMovement = player.GetComponent<MultiplayerMovement>();
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