using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomedMinimapCamera : MonoBehaviour {
	
    public GameObject player;
    public Vector3 offset;

    private Camera mapCamera;
    private float storedShadowDistance;

    private void Start() {
        mapCamera = transform.GetComponent<Camera>();
        mapCamera.aspect = 1;
    }

    private void FixedUpdate() {
        transform.position = player.transform.position + offset;
    }

    private void OnPreRender() {
        storedShadowDistance = QualitySettings.shadowDistance;
        QualitySettings.shadowDistance = 0; // this camera doesn't show shadows
    }

    private void OnPostRender() {
        QualitySettings.shadowDistance = storedShadowDistance;
    }
}
