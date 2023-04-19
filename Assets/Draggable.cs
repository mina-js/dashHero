using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
  Vector3 mousePosOffset;
  public Camera mainCamera;
  Rigidbody2D rb;

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();
  }

  private Vector3 getMouseWorldPosition()
  {
    //capture mouse position and return world point
    return mainCamera.ScreenToWorldPoint(Input.mousePosition);
  }

  //called first frame that youre clicking on it
  void OnMouseDown()
  {
    //capute mouse offset
    mousePosOffset = gameObject.transform.position - getMouseWorldPosition();
  }

  void OnMouseDrag()
  {
    //move the rigidbody2d towards the mouse
    rb.MovePosition(getMouseWorldPosition() + mousePosOffset);
  }
}
