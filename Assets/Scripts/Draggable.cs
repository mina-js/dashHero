using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
  Vector3 mousePosOffset;
  Camera mainCamera;
  Rigidbody2D rb;
  float speed;
  RagdollController ragdollController;

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();

    //get the ragdoll controller on parent object, useful for passing props to all the children
    ragdollController = GetComponentInParent<RagdollController>();

    mainCamera = ragdollController.mainCamera;
    speed = ragdollController.speed;
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
    speed = ragdollController.speed;
  }

  void OnMouseDrag()
  {
    Vector3 direction = (getMouseWorldPosition() + mousePosOffset - transform.position).normalized;
    rb.velocity = new Vector2(direction.x, direction.y) * speed;
  }
}
