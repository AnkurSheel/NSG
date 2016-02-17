using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
  public GameObject sphere;
  public float speed = 5;
  public float duration = 5.0f;
  public bool useConstantSpeed = true;

  Vector3 originalPosition;
  Vector3 targetPosition;
  PathManager pathManager;
  Rotation sphereRotation;
  float elapsedTime;
  bool isDragging = false;
  bool isMouseDown = false;
  bool isDirty = false;

  void Start()
  {
    targetPosition = transform.position;
    pathManager = GetComponent<PathManager>();
    sphereRotation = sphere.GetComponent<Rotation>();
    Reset();
  }

  void Update()
  {
    if (Input.GetMouseButton(0))
    {
      if (!isMouseDown)
      {
        isMouseDown = true;
        Reset();
      }

      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hit;

      if (Physics.Raycast(ray, out hit))
      {
        if (isDragging)
        {
          if (HasPointerMoved())
          {
            pathManager.AddDragPoint();
          }
        }
        else if (hit.collider.gameObject == sphere)
        {
          isDragging = true;
        }
        else
        {
          isDirty = true;
          pathManager.Reset();
          pathManager.AddDragPoint();
        }
      }
    }
    else if (isMouseDown)
    {
      isMouseDown = false;
      isDragging = false;
    }
    MoveObject();
  }

  private bool HasPointerMoved()
  {
    return Input.GetAxis("Mouse X") != 0.0f || Input.GetAxis("Mouse Y") != 0.0f;
  }

  private void Reset()
  {
    pathManager.Reset();
    elapsedTime = 0.0f;
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
      }
      else
      {
        sphereRotation.IsMoving = false;
        pathManager.OnFinishedMoving();
      }
    }
  }

  private float GetTweenFactorCubic()
  {
    float alpha = 1.0f - elapsedTime / duration;
    alpha = alpha * alpha * alpha;
    alpha = 1.0f - alpha;
    alpha = Mathf.Clamp(alpha, 0.0f, 1.0f);
    return alpha;
  }
}