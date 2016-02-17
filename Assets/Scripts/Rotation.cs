using UnityEngine;
using System.Collections;

public class Rotation : MonoBehaviour
{
  public bool isRotateAllowed = false;
  public float speed = 100.0f;
  Vector3 moveDelta;

  bool isMoving;
  public bool IsMoving
  {
    set { isMoving = value; }
  }

  // Use this for initialization
  void Start()
  {
    isMoving = false;
  }

  // Update is called once per frame
  void Update()
  {

//    transform.Rotate(Vector3.up, speed * Time.deltaTime);
    if (isRotateAllowed && isMoving)
    {
      Vector3 axis = Vector3.zero;
      if (moveDelta.y > 0.0f)
      {
        axis.x = -1.0f;
      }
      else if (moveDelta.y < 0.0f)
      {
        axis.x = 1.0f;
      }
      if (moveDelta.x > 0.0f)
      {
        axis.y = -1.0f;
      }
      else if (moveDelta.x < 0.0f)
      {
        axis.y = 1.0f;
      }
      transform.Rotate(moveDelta, speed * Time.deltaTime);
    }

  }

  public void Move(Vector3 position)
  {
    isMoving = true;
    moveDelta = position;
  }
}