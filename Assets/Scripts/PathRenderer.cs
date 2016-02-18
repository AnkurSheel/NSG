using UnityEngine;
using System.Collections;

// Put this script on a Camera
public class PathRenderer : MonoBehaviour
{
  public float lineDepth = -0.2f;
  public Material lineMat;
  ArrayList points = new ArrayList();
  Vector3 offset;

  public Vector3 Offset
  {
    set { offset = value; }
  }

  public void AddPoint(Vector3 point)
  {
    points.Add(point);
  }

  public void Reset()
  {
    points.Clear();
  }

  void DrawConnectingLines()
  {
    Vector3 prevPointPos = Vector3.zero;
    if (points.Count > 0)
    {
      prevPointPos = (Vector3)points[0];
    }
    if (points.Count > 1)
    {
      for (int i = 1; i < points.Count; i++)
      {
        Vector3 pointPos = (Vector3)points[i];

        GL.Begin(GL.LINES);
        lineMat.SetPass(0);
        GL.Color(lineMat.color);
        GL.Vertex3(offset.x - prevPointPos.x, offset.y - prevPointPos.y, lineDepth);
        GL.Vertex3(offset.x - pointPos.x, offset.y - pointPos.y, lineDepth);
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