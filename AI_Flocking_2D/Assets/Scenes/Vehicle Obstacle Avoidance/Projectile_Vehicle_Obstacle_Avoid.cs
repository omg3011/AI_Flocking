using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Vehicle_Obstacle_Avoid : MonoBehaviour
{
  public float _moveSpeed = 5.0f;
  Vector3 _targetPos;
  Vector3 _moveDir;

  private void Start()
  {
    _targetPos = GameObject.FindWithTag("Player").GetComponent<Transform>().position;
    _moveDir = (_targetPos - transform.position).normalized;
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
    // Apply Rotation
    if (_moveDir != Vector3.zero)
    {
      float angle = Angle(new Vector2(_moveDir.x, _moveDir.y).normalized);
      transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, angle);
    }

    // Apply movement
    transform.position += _moveDir * _moveSpeed * Time.deltaTime;
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawLine(transform.position, transform.position + _moveDir * _moveSpeed);
  }
}
