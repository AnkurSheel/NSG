using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
  public GameObject sphere;
  public float speed = 5;
  public float duration = 5.0f;
  public float minTweenTimeForLastNode = 0.5f;
  public bool useConstantSpeed = true;

  Vector3 originalPosition;
  Vector3 targetPosition;
  PathManager pathManager;
  Rotation sphereRotation;
  Collider coll;
  float elapsedTime;
  float tweenDuration;
  bool isDragging = false;
  bool isTouchDown = false;
  bool isDirty = false;

  private bool IsTouch()
  {
    return Input.GetMouseButton(0);
  }

  Vector3 GetTouchPosition()
  {
    return Input.mousePosition;
  }

  private bool IsTouchMoving()
  {
    return Input.GetAxis("Mouse X") != 0.0f || Input.GetAxis("Mouse Y") != 0.0f;
  }

  void Start()
  {
    targetPosition = transform.position;
    pathManager = GetComponent<PathManager>();
    sphereRotation = sphere.GetComponent<Rotation>();
    coll = GetComponent<Collider>();
    Reset();
  }

  void Update()
  {
    if (IsTouch())
    {
      OnTouch();
    }
    else if (isTouchDown)
    {
      OnTouchReleased();
    }
    MoveObject();
  }

  private void Reset()
  {
    pathManager.Reset();
    targetPosition = transform.position;
    elapsedTime = 0.0f;
    tweenDuration = duration;
  }

  private void OnTouch()
  {
    if (!isTouchDown)
    {
      isTouchDown = true;
      Reset();
    }

    Ray ray = Camera.main.ScreenPointToRay(GetTouchPosition());
    RaycastHit hit;

    if (Physics.Raycast(ray, out hit))
    {
      OnClickedOnBackground(hit.collider);
    }
    else if (!isDragging)
    {
      OnClickedOutside();
    }
  }

  private void OnTouchReleased()
  {
    isTouchDown = false;
    isDragging = false;
  }

  private void MoveObject()
  {
    if (!isDragging)
    {
      if (!isDirty && transform.position != targetPosition)
      {
        Vector3 newPos = Vector3.zero;
        if (useConstantSpeed)
        {
          float step = speed * Time.fixedDeltaTime;
          newPos = Vector3.MoveTowards(transform.position, targetPosition, step);
        }
        else
        {
          elapsedTime += Time.deltaTime;
          float alpha = GetTweenFactorCubic();
          if (alpha < 1.0f && !pathManager.HasNextPoint())
          {
            newPos = originalPosition * (1.0f - alpha) + (targetPosition * alpha);
          }
          else
          {
            float step = speed * Time.deltaTime;
            newPos = Vector3.MoveTowards(transform.position, targetPosition, step);
          }
        }
        sphereRotation.Move(newPos - transform.position);
        transform.position = newPos;
        pathManager.SetOffset(transform.position);
      }
      else if (pathManager.HasNextPoint())
      {
        isDirty = false;
        originalPosition = transform.position;
        targetPosition = pathManager.GetNextPoint();

        // this is a hack to make sure we decelerate while dragging
        if (!pathManager.HasNextPoint() && pathManager.GetNumberOfPoints() > 1)
        {
          elapsedTime = 0.0f;
          tweenDuration = minTweenTimeForLastNode;
        }
      }
      else
      {
        pathManager.OnFinishedMoving();
      }
    }
  }

  private float GetTweenFactorCubic()
  {
    elapsedTime = Mathf.Clamp(elapsedTime, 0.0f, tweenDuration);
    float alpha = 1.0f - elapsedTime / tweenDuration;
    alpha = alpha * alpha * alpha;
    alpha = 1.0f - alpha;
    return alpha;
  }

  private void OnClickedOnBackground(Collider collider)
  {
    if (isDragging)
    {
      if (IsTouchMoving())
      {
        pathManager.AddDragPoint();
      }
    }
    else if (collider.gameObject == sphere)
    {
      isDragging = true;
    }
    else
    {
      isDirty = true;
      Reset();
      pathManager.AddDragPoint();
    }
  }

  private void OnClickedOutside()
  {
    Vector3 mPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
    Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mPosition);
    Vector3 closestPoint = coll.ClosestPointOnBounds(worldPoint);
    pathManager.AddDragPoint(closestPoint);
  }
}