using UnityEngine;
using System.Collections;

public class PathManager : MonoBehaviour
{
  public int m_skipPointCount = 1;
  ArrayList m_pathPoints = new ArrayList();
  int m_currentPathPointIndex;
  int m_numberOfPathNodesAdded;
  PathRenderer m_pathRenderer;

  void Start()
  {
    m_pathRenderer = Camera.main.GetComponent<PathRenderer>();
  }

  public void AddDragPoint(bool force = false)
  {
    if (force || ((m_numberOfPathNodesAdded % m_skipPointCount) == 0))
    {
      Vector3 mPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
      Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mPosition);
      AddDragPoint(worldPoint);
    }
    else
    {
      m_numberOfPathNodesAdded++;
    }
  }

  public void AddDragPoint(Vector3 worldPoint)
  {
    worldPoint = transform.position - worldPoint;
    m_pathPoints.Add(worldPoint);
    m_pathRenderer.AddPoint(worldPoint);
    m_numberOfPathNodesAdded++;
  }

  public void Reset()
  {
    if (m_pathRenderer)
    {
      m_pathRenderer.Reset();
    }
    m_pathPoints.Clear();
    m_currentPathPointIndex = 0;
    m_numberOfPathNodesAdded = 0;
  }

  public bool HasNextPoint()
  {
    return m_currentPathPointIndex < m_pathPoints.Count;
  }

  public Vector3 GetNextPoint()
  {
    Vector3 point = (Vector3)m_pathPoints[m_currentPathPointIndex];
    m_currentPathPointIndex++;
    return point;
  }

  public void OnFinishedMoving()
  {
    m_pathRenderer.Reset();
  }

  public void SetOffset(Vector3 offset)
  {
    m_pathRenderer.Offset = offset;
  }

  public int GetNumberOfPoints()
  {
    return m_pathPoints.Count;
  }
}