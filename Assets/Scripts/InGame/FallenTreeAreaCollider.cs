using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenTreeAreaCollider : MonoBehaviour {

    public FallenTree fallenTree;
    public GameObject player;

    private void OnTriggerEnter(Collider col) {
		if (col.gameObject == player) {
			fallenTree.SetCanFall (true);
		}
	}

    private void OnTriggerExit(Collider col) {
		if (col.gameObject == player) {
			fallenTree.SetCanFall (false);
		}
	}
}