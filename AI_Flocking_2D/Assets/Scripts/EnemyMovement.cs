using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
  [Header("Parenting")]
  public List<Transform> ListOfAgent;

  [Header("Basic Settings")]
  public float speed = 5.0f;
  public float DETECt_RANGE = 5;

  [Header("Weights")]
  public float alignmentWeight = 1;
  public float cohesionWeight = 1;
  public float separationWeight = 1;


  [Header("Move Dir")]
  public Vector2 moveDir = Vector2.left;

  [HideInInspector]
  public Vector2 velocity;

  // Alignment: Get the avereage velocity
  Vector2 computeAlignment(Transform myAgent)
  {
    Vector2 result = Vector3.zero;
    int neighbourCount = 0;

    foreach (Transform otherT in ListOfAgent)
    {
      if(otherT != myAgent)
      {
        if(Vector2.Distance(myAgent.position, otherT.position) < DETECt_RANGE)
        {
          EnemyMovement enemy = otherT.GetComponent<EnemyMovement>();
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
        if (Vector2.Distance(myAgent.position, otherT.position) < DETECt_RANGE)
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
        if (Vector2.Distance(myAgent.position, otherT.position) < DETECt_RANGE)
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

  private void Update()
  {
    velocity = Vector2.zero;
    if(ListOfAgent.Count == 0)
    {
      velocity += speed * moveDir;
    }
    else
    {
      Vector2 alignment = computeAlignment(transform) * alignmentWeight;
      Vector2 cohesion = computeCohesion(transform) * cohesionWeight;
      Vector2 separation = computeSeparation(transform) * separationWeight;

      velocity += Vector2.one * speed + alignment + cohesion + separation;
      Debug.Log("Alignment: " + alignment + ", Cohesion: " + cohesion + ", sep: " + separation + ", Vel: " + velocity);
    }

    transform.position += new Vector3(
      velocity.x * Time.deltaTime,
      velocity.x * Time.deltaTime,
      transform.position.z
      );
  }
}
