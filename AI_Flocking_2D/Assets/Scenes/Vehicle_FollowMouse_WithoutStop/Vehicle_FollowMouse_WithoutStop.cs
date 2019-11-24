using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle_FollowMouse_WithoutStop : MonoBehaviour
{
    // Twerkable
    [Header("Movement Settings")]
    public float _maxForce = 0.2f;
    public float _maxSpeed = 4;
    public float _r = 5;

    [HideInInspector]
    public Vector3 _pos;
    [HideInInspector]
    public Vector3 _vel;
    [HideInInspector]
    public Vector3 _accel;
    [HideInInspector]
    public Vector3 _steer;

    // Debug
    public LayerMask _groundMask;
    public Vector3 _mousePos;
    
    // Private
    Camera _cam;
    Rigidbody2D rb;


    private void Start()
    {
        _cam = Camera.main;
        _vel.y = 1;
        _vel.x = 1;
        rb = GetComponent<Rigidbody2D>();
    }


    public void Seek(Vector3 target)
    {
        //-- Desired 
        Vector3 desired = target - transform.position;
        desired.Normalize();
        desired *= _maxSpeed;

        //-- Steering
        _steer = desired - _vel;
        _steer = Vector3.ClampMagnitude(_steer, _maxForce);


        applyForce(_steer);
    }

    void applyForce(Vector3 force)
    {
        _accel += force;
    }

    void UpdateMousePos()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward * 1000);

        if (hit.collider != null)
        {
            _mousePos = hit.point; 

        }

    }

    public static float Angle(Vector2 p_vector2)
    {
        if (p_vector2.x < 0)
        {
            return 360 - (Mathf.Atan2(p_vector2.y, p_vector2.x) * Mathf.Rad2Deg * -1);
        }
        else
        {
            return Mathf.Atan2(p_vector2.y, p_vector2.x) * Mathf.Rad2Deg;
        }
    }

    private void Update()
    {
        UpdateMousePos();
        Seek(_mousePos);

        // Update velocity
        _vel += _accel;

        // Limit Speed
        _vel = Vector3.ClampMagnitude(_vel, _maxSpeed);


        // Rotate
        float angle = Angle(new Vector2(_vel.x, _vel.y).normalized);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, angle);

        // Apply position
        transform.position += _vel * Time.deltaTime;

        // Reset accelerationelertion to 0 each cycle
        _accel = Vector3.zero;
    }
}
