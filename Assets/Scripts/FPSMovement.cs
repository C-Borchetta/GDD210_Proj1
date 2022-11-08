using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPSMovement : MonoBehaviour
{
	//Movement
	public CharacterController CC;
	public float MoveSpeed;
	private float sprint = 0f;
	public bool sprinting;
	private bool canMove = true;

	//Flashlight
	public new Light light;
	public float flickerdelay;
	public Vector2 intensityRange;
	public Vector2 delayRange;
	public float targetIntens;

	//UI
	public Image Stamina;
	public GameObject winscreen;
	public GameObject losescreen;

	// Animations
	public Animator playerAnim;

    private void Start()
    {
		//playerAnim = GetComponent<Animator>();

		winscreen.SetActive(false);
		losescreen.SetActive(false);
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

		//All movement within bool
		if (canMove == true)
		{
			Vector3 movement = Vector3.zero;

			// X/Z movement
			float forwardMovement = Input.GetAxis("Vertical") * (MoveSpeed + sprint) * Time.deltaTime;
			float sideMovement = Input.GetAxis("Horizontal") * (MoveSpeed + sprint) * Time.deltaTime;

			movement += (transform.forward * forwardMovement) + (transform.right * sideMovement);

			//Sprinting
			if (Input.GetKey(KeyCode.LeftShift)) // are you sprinting?
            {
				sprinting = true;
            }
			else
            {
				sprinting = false;
            }

			if (sprinting && (forwardMovement != 0 || sideMovement != 0 )) //sprinting
            {
				playerAnim.SetBool("IsMoving", false);
				playerAnim.SetBool("IsSprinting", true);
				sprint = 3f;
				Stamina.fillAmount -= Time.deltaTime * 0.35f;
			}

			else
			{
				sprint = 0f;
				Stamina.fillAmount += Time.deltaTime * 0.05f;
			}

			if (Stamina.fillAmount == 0f)
			{
				sprint = 0f;
				playerAnim.SetBool("IsMoving", true);
				playerAnim.SetBool("IsSprinting", false);
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

			CC.Move(movement);
		}

		//Raycast from player
	
	}

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
		//Winning
		outside ot = hit.collider.GetComponent<outside>();
		if (ot)
		{
			canMove = false;
			winscreen.SetActive(true);
		}

		//Losing
		MonsterMove mm = hit.collider.GetComponent<MonsterMove>();
		
		if (mm)
		{
			canMove = false;
			losescreen.SetActive(true);
		}
	}
}
