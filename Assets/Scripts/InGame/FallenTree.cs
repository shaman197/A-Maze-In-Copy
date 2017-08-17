using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenTree : MonoBehaviour {

    public Vector3 fallingAxis;
    public float fallingSpeed = 5;
    public float stareTimeForFalling = 2;
    public float maxfallingAngle = 90;
    public FallenTreeAreaCollider areaCollider;
    public Camera firstPersonCamera;
    public bool resetTree;

    private Quaternion startRotation;
    private bool isFalling;
    private bool canFall;
    private float stareTime;

    private void Start()
    {
		startRotation = transform.rotation;
        stareTime = 0;
    }

    private void FixedUpdate() {
		Vector3 screenView = firstPersonCamera.WorldToViewportPoint (transform.position);

		// Alle condities om te garanderen dat de boom in beeld is
		if ((screenView.z > 0 && screenView.x > 0 && screenView.x < 1 && screenView.y > 0 && screenView.y < 1) || resetTree) {
			stareTime += Time.deltaTime;

			if (!isFalling && ((canFall && (stareTime > stareTimeForFalling)) || resetTree)) {
				isFalling = true;
				AudioController.Instance.PlaySound (Sounds.FallingTree);
				StartCoroutine (FallingTree ());
				stareTime = 0;
			}
		} 
		else {
			if (stareTime > 0) {
				stareTime = 0;
			}
		}
	}

    public void SetCanFall(bool value) {
        canFall = value;
    }

    private IEnumerator FallingTree() {
		float currentSpeed = fallingSpeed / 50;
		float currentFallingAngle = 0;

		while (currentFallingAngle < maxfallingAngle) {
			transform.Rotate (fallingAxis, Mathf.Clamp (currentSpeed * currentFallingAngle, 1, fallingSpeed));
			currentFallingAngle = Quaternion.Angle (startRotation, transform.rotation);

			yield return null;
		}

		// For testing purpose
		if (resetTree) {
			StartCoroutine (ResetTree ());
		}

		yield return null;
	}

    private IEnumerator ResetTree() {
        yield return new WaitForSecondsRealtime(1f);
		transform.rotation = startRotation;
        yield return new WaitForSecondsRealtime(1f);
        isFalling = false;
    }
}