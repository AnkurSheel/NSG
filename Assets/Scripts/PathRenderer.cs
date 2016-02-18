using UnityEngine;
using System.Collections;

// Put this script on a Camera
public class PathRenderer : MonoBehaviour
{
  public float m_lineDepth = -0.2f;
  public Material m_lineMat;
  ArrayList m_points = new ArrayList();

  Vector3 m_offset;
  public Vector3 Offset
  {
    set { m_offset = value; }
  }

  public void AddPoint(Vector3 point)
  {
    m_points.Add(point);
  }

  public void Reset()
  {
    m_points.Clear();
  }

  void DrawConnectingLines()
  {
    Vector3 prevPointPos = Vector3.zero;
    if (m_points.Count > 0)
    {
      prevPointPos = (Vector3)m_points[0];
    }
    if (m_points.Count > 1)
    {
      for (int i = 1; i < m_points.Count; i++)
      {
        Vector3 pointPos = (Vector3)m_points[i];

        GL.Begin(GL.LINES);
        m_lineMat.SetPass(0);
        GL.Color(m_lineMat.color);
        GL.Vertex3(m_offset.x - prevPointPos.x, m_offset.y - prevPointPos.y, m_lineDepth);
        GL.Vertex3(m_offset.x - pointPos.x, m_offset.y - pointPos.y, m_lineDepth);
        GL.End();
        prevPointPos = pointPos;
      }
    }
  }

  void OnPostRender()
  {
    DrawConnectingLines();
  }
}