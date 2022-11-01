using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPSController : MonoBehaviour
{
	public float MouseSensitivity;
	public Transform CamTransform;

	private float camRotation = 0f;
	private float Shaking = 0f;
	public float speed;
	public float distance;

	//UI
	public GameObject Key1img;
	public GameObject Key2img;
	public GameObject Key3img;
	public bool gotkey1 = false;
	public bool gotkey2 = false;
	public bool gotkey3 = false;
	public GameObject winscreen;
	public Image Stamina;

	//Doors
	public GameObject Maindoors;
	public GameObject SilverDoor;
	public GameObject BronzeDoor;

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;

		Key1img.SetActive(false);
		Key2img.SetActive(false); 
		Key3img.SetActive(false);
		winscreen.SetActive(false);
	}

	private void Update()
	{
		//Camera rotation
		float mouseInputY = Input.GetAxis("Mouse Y") * MouseSensitivity;
		camRotation -= mouseInputY;
		camRotation = Mathf.Clamp(camRotation, -60f, 70f) + Shaking;
		CamTransform.localRotation = Quaternion.Euler(new Vector3(camRotation, 0f, 0f));

		float mouseInputX = Input.GetAxis("Mouse X") * MouseSensitivity;
		transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0f, mouseInputX, 0f));

		//Screen shake while sprinting
		if(Input.GetKey(KeyCode.LeftShift) && Stamina.fillAmount != 0)
        {
			Shaking = Mathf.Sin(Time.time * speed) * distance;
        }
        else
        {
			Shaking = 0;
        }

		//Raycasting
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;

			if (Physics.Raycast(CamTransform.position, CamTransform.forward, out hit))
			{
				Debug.DrawLine(CamTransform.position + new Vector3(0f, -1f, 0f), hit.point, Color.green, 1f);
				Debug.Log(hit.collider.gameObject.name);

				//Key 1 pickup
				key1script k1 = hit.collider.GetComponent<key1script>();
				if (k1 != null)
				{
					Destroy(hit.collider.gameObject);
					Key1img.SetActive(true);
					gotkey1 = true;
				}

				//Key 2 pickup
				key2script k2 = hit.collider.GetComponent<key2script>();
				if (k2 != null)
				{
					Destroy(hit.collider.gameObject);
					Key2img.SetActive(true);
					gotkey2 = true;
				}

				//Key 3 pickup
				key3script k3 = hit.collider.GetComponent<key3script>();
				if (k3 != null)
				{
					Destroy(hit.collider.gameObject);
					Key3img.SetActive(true);
					gotkey3 = true;
				}
			}
			else
			{
				Debug.DrawRay(CamTransform.position + new Vector3(0f, -1f, 0f), CamTransform.forward * 100f, Color.red, 1f);
			}
		}

	}

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
		//Main Door opening
		maindoor md = hit.collider.GetComponent<maindoor>();

		if (md && gotkey1 == true)
		{
			Maindoors.SetActive(false);
			//Key1img.SetActive(false);
		}

		//Silver door opening
		Silverdoor sd = hit.collider.GetComponent<Silverdoor>();

		if (sd && gotkey2 == true)
		{
			SilverDoor.SetActive(false);
			//Key2img.SetActive(false);
		}

		//Bronze door opening
		Bronzedoor bd = hit.collider.GetComponent<Bronzedoor>();

		if (bd && gotkey3 == true)
		{
			BronzeDoor.SetActive(false);
			//Key3img.SetActive(false);
		}


		//Winning on outside
		outside ot = hit.collider.GetComponent<outside>();

		if (ot)
		{
			winscreen.SetActive(true);
		}

	}
}
