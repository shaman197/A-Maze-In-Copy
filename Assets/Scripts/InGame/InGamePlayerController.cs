using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGamePlayerController : MonoBehaviour
{
	private static InGamePlayerController instance = null;
	public static InGamePlayerController Instance {
		get {
			return instance;
		}
	}
		
	public float standardTimeBeforeRegen = 2f;
	public float decreaseEnergyDistance = 5.0f;
	public float rotateMultiplier = 40.0f;
	public float walkingSpeedMultiplier = 25f;
	public float runningSpeedMultiplier = 35f;
	public float walkingEnergyPerDistance = 0.5f;
	public float runningEnergyPerDistance = 2f;
	public float sideWaysPercentage = 0.5f;
	public float jumpAirSpeed = 0.5f;
    public float jumpPower = 6.5f;
    public float fallingSpeedMultplier = 3f;
    public float radius = 0.45f;
    public Vector3 jumpColliderDirection = new Vector3(0, -0.5f, 0.5f);
    public float waitTimeBetweenJump = 0.5f;
    public Slider slider;
    public Animator animator;

    private bool canRegenEnergy;
	private Rigidbody rb;
	private float distance = 0;
	private Vector3 lastPos;
	private bool canWalk;
    private bool canJump = true;
	private float mouseSensitivity = 7.5f;
    private CapsuleCollider bodyCollider;
    private bool waitingForCanJump;

    private void Awake() {
		if (instance != null && instance != this) {
			Debug.Log ("Error - Er zijn op dit moment meerdere instanties van InGamePlayerController (No Singleton)");
		}
		else {
			instance = this;
		}
	}

	private void Start() {
		rb = transform.GetComponent<Rigidbody>();
        bodyCollider = transform.GetComponent<CapsuleCollider>();
        lastPos = transform.position;
        
        StartCoroutine(EnergyRegen());
	}

    private void FixedUpdate() {
		if (canWalk) {

            bool isRunning = InGameInputController.Instance.ShouldWalkFast ();

			Vector3 movement = InGameInputController.Instance.GetInput();

            animator.SetFloat("HorizontalSpeed", movement.z);
            animator.SetFloat("VerticalSpeed", movement.x);

            if (isRunning && slider.value > 0) {
                rb.AddRelativeForce(movement * runningSpeedMultiplier);
            } 
			else {
                rb.AddRelativeForce(movement * walkingSpeedMultiplier);
            }

            canJump = CanJumpCheck();

            if (InGameInputController.Instance.getJumpInput() && canJump)
            {
                StartCoroutine(WaitTillCanJump());
                // Editing velocity more consistant for jumping
                rb.velocity = new Vector3(rb.velocity.x, jumpPower, rb.velocity.z);
                MissionController.Instance.PlayerHasJumped();
            }

            AddFallingForce();

            float tempDistance = Vector3.Distance (lastPos, transform.position);
			distance += tempDistance;

            if (distance >= decreaseEnergyDistance) {
				if (isRunning && slider.value > 0) {
                    EnergyBarController.Instance.DecreaseEnergy(runningEnergyPerDistance);
                } 
				else {
                    EnergyBarController.Instance.DecreaseEnergy(walkingEnergyPerDistance);
                }

				distance = 0;
			}
			lastPos = transform.position;

            Quaternion rotation = CalcutateRotation ();
			transform.localRotation = Quaternion.Lerp (transform.localRotation, rotation, Time.deltaTime * rotateMultiplier);
		}
	}

    private void OnCollisionEnter(Collision col) {
        // Simulate force
        if (col.gameObject.CompareTag("Rock"))
        {
            Rigidbody rb = col.gameObject.GetComponent<Rigidbody>();
            Vector3 dir = col.contacts[0].point - transform.position;
            rb.AddForce(dir * 100);
        }
    }

    public float GetJumpAirSpeed() {
		return jumpAirSpeed;
	}

	private Quaternion CalcutateRotation()
	{
		Vector3 rotEuler = transform.eulerAngles;
		rotEuler.y = rotEuler.y + getSensitiveMouseInput();
		return Quaternion.Euler(rotEuler);
	}

	public void UpdateMouseSensitivitySetting() {
		mouseSensitivity = DataHolder.Instance.mouseSensitivity;
	}

	private float getSensitiveMouseInput()
	{
		return Input.GetAxis("Mouse X") * mouseSensitivity;
	}

    private IEnumerator EnergyRegen() {
		bool didWalkLastFrame = true;
        while (true) {
			
			if (canRegenEnergy) {
				Vector3 position = this.transform.position;

				if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) {
					if (didWalkLastFrame) {
						yield return new WaitForSeconds(standardTimeBeforeRegen);
						didWalkLastFrame = false;
					}

					// Calculate Time between energy regen
					float waitTime = Mathf.Lerp (0f, 4f, (slider.value/100));
					yield return new WaitForSeconds(waitTime);

					if (position == this.transform.position) {
						EnergyBarController.Instance.AddEnergy (EnergyBarController.Instance.addEnergyOnWait);
					}
				} 
				else {
					didWalkLastFrame = true;
				}
			}
				
            yield return null;
        }
    }

    public void AddFallingForce()
    {
        if (lastPos.y > transform.position.y && (lastPos.y - transform.position.y) > 0.001f)
        {
            rb.AddForce(Vector3.down * Mathf.Abs(rb.velocity.y * fallingSpeedMultplier));
        }
    }

    public void SetPlayerWalkable() {
		canWalk = true;
	}

	public void StopPlayerWalking() {
		canWalk = false;
	}

	public bool GetCanJump() {
		return canJump;
	}

	public void SetRegenBool(bool canRegen) {
		canRegenEnergy = canRegen;
	}

    public Vector3 getPosition()
    {
        return this.transform.position;
    }

    public void PlayCaughtAnimation()
    {
        animator.SetBool("Caught", true);
    }

	public float GetHorizontalRotation() {
		return transform.eulerAngles.y;
	}

	public bool CanPlayerWalk() {
		return canWalk;
	}

    private bool CanJumpCheck()
    {
        if(!waitingForCanJump)
        {
            Collider[] hits = Physics.OverlapSphere(transform.TransformPoint(jumpColliderDirection) + bodyCollider.center, radius);

            if (hits.Length >= 2)
            {
                foreach (Collider hit in hits)
                {
                    if (hit.transform.name == "WaterProDaytime")
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        return false;
    }

    private IEnumerator WaitTillCanJump()
    {
        waitingForCanJump = true;

        yield return new WaitForSecondsRealtime(waitTimeBetweenJump);

        waitingForCanJump = false;
    }

    // Calulation where the sphereCast area is
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(bodyCollider.center + transform.TransformPoint(jumpColliderDirection), radius);
    }
}