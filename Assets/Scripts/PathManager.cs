using UnityEngine;
using System.Collections;

public class PathManager : MonoBehaviour
{
  public int skipPointCount = 7;
  ArrayList pathPoints = new ArrayList();
  int CurrentPathPointIndex;
  int numberOfPathNodesAdded;
  PathRenderer pathRenderer;

  void Start()
  {
    pathRenderer = Camera.main.GetComponent<PathRenderer>();
  }

  public void AddDragPoint(bool force = false)
  {
    if (force || ((numberOfPathNodesAdded % skipPointCount) == 0))
    {
      Vector3 mPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
      Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mPosition);
      worldPoint = transform.position - worldPoint;
      pathPoints.Add(worldPoint);
      pathRenderer.AddPoint(worldPoint);
    }
    numberOfPathNodesAdded++;
  }

  public void Reset()
  {
    if(pathRenderer)
    {
      pathRenderer.Reset();
    }
    pathPoints.Clear();
    CurrentPathPointIndex = 0;
    numberOfPathNodesAdded = 0;
  }

  public bool HasNextPoint()
  {
    return CurrentPathPointIndex < pathPoints.Count;
  }

  public Vector3 GetNextPoint()
  {
    Vector3 point = (Vector3)pathPoints[CurrentPathPointIndex];
    CurrentPathPointIndex++;
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

  public int GetNumberOfPoints()
  {
    return pathPoints.Count;
  }
}