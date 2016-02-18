using UnityEngine;
using System.Collections;

public class Rotation : MonoBehaviour
{
  public bool m_isRotateAllowed = true;
  public float m_RotationSpeed = 100.0f;

  public void Move(Vector3 deltaPosition)
  {
    if(m_isRotateAllowed)
    {
      Vector3 rot = Vector3.zero;
      rot.x = -deltaPosition.y * m_RotationSpeed;
      rot.y = -deltaPosition.x * m_RotationSpeed;
      transform.Rotate(rot);
    }
  }
}