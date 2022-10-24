using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
	public float MouseSensitivity;
	public Transform CamTransform;

	private float camRotation = 0f;

	//UI
	public GameObject Key1img;
	public bool gotkey1 = false;
	public GameObject winscreen;

	//Doors
	public GameObject Maindoors;
	 

	private void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;

		Key1img.SetActive(false);
		winscreen.SetActive(false);
	}

	private void Update()
	{
		//Camera rotation
		float mouseInputY = Input.GetAxis("Mouse Y") * MouseSensitivity;
		camRotation -= mouseInputY;
		camRotation = Mathf.Clamp(camRotation, -90f, 90f);
		CamTransform.localRotation = Quaternion.Euler(new Vector3(camRotation, 0f, 0f));

		float mouseInputX = Input.GetAxis("Mouse X") * MouseSensitivity;
		transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0f, mouseInputX, 0f));

		//Raycasting
		if(Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;

			if (Physics.Raycast(CamTransform.position, CamTransform.forward, out hit))
			{
				Debug.DrawLine(CamTransform.position + new Vector3(0f, -1f, 0f), hit.point, Color.green, 1f);
				Debug.Log(hit.collider.gameObject.name);

				//Key 1 pickup
				key1script k1 = hit.collider.GetComponent<key1script>();
				if(k1 != null)
                {
					Destroy(hit.collider.gameObject);
					Key1img.SetActive(true);
					gotkey1 = true;
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
		
		if(md && gotkey1 == true)
        {
			Maindoors.SetActive(false);
        }

		//Winning on outside
		outside ot = hit.collider.GetComponent<outside>();

        if (ot)
        {
			winscreen.SetActive(true);
		}

	}


}
