using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
  public GameObject m_sphere;
  public float m_speed = 5;
  public float m_duration = 5.0f;
  public float m_minTweenTimeForLastNode = 0.5f;
  public bool m_useConstantSpeed = true;

  Vector3 m_originalPosition;
  Vector3 m_targetPosition;
  PathManager m_pathManager;
  Rotation m_sphereRotation;
  Collider m_coll;
  float m_elapsedTime;
  float m_tweenDuration;
  bool m_isDragging = false;
  bool m_isTouchDown = false;
  bool m_isDirty = false;

  #region Input
  bool IsTouch()
  {
    return Input.GetMouseButton(0);
  }

  Vector3 GetTouchPosition()
  {
    return Input.mousePosition;
  }

  bool IsTouchMoving()
  {
    return Input.GetAxis("Mouse X") != 0.0f || Input.GetAxis("Mouse Y") != 0.0f;
  }
  #endregion

  void Start()
  {
    m_targetPosition = transform.position;
    m_pathManager = GetComponent<PathManager>();
    m_sphereRotation = m_sphere.GetComponent<Rotation>();
    m_coll = GetComponent<Collider>();
    Reset();
  }

  void Update()
  {
    if (IsTouch())
    {
      OnTouch();
    }
    else if (m_isTouchDown)
    {
      OnTouchReleased();
    }

    if (!m_isDragging)
    {
      MoveObject();
    }
  }

  void Reset()
  {
    m_pathManager.Reset();
    m_targetPosition = transform.position;
    m_elapsedTime = 0.0f;
    m_tweenDuration = m_duration;
  }

  void OnTouch()
  {
    if (!m_isTouchDown)
    {
      m_isTouchDown = true;
      Reset();
    }

    Ray ray = Camera.main.ScreenPointToRay(GetTouchPosition());
    RaycastHit hit;

    if (Physics.Raycast(ray, out hit))
    {
      OnClickedOnBackground(hit.collider);
    }
    else if (!m_isDragging)
    {
      OnClickedOutside();
    }
  }

  void OnTouchReleased()
  {
    m_isTouchDown = false;
    m_isDragging = false;
  }

  void OnClickedOnBackground(Collider collider)
  {
    if (m_isDragging)
    {
      if (IsTouchMoving())
      {
        m_pathManager.AddDragPoint();
      }
    }
    else if (collider.gameObject == m_sphere)
    {
      m_isDragging = true;
    }
    else
    {
      m_isDirty = true;
      Reset();
      m_pathManager.AddDragPoint();
    }
  }

  void OnClickedOutside()
  {
    m_isDirty = true;
    Reset();
    Vector3 mPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
    Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mPosition);
    Vector3 closestPoint = m_coll.ClosestPointOnBounds(worldPoint);
    m_pathManager.AddDragPoint(closestPoint);
  }

  void MoveObject()
  {
    if (!m_isDirty && transform.position != m_targetPosition)
    {
      MoveToTargetPosition();
    }
    else if (m_pathManager.HasNextPoint())
    {
      SetTargetPositionFromPathNode();
    }
    else
    {
      m_pathManager.OnFinishedMoving();
    }
  }

  void MoveToTargetPosition()
  {
    Vector3 newPos = Vector3.zero;
    if (m_useConstantSpeed)
    {
      newPos = CalculatePositionOnConstantSpeed();
    }
    else
    {
      newPos = CalculatePositionOnTween();
    }
    m_sphereRotation.Move(newPos - transform.position);
    transform.position = newPos;
    m_pathManager.SetOffset(transform.position);
  }

  void SetTargetPositionFromPathNode()
  {
    m_isDirty = false;
    m_originalPosition = transform.position;
    m_targetPosition = m_pathManager.GetNextPoint();

    // this is a hack to make sure we decelerate while dragging
    if (!m_pathManager.HasNextPoint() && m_pathManager.GetNumberOfPoints() > 1)
    {
      m_elapsedTime = 0.0f;
      m_tweenDuration = m_minTweenTimeForLastNode;
    }
  }

  Vector3 CalculatePositionOnConstantSpeed()
  {
    float step = m_speed * Time.fixedDeltaTime;
    return Vector3.MoveTowards(transform.position, m_targetPosition, step);
  }

  Vector3 CalculatePositionOnTween()
  {
    m_elapsedTime += Time.deltaTime;
    float alpha = GetTweenFactorCubic();
    if (alpha < 1.0f && !m_pathManager.HasNextPoint())
    {
      return (m_originalPosition * (1.0f - alpha) + (m_targetPosition * alpha));
    }
    else
    {
      float step = m_speed * Time.deltaTime;
      return Vector3.MoveTowards(transform.position, m_targetPosition, step);
    }
  }

  float GetTweenFactorCubic()
  {
    m_elapsedTime = Mathf.Clamp(m_elapsedTime, 0.0f, m_tweenDuration);
    float alpha = 1.0f - m_elapsedTime / m_tweenDuration;
    alpha = alpha * alpha * alpha;
    alpha = 1.0f - alpha;
    return alpha;
  }
}