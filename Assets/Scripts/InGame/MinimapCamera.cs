using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour {

    public Transform mazeFloor;
    public Vector3 offset;

    private Camera mapCamera;
    private Vector3 mapCenter;
    private Vector3 mapExtends;
    private float storedShadowDistance;

    private void Start() {
        mapCamera = transform.GetComponent<Camera>();

        calculateCenter();
        transform.position = mapCenter + offset;

        float cameraSize = (mapExtends.x > mapExtends.z) ? mapExtends.x : mapExtends.z;
        mapCamera.orthographicSize = cameraSize * 1.1f;
        mapCamera.aspect = 1;
    }

    private void calculateCenter() {
        Vector3 min = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
        Vector3 max = new Vector3(Mathf.NegativeInfinity, Mathf.NegativeInfinity, Mathf.NegativeInfinity);

        foreach (Transform tile in mazeFloor)
        {
            Vector3 tileMin = tile.GetComponent<Renderer>().bounds.min;
            Vector3 tileMax = tile.GetComponent<Renderer>().bounds.max;

            if (tileMin.x < min.x)
                min.x = tileMin.x;
            if (tileMin.y < min.y)
                min.y = tileMin.y;
            if (tileMin.z < min.z)
                min.z = tileMin.z;

            if (tileMax.x > max.x)
                max.x = tileMax.x;
            if (tileMax.y > max.y)
                max.y = tileMax.y;
            if (tileMax.z > max.z)
                max.z = tileMax.z;
        }

        mapCenter = (max + min) / 2;
        mapExtends = max - mapCenter;
    }

    private void OnPreRender() {
        storedShadowDistance = QualitySettings.shadowDistance;
        QualitySettings.shadowDistance = 0; // this camera doesn't show shadows
    }

    private void OnPostRender() {
        QualitySettings.shadowDistance = storedShadowDistance;
    }
}
