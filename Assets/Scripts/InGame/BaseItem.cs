using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseItem : MonoBehaviour {

    public event UnityAction<Collider> TriggerEnterDelegate;

    private void OnTriggerEnter(Collider collider) {
		if (TriggerEnterDelegate != null) {
			Destroy (this.gameObject);
			TriggerEnterDelegate (collider);
		}
	}
}
