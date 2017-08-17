using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerMarker : MonoBehaviour {

    public GameObject myPlayer;
    public GameObject otherPlayer;
    public Vector3 positionOffset;
    public Vector3 rotationOffset;
    public float distanceFromMyPlayer = 3;
	
	void FixedUpdate () {

        if(myPlayer != null && otherPlayer != null)
        {
            transform.position = Vector3.Lerp(myPlayer.transform.position + positionOffset, otherPlayer.transform.position + positionOffset, Time.deltaTime * distanceFromMyPlayer);
            transform.LookAt(otherPlayer.transform);
            transform.rotation *= Quaternion.Euler(rotationOffset);
        }
    }
}
