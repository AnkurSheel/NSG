using UnityEngine;
using System.Collections;

public class Rotation : MonoBehaviour
{
  public bool isRotateAllowed = false;
  public float speed = 100.0f;

  public void Move(Vector3 deltaPosition)
  {
    if(isRotateAllowed)
    {
      Vector3 rot = Vector3.zero;
      rot.x = -deltaPosition.y * speed;
      rot.y = -deltaPosition.x * speed;
      transform.Rotate(rot);
    }
  }
}