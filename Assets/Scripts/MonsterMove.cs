using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{

    // Update is called once per frame
    public Transform Target;
    public float MoveSpeed;

    private void Update()
    {
        Vector3 moveDir = (Target.position - transform.position).normalized;
        moveDir.y = 0; // Remove Y component
        transform.position += (moveDir * MoveSpeed * Time.deltaTime);
        
    }
}
