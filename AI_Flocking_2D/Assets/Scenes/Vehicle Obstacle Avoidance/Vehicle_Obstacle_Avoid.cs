using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle_Obstacle_Avoid : MonoBehaviour
{
  //-- Public Twerkable
  [Header("Cap Movement Settings")]
  public float _maxForce = 0.2f;
  public float _maxSpeed = 4.0f;
  public float _maxSeeAhead = 30.0f;
  public float _MAX_AVOID_FORCE = 15.0f;

  [Header("Cap Dodging Settings")]
  public float _maxForce_dodging = 0.2f;
  public float _maxSpeed_dodging = 4.0f;


  [Header("Target Settings")]
  public Transform targetT;

  [Header("Obstacle Settings")]
  public float _obstacle_r = 5.0f;
  public Transform obstacleT;


  [Header("Weights Settings")]
  public float seekWeight = 0.3f;
  public float avoidWeight = 0.7f;

  //-- Hidden
  [HideInInspector]
  public Vector3 _vel;
  [HideInInspector]
  public Vector3 _accel;
  [HideInInspector]
  public Vector3 _steer;

  //-- Private
  Vector3 _ahead;
  Vector3 _aheadHalf;  // Check Half
  float _dynamic_length;
  Vector3 _avoidance_force;
  Transform myTransform;

  float _currMaxSpeed;
  float _currMaxForce;

  private void Start()
  {
    myTransform = GetComponent<Transform>();

    //-- Starting vel
    _vel.x = -1;
    _vel.y = -2;

    _currMaxSpeed = _maxSpeed;
  }

  private void Update()
  {
    _steer = GetSeek(targetT.position) * seekWeight + GetCollisionAvoidanceForce() * avoidWeight;

    _steer = Vector3.ClampMagnitude(_steer, _currMaxForce);

    _vel += _steer;
    _vel = Vector3.ClampMagnitude(_vel, _currMaxSpeed);


    // Rotate
    if (_vel != Vector3.zero)
    {
      float angle = Angle(new Vector2(_vel.x, _vel.y).normalized);
      myTransform.localEulerAngles = new Vector3(myTransform.localEulerAngles.x, myTransform.localEulerAngles.y, angle);
    }

    myTransform.position += _vel * Time.deltaTime;

  }

  private void OnDrawGizmos()
  {

    Gizmos.color = Color.red;
    Gizmos.DrawLine(transform.position, _ahead);

    Gizmos.color = Color.blue;
    Gizmos.DrawLine(transform.position, _aheadHalf);

    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(obstacleT.position, _obstacle_r);
  }

  //-- Helper
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

  void truncate(ref Vector3 vec, float max)
  {
    float i = max / vec.magnitude;

    i = (i < 1.0f) ? 1.0f : i;

    vec *= i;
  }
  
  bool Distance_LineIntersectCircle(Vector3 ahead, Vector3 aheadHalf, Transform target)
  {
    if(Vector3.Distance(target.position, ahead) <= _obstacle_r) 
      return true;

    if(Vector3.Distance(target.position, aheadHalf) <= _obstacle_r)
      return true;

    return false;
  }

  //-- Core
  Vector3 GetSeek(Vector3 target)
  {
    Vector3 desired = target - transform.position;
    desired.Normalize();
    desired *= _currMaxSpeed;
    Vector3 force = desired - _vel;

    return force;
  }

  Vector3 GetCollisionAvoidanceForce()
  {
    //-- See-ing ahead
    _ahead = transform.position + _vel.normalized * _dynamic_length;
    _aheadHalf = transform.position + _vel.normalized * _maxSeeAhead * 0.5f;

    //-- TODO
    //Get nearest target from list of target
    if(Distance_LineIntersectCircle(_ahead, _aheadHalf, obstacleT))
    {
      //-- Get avoid force
      _avoidance_force = _ahead - obstacleT.position;
      _avoidance_force = _avoidance_force.normalized * _MAX_AVOID_FORCE;
      _currMaxSpeed = _maxSpeed_dodging;
      _currMaxForce = _maxForce_dodging;
    }
    else
    {
      _avoidance_force = Vector3.zero;
      _currMaxSpeed = _maxSpeed;
      _currMaxForce = _maxForce;
    }

    return _avoidance_force;
  }
}
