using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InGameInputController : MonoBehaviour {

	private static InGameInputController instance = null;
	public static InGameInputController Instance {
		get {
			return instance;
		}
	}
		
	private float moveHorizontal;
	private float moveVertical;
	private float jumpAirSpeed;
    private bool jumping;

	private void Awake() {
		if (instance != null && instance != this) {
			Debug.Log ("Error - Er zijn op dit moment meerdere instanties van InGameInputController (No Singleton)");
		}
		else {
			instance = this;
		}
	}

	private void Start() {
		jumpAirSpeed = InGamePlayerController.Instance.GetJumpAirSpeed ();
	}

	private void FixedUpdate() {
		if (Input.GetAxis ("Horizontal") != 0) {
			if (Input.GetAxis ("Horizontal") > 0) {
				moveHorizontal = 1;
			} 
			else if (Input.GetAxis ("Horizontal") < 0) {
				moveHorizontal = -1;
			}
		} 
		else {
			moveHorizontal = 0;
		}

		if (Input.GetAxis ("Vertical") != 0) {
			if (Input.GetAxis ("Vertical") > 0) {
				moveVertical = 1;
			} 
			else if (Input.GetAxis ("Vertical") < 0) {
				moveVertical = -1;
			}
		} 
		else {
			moveVertical = 0;
		}

		if (Input.GetButtonDown ("Jump")) {
			jumping = true;
		} 
		else {
			jumping = false;
		}
			
		if (Input.GetButton ("Running") && (Input.GetAxis ("Horizontal") == 1 || Input.GetAxis ("Vertical") == 1)) {
			MissionController.Instance.PlayerIsRunning ();
		} 
		else {
			MissionController.Instance.PlayerDoesNotRun ();
		}
	}

	public Vector3 GetInput() {
		Vector3 input = new Vector3 (0f, 0f, 0f);
		input.z = moveVertical;

		if (moveHorizontal != 0) {
			input.x = moveHorizontal;
		}
		if (moveVertical != 0) {
			input.z = moveVertical;
		}

		if (moveVertical != 0 && moveHorizontal != 0) {
			input.x = input.x * 0.71f;
			input.z = input.z * 0.71f;
		}

		if (!InGamePlayerController.Instance.GetCanJump()) {
			input.x = input.x * jumpAirSpeed;
			input.z = input.z * jumpAirSpeed;
		}

		return input;
	}

    public bool ShouldWalkFast() {
		if (Input.GetButton("Running")) {
			return true;
		}

		return false;
	}

    public bool getJumpInput()
    {
        return jumping;
    }
}
