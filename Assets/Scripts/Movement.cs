using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
  public GameObject sphere;
  public float speed = 5;
  public float pathDepth = -0.2f;
  Vector3 targetPosition;
  Vector3 originalPosition;
  LineRenderer line;
  int CurrentDragPointIndex;
  ArrayList dragPoints = new ArrayList();
  bool isDragging = false;
  bool isMouseDown = false;

  // Use this for initialization
  void Start()
  {
    targetPosition = transform.position;
    CurrentDragPointIndex = 0;
    line = GetComponent<LineRenderer>();
    line.SetWidth(0.1f, 0.1f);
  }

  // Update is called once per frame
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
        else
        {
          if (hit.collider.gameObject == sphere)
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
    }
    else if (isMouseDown)
    {
      isMouseDown = false;
      isDragging = false;
    }
  }

  public void FixedUpdate()
  {
    MoveObject();
  }

  private void AddDragPoint()
  {
    Vector3 mPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
    Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mPosition);
    dragPoints.Add(worldPoint);
  }

  private bool HasPointerMoved()
  {
    return Input.GetAxis("Mouse X") != 0.0f || Input.GetAxis("Mouse Y") != 0.0f;
  }

  private void DrawLine()
  {
    line.SetVertexCount(dragPoints.Count);
    for (int i = 0; i < dragPoints.Count; i++)
    {
      Vector3 worldPoint = (Vector3)dragPoints[i];
      worldPoint.z = pathDepth;
      line.SetPosition(i, worldPoint);
    }
  }

  private void Reset()
  {
    dragPoints.Clear();
    line.SetVertexCount(0);
    CurrentDragPointIndex = 0;
    originalPosition = transform.position;
  }

  private void MoveObject()
  {
    if (!isDragging)
    {
      if (transform.position != targetPosition)
      {
        float step = speed * Time.fixedDeltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
      }
      else if (CurrentDragPointIndex < dragPoints.Count)
      {
        targetPosition = originalPosition - (Vector3)dragPoints[CurrentDragPointIndex];
        CurrentDragPointIndex++;
      }
    }
  }
}