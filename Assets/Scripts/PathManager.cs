using UnityEngine;
using System.Collections;

public class PathManager : MonoBehaviour
{
  public int skipPointCount = 7;
  ArrayList dragPoints = new ArrayList();
  int CurrentDragPointIndex;
  int currentPointCount;
  PathRenderer pathRenderer;

  void Start()
  {
    pathRenderer = Camera.main.GetComponent<PathRenderer>();
  }

  public void AddDragPoint(bool force = false)
  {
    if (force || ((currentPointCount % skipPointCount) == 0))
    {
      Vector3 mPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
      Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mPosition);
      worldPoint = transform.position - worldPoint;
      dragPoints.Add(worldPoint);
      pathRenderer.AddPoint(worldPoint);
    }
    currentPointCount++;
  }

  public void Reset()
  {
    if(pathRenderer)
    {
      pathRenderer.Reset();
    }
    dragPoints.Clear();
    CurrentDragPointIndex = 0;
    currentPointCount = 0;
  }

  public bool HasNextPoint()
  {
    return CurrentDragPointIndex < dragPoints.Count;
  }

  public Vector3 GetNextPoint()
  {
    Vector3 point = (Vector3)dragPoints[CurrentDragPointIndex];
    CurrentDragPointIndex++;
    return point;
  }

  public void OnFinishedMoving()
  {
    pathRenderer.Reset();
  }

  public void SetOffset(Vector3 offset)
  {
    pathRenderer.Offset = offset;
  }
}