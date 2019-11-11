using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //-- Prefab
    public Transform target;

    //-- Twerkable
    public float speed = 5;
    public float dodgeWeight = 3;
    public float detection_range;
    public float fov_angle = 90;
    public Vector3 moveDir;
    Vector3 originalDir;


    private void Start()
    {
        originalDir = moveDir;
    }

    void Test1()
    {
        Vector3 headingVector = transform.position - target.position;

        float scale = headingVector.magnitude / detection_range;

        moveDir += headingVector.normalized / scale;
        Debug.Log("Siam ah");
    }

    void Test2()
    {
        Vector3 headingVector = transform.position - target.position;
        //Vector3 headingVector = target.position - transform.position;
        float len = headingVector.magnitude;

        if(len < detection_range)
        {
            Vector3 adj = headingVector.normalized;
            float diff = (detection_range - len) * dodgeWeight;
            adj *= diff;
            moveDir -= adj;
            moveDir.Normalize();
        }
    }

    bool IsInCone()
    {
        Vector3 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;


        Debug.Log("Angle: " + angle + ", Left: " + (transform.localEulerAngles.x - fov_angle / 2));
        if(angle > transform.rotation.x - fov_angle/2)
        {
            if (angle < transform.localEulerAngles.x + fov_angle / 2)
                return true;
        }

        return false;
    }


    private void Update()
    {
        moveDir = originalDir;
        if (target)
        {
            if (Vector3.Distance(target.position, transform.position) <= detection_range)
            {
                if(IsInCone())
                {
                    Debug.Log("In view");
                }
                //Test1();
                //Test2();
            }
        }
        moveDir.z = 0;  
        transform.rotation = Quaternion.LookRotation(moveDir);
        transform.position += moveDir * speed * Time.deltaTime;
    }
}
