using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPSMovement : MonoBehaviour
{
	public CharacterController CC;
	public float MoveSpeed;
	private float sprint = 0f;
	public float Gravity = -9.8f;
	public float JumpSpeed;
	public bool sprinting;

	public float verticalSpeed;

	public new Light light;
	public float flickerdelay;
	public Vector2 intensityRange;
	public Vector2 delayRange;
	public float targetIntens;
	private bool canMove = true;

	//UI
	public Image Stamina;

	// Animations
	public Animator playerAnim;

    private void Start()
    {
		//playerAnim = GetComponent<Animator>();
    }

    private void Update()
	{
		//Flashlight flickering
		flickerdelay -= Time.deltaTime;
		if (flickerdelay <= 0)
		{
			light.intensity = Random.Range(intensityRange.x, intensityRange.y);
			flickerdelay = Random.Range(delayRange.x, delayRange.y);
		}
		light.intensity = Mathf.Lerp(light.intensity, targetIntens, Time.deltaTime * 6);

		if (canMove == true)
		{
			Vector3 movement = Vector3.zero;

			// X/Z movement
			float forwardMovement = Input.GetAxis("Vertical") * (MoveSpeed + sprint) * Time.deltaTime;
			float sideMovement = Input.GetAxis("Horizontal") * (MoveSpeed + sprint) * Time.deltaTime;

			movement += (transform.forward * forwardMovement) + (transform.right * sideMovement);

			//Sprinting
			if (Input.GetKey(KeyCode.LeftShift)) // are your sprinting?
            {
				sprinting = true;
            }
			else
            {
				sprinting = false;
            }

			if (sprinting && (forwardMovement != 0 || sideMovement != 0 )) //sprinting
            {
				playerAnim.SetBool("IsMoving", true);
				playerAnim.SetBool("IsSprinting", true);
				sprint = 3f;
				Stamina.fillAmount -= Time.deltaTime * 0.4f;
			}

			else
			{
				sprint = 0f;
				Stamina.fillAmount += Time.deltaTime * 0.06f;
			}

			if (Stamina.fillAmount == 0f)
			{
				sprint = 0f;
			}
			if(forwardMovement == 0 && sideMovement == 0 && !sprinting) // idle
            {
				playerAnim.SetBool("IsMoving", false);
				playerAnim.SetBool("IsSprinting", false);
			}
			if(!sprinting && (forwardMovement != 0 || sideMovement != 0)) // walking
            {
				playerAnim.SetBool("IsMoving", true);
				playerAnim.SetBool("IsSprinting", false);
			}


			//Jump
			if (CC.isGrounded)
			{
				verticalSpeed = 0f;
				if (Input.GetKeyDown(KeyCode.Space))
				{
					verticalSpeed = JumpSpeed;
				}
			}

			verticalSpeed += (Gravity * Time.deltaTime);
			movement += (transform.up * verticalSpeed * Time.deltaTime);

			CC.Move(movement);
		}
	
	}

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
		//Stop movement after winning
		outside ot = hit.collider.GetComponent<outside>();
		if (ot)
		{
			canMove = false;
		}
	}
}
