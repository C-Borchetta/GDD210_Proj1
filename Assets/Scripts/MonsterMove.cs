using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterMove : MonoBehaviour
{

	public Transform Target;
	//public float MoveSpeed;

	public Camera Cam;
	public NavMeshAgent agent;

	private void Start()
	{

	}

	private void Update()
	{
		//Vector3 moveDir = (Target.position - transform.position).normalized;
		//moveDir.y = 0; // Remove Y component
		//transform.position += (moveDir * MoveSpeed * Time.deltaTime);

		Ray ray = Cam.ScreenPointToRay(Target.position);
		RaycastHit hit;

		//Draws ray out towards Player, sets destination at that point
		if(Physics.Raycast(ray, out hit))
        {
			agent.SetDestination(hit.point);
			FPSMovement p = hit.transform.GetComponent<FPSMovement>();
			Debug.Log(hit.distance);

			if (hit.distance <= 2.8)
            {
				p.KillPlayer(true);
				
            }
        }

	}



}