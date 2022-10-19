using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPSMovement : MonoBehaviour
{
	public CharacterController CC;
	public float MoveSpeed;
	public float Gravity = -9.8f;
	public float JumpSpeed;

	public float verticalSpeed;

	public new Light light;
	public float flickerdelay;
	public Vector2 intensityRange;
	public Vector2 delayRange;
	public float targetIntens;

	private void Update()
	{
		flickerdelay -= Time.deltaTime;
		if(flickerdelay <= 0)
        {
			light.intensity = Random.Range(intensityRange.x, intensityRange.y);
			flickerdelay = Random.Range(delayRange.x, delayRange.y);
        }

		light.intensity = Mathf.Lerp(light.intensity, targetIntens, Time.deltaTime * 6);
		

		Vector3 movement = Vector3.zero;

		// X/Z movement
		float forwardMovement = Input.GetAxis("Vertical") * MoveSpeed * Time.deltaTime;
		float sideMovement = Input.GetAxis("Horizontal") * MoveSpeed * Time.deltaTime;

		movement += (transform.forward * forwardMovement) + (transform.right * sideMovement);

		if (CC.isGrounded)
		{
			verticalSpeed = 0f;
			if(Input.GetKeyDown(KeyCode.Space))
			{
				verticalSpeed = JumpSpeed;
			}
		}
		
		verticalSpeed += (Gravity * Time.deltaTime);
		movement += (transform.up * verticalSpeed * Time.deltaTime);

		CC.Move(movement);
	}
}
