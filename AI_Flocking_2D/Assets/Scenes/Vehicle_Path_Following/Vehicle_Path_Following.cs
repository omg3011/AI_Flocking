using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle_Path_Following : MonoBehaviour
{
    /*
    // Twerkable
    [Header("Movement Settings")]
    public float _maxForce = 0.2f;
    public float _maxSpeed = 4;e_
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

    public void Seek()
    {
        //-- Reached target yet?
        if (Vector3.Distance(_target, transform.position) < 0.5f)
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



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Helper
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    Bounds OrthographicBounds(Camera camera)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        Bounds bounds = new Bounds(
            camera.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
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


    void applyForce(Vector3 force)
    {
        _accel += force;
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

    Vector3 getNormalPoint(Vector3 p, Vector3 a, Vector3 b)
    {
        //PVector that points from a to p
        Vector3 ap = p - a;
        //PVector that points from a to b
        Vector3 ab = b - a;

        //Using the dot product for scalar projection        
        ab.Normalize();
        ab *= (ap.x * ab.x + ap.y * ab.y);

        //Finding the normal point along the line segment
        Vector3 normalPoint = a + ab;

        return normalPoint;
    }


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Steering Behaviour
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
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

    void follow(Vector3 p_start, Vector3 p_end, float p_radius)
    {
        //Step 1: Predict the vehicle’s future location.
        Vector3 predict = _vel;
        predict.Normalize();
        predict *= 25;
        Vector3 predictLoc = transform.position + predict; 

        //Step 2: Find the normal point along the path.
        Vector3 a = p_start;
        Vector3 b = p_end;
        Vector3 normalPoint = getNormalPoint(predictLoc, a, b);

        //Step 3: Move a little further along the path and set a target.
        Vector3 dir = b - a;
        dir.Normalize();
        dir *= 10;
        Vector3 target = normalPoint + dir;

        //Step 4: If we are off the path, seek that target in order to stay on the path.
        float distance = Vector3.Distance(normalPoint, predictLoc);
        if (distance > p_radius)
        {
            Seek(target);
        }
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
    */
}
