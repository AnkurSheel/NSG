using UnityEngine;
using System.Collections;

// Put this script on a Camera
public class PathRenderer : MonoBehaviour
{
  public float lineDepth = -0.2f;
  public Material lineMat;
  ArrayList points = new ArrayList();
  Vector3 backgroundPosition;

  public Vector3 BackgroundPosition
  {
    set { backgroundPosition = value; }
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
      foreach (Vector3 point in points)
      {
        Vector3 pointPos = point;

        GL.Begin(GL.LINES);
        lineMat.SetPass(0);
        GL.Color(lineMat.color);
        GL.Vertex3(backgroundPosition.x - prevPointPos.x, backgroundPosition.y - prevPointPos.y, lineDepth);
        GL.Vertex3(backgroundPosition.x - pointPos.x, backgroundPosition.y - pointPos.y, lineDepth);
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