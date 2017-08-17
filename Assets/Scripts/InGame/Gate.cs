using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {

    public Animator animator;
    public Transform pressurePlates;
    public int amountOfPressurePlatesToOpenDoor;

    private int pressedPressurePlateCount;
    private bool open;
	private bool alreadyOpenedDoor = false;

    private void Start() {
		if (pressurePlates) {
			StartCoroutine (AddPressurePlates ());
		}
	}

    private IEnumerator AddPressurePlates() {
		foreach (Transform child in pressurePlates) {
			PressurePlate pressurePlate = child.GetChild (1).GetComponent<PressurePlate> ();
			pressurePlate.TriggerEnterDelegate += (Collider c) => {
				pressedPressurePlateCount++;
				AudioController.Instance.PlaySound (Sounds.OpenDoor);
				ChangeDoorState ();
			};

			pressurePlate.TriggerExitDelegate += (Collider c) => {
				pressedPressurePlateCount--;
				AudioController.Instance.PlaySound (Sounds.CloseDoor);
				ChangeDoorState ();
			};
		}

		yield return null;
	}

    public void ChangeDoorState() {
		if (amountOfPressurePlatesToOpenDoor != 0) {
			if (pressedPressurePlateCount >= amountOfPressurePlatesToOpenDoor && !open) {
				animator.SetBool ("Open", true);
				open = true;


				UpdateDoorMission ();
			} 
			else if (pressedPressurePlateCount < amountOfPressurePlatesToOpenDoor && open) {
				animator.SetBool ("Open", false);
				open = false;
			}
		}

        // If amountOfPressurePlatesToOpen is 0, the default is all plates should be pressed
        else {
			if (pressedPressurePlateCount == pressurePlates.childCount && !open) {
				animator.SetBool ("Open", true);
				open = true;

				UpdateDoorMission ();
			} 
			else if (pressedPressurePlateCount != pressurePlates.childCount && open) {
				animator.SetBool ("Open", false);
				open = false;
			}
		}
	}

	private void UpdateDoorMission() {
		if (!alreadyOpenedDoor) {
			alreadyOpenedDoor = true;

            if(MissionController.Instance != null) {
                MissionController.Instance.PlayerOpenedNewDoor();
            }
		}
	}
}
