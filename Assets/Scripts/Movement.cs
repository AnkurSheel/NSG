using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
  public GameObject sphere;
  public float speed = 5;
  public float pathDepth = -0.2f;
  Vector3 targetPosition;
  bool isDragging = false;
  bool isMouseDown = false;
  PathManager pathManager;

  void Start()
  {
    targetPosition = transform.position;
    pathManager = GetComponent<PathManager>();
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
          pathManager.Reset();
          pathManager.AddDragPoint();
        }
      }
    }
    else if (isMouseDown)
    {
      if (isDragging)
      {
        pathManager.AddDragPoint(true);
      }
      isMouseDown = false;
      isDragging = false;
    }
  }

  public void FixedUpdate()
  {
    MoveObject();
  }

  private bool HasPointerMoved()
  {
    return Input.GetAxis("Mouse X") != 0.0f || Input.GetAxis("Mouse Y") != 0.0f;
  }

  private void Reset()
  {
    pathManager.Reset();
  }

  private void MoveObject()
  {
    if (!isDragging)
    {
      if (transform.position != targetPosition)
      {
        float step = speed * Time.fixedDeltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        pathManager.SetOffset(transform.position);
      }
      else if (pathManager.HasNextPoint())
      {
        targetPosition = pathManager.GetNextPoint();
      }
      else
      {
        pathManager.OnFinishedMoving();
      }
    }
  }
}