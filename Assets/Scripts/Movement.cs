using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{

  public GameObject cube;
  public GameObject sceneCamera;
  public float speed = 5;
  Vector3 targetPosition;

  // Use this for initialization
  void Start()
  {
    targetPosition = transform.position;
  }

  // Update is called once per frame
  void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hit;

      if (Physics.Raycast(ray, out hit))
      {
        targetPosition = hit.point;
        Debug.DrawLine(transform.position, targetPosition, Color.red, 30);
        targetPosition = transform.position - targetPosition;
        Debug.DrawLine(transform.position, targetPosition, Color.blue, 30);
      }
    }
    if (transform.position != targetPosition)
    {
      float step = speed * Time.deltaTime;
      transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
    }
//     if(cube.transform.position != targetPosition)
//     {
//       float step = speed * Time.deltaTime;
//       cube.transform.position = Vector3.MoveTowards(cube.transform.position, targetPosition, step);
//       sceneCamera.transform.position = new Vector3(cube.transform.position.x, cube.transform.position.y, sceneCamera.transform.position.z);
//     }
  }
}