using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle_Wandering_Randomness : MonoBehaviour
{
    // Twerkable
    [Header("Movement Settings")]
    public float _maxForce = 0.2f;
    public float _maxSpeed = 4;
    public float _r = 5;

    public int units_of_distance = 4;

    [HideInInspector]
    public Vector3 _vel;
    [HideInInspector]
    public Vector3 _accel;
    [HideInInspector]
    public Vector3 _steer;
    
    // Private
    Camera _cam;
    Rigidbody2D rb;

    Bounds _worldSize;

    Vector3 _target;

    private void Start()
    {
        _cam = Camera.main;
        _vel.y = 1;
        _vel.x = 1;
        rb = GetComponent<Rigidbody2D>();
        _worldSize = OrthographicBounds(_cam);
    }

    Vector3 GetFuturePosition()
    {
        return transform.position + _vel * Time.deltaTime;
    }

    Vector3 SelectRandomPointInTheCircle(Vector3 futurePos)
    {
        Vector3 randomDir = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);

        futurePos += randomDir * _r * units_of_distance;

        return futurePos;
    }

    void GetTarget()
    {
        _target = SelectRandomPointInTheCircle(
                GetFuturePosition()
            );

        // Clamp
        // Bounds: Warp if out of camera
        if (_target.x < _worldSize.min.x)
        {
            _target.x = _worldSize.min.x;
        }
        else if (_target.x > _worldSize.max.x)
        {
            _target.x = _worldSize.max.x;
        }
        if (_target.y < _worldSize.min.y)
        {
            _target.y = _worldSize.min.y;
        }
        else if (_target.y > _worldSize.max.y)
        {
            _target.y = _worldSize.max.y;
        }
    }

    public void Seek_Random()
    {
        //-- Reached target yet?
        if(Vector3.Distance(_target, transform.position) < 0.5f)
        {
            GetTarget();
        }


        //-- Desired 
        Vector3 desired = _target - transform.position;
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

    Bounds OrthographicBounds(Camera camera)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        Bounds bounds = new Bounds(
            camera.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }

    private void Update()
    {
        Seek_Random();

        // Update velocity
        _vel += _accel;

        // Limit Speed
        _vel = Vector3.ClampMagnitude(_vel, _maxSpeed);


        // Rotate
        if(_vel != Vector3.zero)
        {
            float angle = Angle(new Vector2(_vel.x, _vel.y).normalized);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, angle);
        }
        // Apply position
        transform.position += _vel * Time.deltaTime;

        // Reset accelerationelertion to 0 each cycle
        _accel = Vector3.zero;

        // Bounds: Warp if out of camera
        if(transform.position.x < _worldSize.min.x)
        {
            transform.position = new Vector3(
                    _worldSize.max.x,
                    transform.position.y,
                    transform.position.z
                );
        }
        else if(transform.position.x > _worldSize.max.x)
        {
            transform.position = new Vector3(
                    _worldSize.min.x,
                    transform.position.y,
                    transform.position.z
                );
        }
        if (transform.position.y < _worldSize.min.y)
        {
            transform.position = new Vector3(
                    transform.position.x,
                    _worldSize.max.y,
                    transform.position.z
                );
        }
        else if (transform.position.y > _worldSize.max.y)
        {
            transform.position = new Vector3(
                    transform.position.x,
                    _worldSize.min.y,
                    transform.position.z
                );
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(_target, 3);
    }
}
