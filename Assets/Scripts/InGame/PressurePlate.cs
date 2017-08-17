using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour {

    public Animator animator;

    public event UnityAction<Collider> TriggerEnterDelegate;
    public event UnityAction<Collider> TriggerExitDelegate;

    private void OnTriggerEnter(Collider other) {
        animator.SetBool("Pressed", true);
		AudioController.Instance.PlaySound (Sounds.PressPlate);
        TriggerEnterDelegate(other);
    }

    private void OnTriggerExit(Collider other) {
        animator.SetBool("Pressed", false);
		AudioController.Instance.PlaySound (Sounds.PressPlate);
        TriggerExitDelegate(other);
    }
}