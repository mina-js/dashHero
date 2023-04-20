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

  public bool isAnchor;
  bool isLaunched;

  void Start()
  {
    rb = GetComponent<Rigidbody2D>();

    //get the ragdoll controller on parent object, useful for passing props to all the children
    ragdollController = GetComponentInParent<RagdollController>();
    mainCamera = Camera.main;
    speed = ragdollController.speed;

    isLaunched = false;
    EventManager.OnEventEmitted += HandleEvent;
  }

  void Update()
  {
    //rotate the rb to face the velocity direction
    if (isLaunched && isAnchor)
    {
      OrientInDirectionOfMovement();
    }
  }

  //called first frame that youre clicking on it
  void OnMouseDown()
  {
    //capute mouse offset
    mousePosOffset = gameObject.transform.position - getMouseWorldPosition();
    speed = ragdollController.speed;
    isLaunched = false;
  }

  void OnMouseDrag()
  {
    Vector3 direction = (getMouseWorldPosition() + mousePosOffset - transform.position);
    rb.velocity = new Vector2(direction.x, direction.y) * speed;
  }

  void OnMouseUp()
  {
    isLaunched = true;
  }

  void HandleEvent(string eventKey, object data)
  {
    Dictionary<string, object> dataDict = data as Dictionary<string, object>;

    if (eventKey == "bodyPartCollision")
    {
      isLaunched = false; //stops it from trying to align itself once it collides, todo this wont work once enemies arent colliders hmmmm
    }
  }

  private void OrientInDirectionOfMovement()
  {
    // Calculate the angle between the current up direction and the velocity direction
    float angle = Vector2.SignedAngle(rb.velocity, transform.up);
    // Rotate the Rigidbody2D around the z-axis to face the velocity direction
    rb.MoveRotation(rb.rotation - angle);
  }

  private Vector3 getMouseWorldPosition()
  {
    //capture mouse position and return world point
    return mainCamera.ScreenToWorldPoint(Input.mousePosition);
  }
}
