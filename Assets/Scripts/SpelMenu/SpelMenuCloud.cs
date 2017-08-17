using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpelMenuCloud : MonoBehaviour {

	public float rotationSpeed;

	private void Start() {
		// Give a random rotation to the cloud when spelMenu starts
		Vector3 tempEuler = new Vector3(transform.eulerAngles.x, Random.Range(0f, 359f), transform.eulerAngles.z);
		transform.rotation = Quaternion.Euler (tempEuler);

		StartCoroutine (RotateCloud ());
	}

	private IEnumerator RotateCloud() {
		while (true) {
			Vector3 tempEuler = transform.eulerAngles;
			tempEuler.y += rotationSpeed;
			transform.rotation = Quaternion.Euler (tempEuler);

			yield return null;
		}
	}

	public void RemoveFog() {
		StartCoroutine (FogFadeOut ());
	}

	private IEnumerator FogFadeOut() {
		yield return new WaitForSecondsRealtime (1f);
		rotationSpeed = 10;
		bool isRemoving = true;

		while (isRemoving) {
			Vector3 scale = transform.localScale;
			scale.x *= 0.95f;
			scale.y *= 0.95f;
			transform.localScale = scale;

			if (transform.lossyScale.x <= 0.1) {
				gameObject.SetActive (false);
				isRemoving = false;
			}

			yield return null;
		}
	}

	private void OnMouseDown() {
		// For debug only
		if (Application.isEditor) {
			Debug.Log ("TODO: Hier moet een pop-up komen om cloud weg te kopen");

			RemoveFog ();
			transform.parent.GetComponent<GroundTile> ().isPlayable = true;
			TileManager.Instance.UpdateTilesLocal ();
		}
	}
}