using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{

    // Update is called once per frame
    public Transform Target;
    public float MoveSpeed;
	public Vector3 moveDir;

	private void Start()
	{
		enemyState = EnemyStateEnum.Idle;
		rb = GetComponent<Rigidbody>();
	}

	private void Update()
    {
        //Vector3 moveDir = (Target.position - transform.position).normalized;
        //moveDir.y = 0; // Remove Y component
        //transform.position += (moveDir * MoveSpeed * Time.deltaTime);

		switch (enemyState)
		{
			case EnemyStateEnum.Idle:
				UpdateIdle();
				break;
			case EnemyStateEnum.Patrol:
				UpdatePatrol();
				break;
			case EnemyStateEnum.Attack:
				UpdateAttack();
				break;
		}
	}

	public enum EnemyStateEnum
	{
		Idle,
		Patrol,
		Attack
	}
	[SerializeField] private EnemyStateEnum enemyState;
	[SerializeField] private FPSMovement player;

	private Vector3 lastSeenPosition;
	private Rigidbody rb;
	private float attackDelay;

	private void UpdateIdle()
	{
		if (TargetInLOS())
		{
			Debug.Log("Idle > Patrol");
			enemyState = EnemyStateEnum.Patrol;
		}
	}

	private void UpdatePatrol()
	{
		attackDelay -= Time.deltaTime;

		if (TargetInLOS())
		{
			lastSeenPosition = player.transform.position;
			if (Vector3.Distance(transform.position, lastSeenPosition) < 1f)
			{
				//Attack
				if (attackDelay <= 0f)
				{
					attackDelay = 1.5f;
					enemyState = EnemyStateEnum.Attack;
					moveDir = (lastSeenPosition - transform.position).normalized;
					moveDir.y = 0; // Remove Y component
					transform.position += (moveDir * MoveSpeed * Time.deltaTime);
				}
			}
		}
		else
		{
			if (Vector3.Distance(transform.position, lastSeenPosition) < 0.1f)
			{
				Debug.Log("Patrol > Idle");
				enemyState = EnemyStateEnum.Idle;
			}
		}

		moveDir = (lastSeenPosition - transform.position).normalized;
		moveDir.y = 0; // Remove Y component
		transform.position += (moveDir * MoveSpeed * Time.deltaTime);
	}

	private void UpdateAttack()
	{
		attackDelay -= Time.deltaTime;
		if (attackDelay <= 0f)
		{
			attackDelay = 1f;
			enemyState = EnemyStateEnum.Idle;
		}
	}

	private bool TargetInLOS()
	{
		Debug.DrawLine(transform.position, player.transform.position, Color.red);
		RaycastHit[] hits = Physics.RaycastAll(transform.position, player.transform.position - transform.position);
		if (hits.Length > 0)
		{
			if (hits[1].transform.GetComponent<FPSMovement>())
			{
				return true;
			}
		}
		return false;
	}
}
