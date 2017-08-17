using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpelMenuInputController : MonoBehaviour {

	private Dictionary<KeyCode, Func<Action>> keyDownEvents = new Dictionary<KeyCode, Func<Action>>();

	private void Start() {
		AddInputKeys ();
	}

	private void AddInputKeys() {

		// ArrowKeys
		keyDownEvents.Add(KeyCode.UpArrow, delegate(){
			MovePlayerCall("upwards");
			return null;
		});
		keyDownEvents.Add(KeyCode.RightArrow, delegate(){
			MovePlayerCall("rightwards");
			return null;
		});
		keyDownEvents.Add(KeyCode.DownArrow, delegate(){
			MovePlayerCall("downwards");
			return null;
		});
		keyDownEvents.Add(KeyCode.LeftArrow, delegate(){
			MovePlayerCall("leftwards");
			return null;
		});

		// WASD Keys
		keyDownEvents.Add(KeyCode.W, delegate(){
			MovePlayerCall("upwards");
			return null;
		});
		keyDownEvents.Add(KeyCode.D, delegate(){
			MovePlayerCall("rightwards");
			return null;
		});
		keyDownEvents.Add(KeyCode.S, delegate(){
			MovePlayerCall("downwards");
			return null;
		});
		keyDownEvents.Add(KeyCode.A, delegate(){
			MovePlayerCall("leftwards");
			return null;
		});
	}

	private void MovePlayerCall(string direction) {
		SpelMenuPlayerController.Instance.CheckForValidMove (direction);
	}

	private void Update() {
		foreach (var keyDownEvent in keyDownEvents) {
			if (Input.GetKeyDown(keyDownEvent.Key)) {
				keyDownEvent.Value ();
			}
		}
	}
}
