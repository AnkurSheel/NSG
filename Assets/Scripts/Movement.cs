using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
  public GameObject sphere;
  public float speed = 5;
  public float pathDepth = -0.2f;
  public int skipPointCount = 7;
  Vector3 targetPosition;
  ArrayList dragPoints = new ArrayList();
  int CurrentDragPointIndex;
  bool isDragging = false;
  bool isMouseDown = false;
  PathRenderer pathRenderer;
  int currentPointCount;

  void Start()
  {
    targetPosition = transform.position;
    pathRenderer = Camera.main.GetComponent<PathRenderer>();
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
            AddDragPoint();
            DrawLine();
          }
        }
        else if (hit.collider.gameObject == sphere)
        {
          isDragging = true;
        }
        else
        {
          dragPoints.Clear();
          AddDragPoint();
        }
      }
    }
    else if (isMouseDown)
    {
      if(isDragging)
      {
        AddDragPoint(true);
      }
      isMouseDown = false;
      isDragging = false;
    }
  }

  public void FixedUpdate()
  {
    MoveObject();
  }

  private void AddDragPoint(bool force = false)
  {
    if(force || ((currentPointCount % skipPointCount) == 0))
    {
      Vector3 mPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
      Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mPosition);
      worldPoint = transform.position - worldPoint;
      dragPoints.Add(worldPoint);
    }
    currentPointCount++;
  }

  private bool HasPointerMoved()
  {
    return Input.GetAxis("Mouse X") != 0.0f || Input.GetAxis("Mouse Y") != 0.0f;
  }

  private void DrawLine()
  {
    Vector3 worldPoint = (Vector3)dragPoints[dragPoints.Count - 1];
    pathRenderer.AddPoint(worldPoint);
  }

  private void Reset()
  {
    dragPoints.Clear();
    pathRenderer.Reset();
    CurrentDragPointIndex = 0;
    currentPointCount = 0;
  }

  private void MoveObject()
  {
    if (!isDragging)
    {
      if (transform.position != targetPosition)
      {
        float step = speed * Time.fixedDeltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        pathRenderer.BackgroundPosition = transform.position;
      }
      else if (CurrentDragPointIndex < dragPoints.Count)
      {
        targetPosition = (Vector3)dragPoints[CurrentDragPointIndex];
        CurrentDragPointIndex++;
      }
      else
      {
        pathRenderer.Reset();
      }
    }
  }
}