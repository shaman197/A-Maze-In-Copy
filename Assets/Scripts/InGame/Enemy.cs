using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    public float locationDeviation = 0.5f;
    public float caughtRange = 2.0f;
    //public float audioRange = 20.0f;
    public EndLevelController endlevelController;
    public bool caught;
	public GameObject player;
    public Vector3[] locations;
    public float warningRange = 10f;
    public WarningSign warningSign;

    private NavMeshAgent enemyAgent;
	private Animator animator;
	private bool enemyCanWalk = true;
    private int locationCount = 0;

	private AudioSource audioSource;

    private void Start() {
        enemyAgent = transform.GetComponent<NavMeshAgent>();
        animator = transform.GetComponent<Animator>();
		audioSource = transform.GetComponent <AudioSource>();

        enemyAgent.autoBraking = false;

        GoToNextLocation();
        FreezeMovement();
    }

	private void FixedUpdate() {

		if (!caught && enemyCanWalk) {

			Vector3 dir = transform.position - enemyAgent.destination;
			Debug.DrawRay(transform.position, -dir, Color.red); // For debug: Ray to see towards which location the hunter is walking

            if (!enemyAgent.pathPending && enemyAgent.remainingDistance < locationDeviation)
            {
                GoToNextLocation();
            }

            if (Vector3.Distance(transform.position, player.transform.position) < caughtRange) {
                playerCaught();
            }

            if (Vector3.Distance(transform.position, player.transform.position) < warningRange)
            {
                warningSign.StartWarning();
            }
        }
	}

	public void SetAudioVolume(float volume) {
		audioSource.volume = volume;
	}

	private void GoToNextLocation() {
		animator.SetFloat ("Speed", 1);
        enemyAgent.destination = locations[locationCount];
        locationCount = (locationCount + 1) % locations.Length;
    }

    private void playerCaught() {
        FreezeMovement();
		AudioController.Instance.PlaySound (Sounds.FailingLevel);
        caught = true;
        InGamePlayerController.Instance.PlayCaughtAnimation();
        transform.LookAt(player.transform);
        endlevelController.ShowFailed();
        PauseGameController.Instance.FreezeGame ();
    }

	public void SetWalkableEnemyBool(bool canWalk) {
		enemyCanWalk = canWalk;
	}

    public void FreezeMovement() {
		animator.SetFloat("Speed", 0);
		enemyAgent.velocity = Vector3.zero;
		enemyAgent.Stop();
		SetWalkableEnemyBool(false);
		audioSource.Stop ();
    }

    public void UnFreezeMovement() {
		enemyAgent.Resume();
		animator.SetFloat("Speed", 1);
		SetWalkableEnemyBool(true);
		audioSource.Play ();
    }
}