using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle_Projectile_Avoidance_W_Dir : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform T_target;

    [Header("Avoid Settings")]
    [Range(10, 60)]
    public float Detect_Range;
    [Range(0, 360)]
    public float viewAngle;
    [Range(90, 600)]
    public float rotateWeight;

    // Twerkable
    [Header("Movement Settings")]
    public float _maxForce = 0.2f;
    public float _maxSpeed = 4;
    public float _r = 5;

    [HideInInspector]
    public Vector3 _vel;
    [HideInInspector]
    public Vector3 _accel;
    [HideInInspector]
    public Vector3 _steer;

    // Private
    Camera _cam;
    Rigidbody2D rb;

    Vector3 slamPos = Vector3.up * 100;
    Vector3 slamPos2 = Vector3.up * 100;
    Vector3 slamPos3 = Vector3.up * 100;
    Vector3 slamPos4 = Vector3.up * 100;

    //List<Transform> _listOfProjectile = new List<Transform>();

    public LayerMask targetMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    ////////////////////////////////////////////////////////////////////////////////
    // Unity Function
    ////////////////////////////////////////////////////////////////////////////////
    private void Start()
    {
        _cam = Camera.main;
        _vel.y = 1;
        _vel.x = 1;
        rb = GetComponent<Rigidbody2D>();

        //GameObject[] go = GameObject.FindGameObjectsWithTag("Projectile");
        //foreach(GameObject a in go)
        //{
        //    _listOfProjectile.Add(a.GetComponent<Transform>());
        //}
    }

    private void Update()
    {
        if (T_target)
            Seek(T_target.position);

        Projectile_Avoidance();

        // Update velocity
        _vel += _accel;

        // Limit Speed
        //_vel = Vector3.ClampMagnitude(_vel, _maxSpeed);


        // Rotate
        if (_vel != Vector3.zero)
        {
            float angle = Angle(new Vector2(_vel.x, _vel.y).normalized);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, angle);
        }
        // Apply position
        transform.position += _vel * Time.deltaTime;

        // Reset accelerationelertion to 0 each cycle
        _accel = Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Detect_Range);

        Gizmos.color = Color.cyan;
        Vector3 viewAngleA = DirFromAngle(-viewAngle / 2, false);
        Vector3 viewAngleB = DirFromAngle(viewAngle / 2, false);

        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * Detect_Range);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * Detect_Range);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + _vel);

        Gizmos.DrawSphere(slamPos, 1);
        Gizmos.DrawSphere(slamPos2, 1);
        Gizmos.DrawSphere(slamPos3, 1);
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(slamPos4, 1);

    }


    ////////////////////////////////////////////////////////////////////////////////
    // Steering Function
    ////////////////////////////////////////////////////////////////////////////////
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

    void Arrive(Vector3 target)
    {
        //-- Desired 
        Vector3 desired = target - transform.position;
        float d = desired.magnitude;
        desired.Normalize();

        //-- Within range
        if (d < _r)
        {
            float newSpeed = d - _r;
            if (newSpeed < 0)
                newSpeed = 0;
            desired *= newSpeed;
        }
        else
            desired *= _maxSpeed;

        //-- Steering
        _steer = desired - _vel;
        _steer = Vector3.ClampMagnitude(_steer, _maxForce);


        applyForce(_steer);
    }

    void Projectile_Avoidance()
    {
        FindVisibleTargets();
    }

    ////////////////////////////////////////////////////////////////////////////////
    // Helper Function
    ////////////////////////////////////////////////////////////////////////////////
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

    int AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0f)
        {
            return -1;
        }
        else if (dir < 0f)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, Detect_Range, targetMask);
        Vector3 closestTarget = Vector3.zero;
        float closestDist = 9999;
        bool foundTarget = false;

        for (int i = 0; i < targetsInViewRadius.Length; ++i)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.right, dirToTarget) < viewAngle / 2)
            {
                float distToTarget = Vector3.Distance(transform.position, target.position);
                if(distToTarget < closestDist)
                {
                    if (Physics2D.Raycast(transform.position, dirToTarget, closestDist, targetMask))
                    {
                        closestDist = distToTarget;
                        closestTarget = target.position;
                        foundTarget = true;
                    }
                }
            }
        }

        if(foundTarget)
        {
            Vector3 endPos = Vector3.zero;

            float enemyDist = closestDist;
            Vector3 enemyPos = closestTarget;
            
            Vector3 myVel = _vel.normalized;
            Vector3 myPos = transform.position;

            float gap = Vector3.Distance(enemyPos, myPos);
            float percent = gap / Detect_Range;
            Vector3 dDir = (myPos + myVel * _maxSpeed - enemyPos).normalized;

            endPos = myPos + myVel * _maxSpeed + dDir;

            slamPos = myPos + myVel * _maxSpeed;
            slamPos2 = endPos;

            //-- Desired 
            Vector3 desired = endPos - transform.position;
            desired.Normalize();
            desired *= _maxSpeed;

            //-- Steering
            _steer = desired - _vel;
            _steer = Vector3.ClampMagnitude(_steer, _maxForce);


            applyForce(_steer);

            return;
        }
    }

    void applyForce(Vector3 force)
    {
        _accel += force;
    }

    public Vector3 DirFromAngle(float angleInDeg, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
            angleInDeg += transform.eulerAngles.z;

        return new Vector3(
            Mathf.Cos(angleInDeg * Mathf.Deg2Rad),
            Mathf.Sin(angleInDeg * Mathf.Deg2Rad),
            0);
    }



  
}
