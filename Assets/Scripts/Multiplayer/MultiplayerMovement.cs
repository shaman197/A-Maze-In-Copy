using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerMovement : Photon.PunBehaviour {

	private static MultiplayerMovement instance = null;
	public static MultiplayerMovement Instance {
		get
		{
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
    public float jumpUpPower = 6.5f;
    public float fallingSpeedMultplier = 3.0f;
    public float jumpAirSpeed = 0.5f;
    public Vector3 jumpColliderDirection;
    public float waitTimeBetweenJump = 0.5f;
    public Animator animator;
    public Vector3 positionOffset;

    private Rigidbody rb;
    private bool canRegenEnergy;
    private float distance = 0;
    private Vector3 lastPos;
    private bool canWalk = true;
    private bool canJump = true;
    private CapsuleCollider bodyCollider;
    private float radius = 0.3f;
    private float jumpRaycastDistance = 1f;
    private bool waitingForCanJump;
    private PhotonView myPhotonView;
	private float mouseSensitivity = 7.5f;

	private void Awake() {
		if (instance != null && instance != this) {
			Debug.Log ("Error - Er zijn op dit moment meerdere instanties van MultiplayerMovement (No Singleton)");
		} else {
			instance = this;
		}
	}

    private void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        bodyCollider = transform.GetComponent<CapsuleCollider>();
        myPhotonView = GetComponent<PhotonView>();

        StartCoroutine(EnergyRegen());
    }

    private void FixedUpdate()
    {
        if(myPhotonView.isMine && canWalk)
        {
            bool isRunning = ShouldRun();

            Vector3 movement = CalculateMovement();

            animator.SetFloat("HorizontalSpeed", movement.z);
            animator.SetFloat("VerticalSpeed", movement.x);

            if (isRunning && EnergyBarController.Instance.GetEnergyValue() > 0)
            {
                rb.AddRelativeForce(movement * runningSpeedMultiplier);
            }
            else
            {
                rb.AddRelativeForce(movement * walkingSpeedMultiplier);
            }

            canJump = CanJumpCheck();

            if (Input.GetButtonDown("Jump") && canJump)
            {
                StartCoroutine(WaitTillCanJump());
                // Editing velocity more consistant for jumping
                rb.velocity = new Vector3(rb.velocity.x, jumpUpPower, rb.velocity.z);
            }

            AddFallingForce();

            float tempDistance = Vector3.Distance(lastPos, transform.position);
            distance += tempDistance;
            if (distance >= decreaseEnergyDistance)
            {
                if (isRunning && EnergyBarController.Instance.GetEnergyValue() > 0)
                {
                    EnergyBarController.Instance.DecreaseEnergy(runningEnergyPerDistance);
                }
                else
                {
                    EnergyBarController.Instance.DecreaseEnergy(walkingEnergyPerDistance);
                }

                distance = 0;
            }
            lastPos = transform.position;

            Quaternion rotation = CalcutateRotation();
            transform.localRotation = Quaternion.Lerp(transform.localRotation, rotation, Time.deltaTime * rotateMultiplier);
        }

        else if(!canWalk)
        {
            animator.SetFloat("HorizontalSpeed", 0);
            animator.SetFloat("VerticalSpeed", 0);
        }
    }

    private Vector3 CalculateMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 input = new Vector3(0f, 0f, 0f);
        input.z = Input.GetAxis("Vertical");

        if (moveHorizontal != 0)
        {
            input.x = moveHorizontal;
        }
        if (moveVertical != 0)
        {
            input.z = moveVertical;
        }

        if (moveVertical != 0 && moveHorizontal != 0)
        {
            input.x = input.x * 0.71f;
            input.z = input.z * 0.71f;
        }

        if (!canJump)
        {
            input.x = input.x * jumpAirSpeed;
            input.z = input.z * jumpAirSpeed;
        }

        return input;
    }

    private bool ShouldRun()
    {
        if (Input.GetButton("Running"))
        {
            return true;
        }

        return false;
    }

    private IEnumerator EnergyRegen()
    {
        bool didWalkLastFrame = true;
        while (true)
        {

            if (canRegenEnergy)
            {
                Vector3 position = this.transform.position;

                if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
                {
                    if (didWalkLastFrame)
                    {
                        yield return new WaitForSeconds(standardTimeBeforeRegen);
                        didWalkLastFrame = false;
                    }

                    // Calculate Time between energy regen
                    float waitTime = Mathf.Lerp(0f, 4f, (EnergyBarController.Instance.GetEnergyValue() / 100));
                    yield return new WaitForSeconds(waitTime);

                    if (position == this.transform.position)
                    {
                        EnergyBarController.Instance.AddEnergy(EnergyBarController.Instance.addEnergyOnWait);
                    }
                }
                else
                {
                    didWalkLastFrame = true;
                }
            }

            yield return null;
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        // Simulate force, so not master can also push the rock
        if (col.gameObject.CompareTag("Rock"))
        {
            Rigidbody rb = col.gameObject.GetComponent<Rigidbody>();
            Vector3 dir = col.contacts[0].point - transform.position;
            col.gameObject.GetComponent<Rigidbody>().AddForce(dir * 100);
        }
    }

    public float GetJumpAirSpeed()
    {
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

    public float GetHorizontalRotation()
    {
        return transform.eulerAngles.y;
    }

    public void SetWalkable(bool value)
    {
        canWalk = value;
    }

    public void AddFallingForce()
    {
        if (lastPos.y > transform.position.y && (lastPos.y - transform.position.y) > 0.01f)
        {
            rb.AddForce(Vector3.down * Mathf.Abs(rb.velocity.y * fallingSpeedMultplier));
        }
    }

    private bool CanJumpCheck()
    {
        if (!waitingForCanJump)
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.TransformPoint(Vector3.zero) + positionOffset, radius, jumpColliderDirection, jumpRaycastDistance);

            if (hits.Length >= 2)
            {
                foreach (RaycastHit hit in hits)
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
    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawSphere(positionOffset + transform.TransformPoint(jumpColliderDirection * jumpRaycastDistance), radius);
    //}
}
