using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour {

	private void Start() {
		StartCoroutine (ObjectRotation());
	}

	private IEnumerator ObjectRotation() {
		while (true) {
			transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
			yield return null;
		}
	}
}