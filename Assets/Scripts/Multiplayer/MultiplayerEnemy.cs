using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class MultiplayerEnemy : Photon.PunBehaviour {

    public MultiplayerPause pauzeController;
    public float locationDeviation = 0.5f;
    public float caughtRange = 2.0f;
    public float audioRange = 20.0f;
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
	private AudioSource enemyAudioSource;

    private void Awake() {
        // Sometimes the enemy controller is faster than this start
        enemyAgent = transform.GetComponent<NavMeshAgent>();
        animator = transform.GetComponent<Animator>();
    }

    private void Start() {
        enemyAgent.autoBraking = false;
		enemyAudioSource = transform.GetComponent<AudioSource> ();

        GoToNextLocation();
        FreezeMovement();
    }

    private void FixedUpdate() {
		float speed = Vector3.Project (enemyAgent.desiredVelocity, transform.forward).magnitude;
		animator.SetFloat ("Speed", speed);

		if (!caught && enemyCanWalk) {

			Vector3 dir = transform.position - enemyAgent.destination;
			Debug.DrawRay (transform.position, -dir, Color.red);    // For debug: Ray to see towards which location the hunter is walking

            if (!enemyAgent.pathPending && enemyAgent.remainingDistance < locationDeviation)
            {
                GoToNextLocation();
            }

            if (Vector3.Distance (transform.position, player.transform.position) < caughtRange) {
				PlayerCaughtForAll ();
			}

            if (Vector3.Distance(transform.position, player.transform.position) < warningRange)
            {
                warningSign.StartWarning();
            }
        }
	}

    private void GoToNextLocation() {
        animator.SetFloat("Speed", 1);
        enemyAgent.destination = locations[locationCount];
        locationCount = (locationCount + 1) % locations.Length;
    }

    [PunRPC]
    private void PlayerCaught() {
        FreezeMovement();
        caught = true;
        transform.LookAt(player.transform);
        endlevelController.ShowFailed();
        pauzeController.FreezeGame();
    }

    [PunRPC]
    private void PlayerUnCaught()
    {
        UnFreezeMovement();
        caught = false;
        endlevelController.HideFailed();
        pauzeController.UnFreezeGame();
    }

	public void SetAudioVolume(float volume) {
		enemyAudioSource.volume = volume;
	}

    public void PlayerCaughtForAll() {
        photonView.RPC("PlayerCaught", PhotonTargets.All);
    }

    public void PlayerUnCaughtForAll()
    {
        photonView.RPC("PlayerUnCaught", PhotonTargets.All);
    }

    public void SetWalkableEnemyBool(bool canWalk) {
        enemyCanWalk = canWalk;
    }

    public void FreezeMovement() {
        animator.SetFloat("Speed", 0);
        enemyAgent.velocity = Vector3.zero;
        enemyAgent.Stop();
        SetWalkableEnemyBool(false);
		enemyAudioSource.Stop ();
    }

    public void UnFreezeMovement() {
        enemyAgent.Resume();
        animator.SetFloat("Speed", 1);
        SetWalkableEnemyBool(true);
		enemyAudioSource.Play ();
    }
}