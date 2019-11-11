using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public Vector3 moveDir;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(moveDir);
        transform.position += moveDir * speed * Time.deltaTime;
    }
}
