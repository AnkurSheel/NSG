using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
  public GameObject sphere;
  public float speed = 5;
  bool isDragging = false;
  bool isMouseDown = false;
  Vector3 targetPosition;
  ArrayList dragPoints = new ArrayList();
  LineRenderer line;

  // Use this for initialization
  void Start()
  {
    targetPosition = transform.position;
    line = GetComponent<LineRenderer>();
    line.SetWidth(0.1f, 0.1f);
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetMouseButton(0))
    {
      if(!isMouseDown)
      {
        isMouseDown = true;
        ClearPath();
      }
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hit;

      if (Physics.Raycast(ray, out hit))
      {
        if (isDragging)
        {
          if (HasPointerMoved())
          {
            AddDragPoints();
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
            SetTargetPositionToMoveBackground(hit.point);
          }
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

  private void AddDragPoints()
  {
    Vector3 mPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 4.7f);
    dragPoints.Add(Camera.main.ScreenToWorldPoint(mPosition));
    line.SetVertexCount(dragPoints.Count);
    line.SetPosition(dragPoints.Count - 1, (Vector3)dragPoints[dragPoints.Count - 1]);
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
      line.SetPosition(i, (Vector3)dragPoints[i]);
    }
  }

  private void SetTargetPositionToMoveBackground(Vector3 touchPosition)
  {
    targetPosition = transform.position - touchPosition;
  }

  private void ClearPath()
  {
    dragPoints.Clear();
    line.SetVertexCount(0);
  }

  private void MoveObject()
  {
    if (transform.position != targetPosition)
    {
      float step = speed * Time.deltaTime;
      transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
    }
  }

}