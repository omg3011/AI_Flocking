using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour
{
  [Header("Movement settings")]
  public Vector2 moveDir;
  public float speed = 5.0f;

  [Header("Flock settings")]
  public float DETECT_RANGE = 5;
  public float alignmentWeight = 1;
  public float cohesionWeight = 1;
  public float separationWeight = 1;
  public bool is_grouped = false;

  // Private
  public List<Transform> ListOfAllEnemies;
  private List<Transform> ListOfAgent = new List<Transform>();
  private Vector2 velocity;


  void ApplyMovementAndRotation()
  {
    transform.rotation = Quaternion.LookRotation(velocity.normalized);
    transform.position += new Vector3(
      velocity.x * Time.deltaTime,
      velocity.y * Time.deltaTime,
      transform.position.z
      );
  }
  
  void Update_JoinFlock()
  {
    foreach(Transform otherT in ListOfAllEnemies)
    {
      if(otherT != transform)
      {
        if(Vector2.Distance(otherT.position, transform.position) < DETECT_RANGE)
        {
          if(!ListOfAgent.Contains(transform))
          {
            is_grouped = true;
            ListOfAgent.Add(transform);
          }
        }
      }
    }
  }

  // Alignment: Get the avereage velocity
  Vector2 computeAlignment(Transform myAgent)
  {
    Vector2 result = Vector3.zero;
    int neighbourCount = 0;

    foreach (Transform otherT in ListOfAgent)
    {
      if (otherT != myAgent)
      {
        if (Vector2.Distance(myAgent.position, otherT.position) < DETECT_RANGE)
        {
          TestMovement enemy = otherT.GetComponent<TestMovement>();
          result += enemy.velocity;
          neighbourCount++;
        }
      }
    }

    if (neighbourCount == 0)
      return result;

    result /= neighbourCount;
    result.Normalize();

    return result;
  }


  // Cohesion: Get the direction towards center of mass
  Vector2 computeCohesion(Transform myAgent)
  {
    Vector2 result = Vector3.zero;
    int neighbourCount = 0;

    foreach (Transform otherT in ListOfAgent)
    {
      if (otherT != myAgent)
      {
        if (Vector2.Distance(myAgent.position, otherT.position) < DETECT_RANGE)
        {
          result.x += otherT.position.x;
          result.y += otherT.position.y;
          neighbourCount++;
        }
      }
    }

    if (neighbourCount == 0)
      return result;

    result /= neighbourCount;
    result = new Vector2(result.x - myAgent.position.x, result.y - myAgent.position.y);
    result.Normalize();

    return result;
  }


  // Separation: Steer away from neighbour
  Vector2 computeSeparation(Transform myAgent)
  {
    Vector2 result = Vector3.zero;
    int neighbourCount = 0;

    foreach (Transform otherT in ListOfAgent)
    {
      if (otherT != myAgent)
      {
        if (Vector2.Distance(myAgent.position, otherT.position) < DETECT_RANGE)
        {
          result.x += otherT.position.x - myAgent.position.x;
          result.y += otherT.position.y - myAgent.position.y;
          neighbourCount++;
        }
      }
    }

    if (neighbourCount == 0)
      return result;

    result /= neighbourCount;
    result *= -1;         // Move reverse
    result.Normalize();

    return result;
  }


  void Apply_Flocking()
  {
    Vector2 alignment = computeAlignment(transform) * alignmentWeight;
    Vector2 cohesion = computeCohesion(transform) * cohesionWeight;
    Vector2 separation = computeSeparation(transform) * separationWeight;

    velocity += alignment + cohesion + separation;
    Debug.Log("Alignment: " + alignment + ", Cohesion: " + cohesion + ", sep: " + separation + ", Vel: " + velocity);
  }

  private void Update()
  {
    // Join flock
    Update_JoinFlock();

    // Apply flocking if need
    if (ListOfAgent.Count > 0)
    {
      Apply_Flocking();
    }
    else
    {
      // Default velocity movement
      velocity = moveDir * speed;
    }

    // Apply movement to transform
    ApplyMovementAndRotation();
  }
}
