using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour {

	private static TimerController instance = null;
	public static TimerController Instance {
		get {
			return instance;
		}
	}

    public float time;
    public Text timeText;

    private bool start;

    private void Awake () {
		if (instance != null && instance != this) {
			Debug.Log ("Error - Er zijn op dit moment meerdere instanties van TimerController (No Singleton)");
		}
		else {
			instance = this;
		}
    }

	private void Start() {
		time = 0.0f;
		start = false;
	}

    private void FixedUpdate () {
        if (start) {
            time += Time.deltaTime;

            timeText.text = string.Format("{0:00}:{1:00}", (int) time / 60, (int) time % 60);
        }
    }

    public void StartTimer() {
        start = true;
    }

    public void StopTimer() {
        start = false;
    }
}