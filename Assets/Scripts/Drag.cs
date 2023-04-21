using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//None is after a collision or at the beginning
//launching starts whens its first click and ends after swipeMs milliseconds
//launched is after launch, even if still dragging launch has alresdy happened too bad so sad
public enum SwipeState
{
  None,
  Launching,
  Launched,
}

public class Drag : MonoBehaviour
{
  Vector3 mousePosOffset;
  [SerializeField] Camera mainCamera;
  Rigidbody2D rb;
  [SerializeField] float speed; //TODO: use this for the speed when its been launched
  [SerializeField] float trackingSpeed;
  SwipeState swipeState;
  float swipeTimer = 0f;
  public float swipeMS = 100f; //how long you have to swipe for (to prevent crashing lol)
  public float effectsCutoff = 0.1f; //how fast you have to be moving to run effects

  void Start()
  {
    swipeState = SwipeState.None;
    swipeTimer = 0f;

    rb = GetComponent<Rigidbody2D>();

    EventManager.OnEventEmitted += HandleEvent;
  }

  void Update()
  {
    if (swipeState == SwipeState.None) return;
    //TODO one day maybe launchING effects?
    if (swipeState == SwipeState.Launched) HandleLaunchEffects();
  }

  void HandleLaunchEffects()
  {

    //if velocity magnitude is below launching threshold dont run effects
    if (rb.velocity.magnitude < 0.1f) return;

    //todo: do other effects that might happened while launching (before hitting an obstacle that turns it off)

    OrientInDirectionOfMovement();
  }

  private void OrientInDirectionOfMovement()
  {
    if (rb.velocity.magnitude < effectsCutoff) return;

    // Calculate the angle between the current up direction and the velocity direction
    float angle = Vector2.SignedAngle(rb.velocity, transform.up);
    // Rotate the Rigidbody2D around the z-axis to face the velocity direction
    rb.MoveRotation(rb.rotation - angle);
  }

  //you only stop being "launched" when you collide with an event from BodyPartCollisionBehavior currently
  void HandleEvent(string eventKey, object data)
  {
    Dictionary<string, object> dataDict = data as Dictionary<string, object>;

    if (eventKey == "bodyPartCollision")
    {
      Debug.Log("COLLIDED RESETTING");
      resetLaunch();
      //TODO: have some types of collisions that dont stop launch process
    }
  }

  //MOUSE DOWN STUFF -- theres probably a better way lol

  //called first frame that youre clicking on it
  public void OnMouseDown()
  {
    mousePosOffset = gameObject.transform.position - getMouseWorldPosition(); // capute mouse offset

    //make it snap to mouse position
    rb.position = getMouseWorldPosition();
    rb.velocity = Vector2.zero;

    // Debug.Log("Starting launch!");
    swipeState = SwipeState.Launching;
  }

  //stops registering after swipMS milliseconds
  public void OnMouseDrag()
  {

    swipeTimer += Time.deltaTime * 1000f;
    // Debug.Log("swipeTimer: " + swipeTimer + " swipeMS: " + swipeMS);

    if (swipeTimer >= swipeMS) //if been launching for too long, launch it
    {
      if (swipeState == SwipeState.Launching) Launch();
      return;
    }
    else
    {
      TrackSwipe();

    }
  }

  public void OnMouseUp()
  {
    if (swipeState != SwipeState.Launched) Launch();
  }

  void Launch()
  {
    Debug.Log("LAUNCH");
    //TODO play launch animation

    swipeTimer = 0f;
    swipeState = SwipeState.Launched;

    EventManager.EmitEvent("launched", null);

    //add a one time acceleration to rigidbody
    rb.AddForce(rb.velocity.normalized * speed, ForceMode2D.Impulse);

  }

  void TrackSwipe()
  {
    Vector3 direction = (getMouseWorldPosition() + mousePosOffset - transform.position);

    // Debug.Log("tracking velocity (mouse drag) " + direction + " " + direction.x + " " + direction.y + " speed " + speed);
    rb.velocity = new Vector2(direction.x, direction.y) * trackingSpeed;
    // Debug.Log("velocity mag: " + rb.velocity.magnitude);
  }

  private Vector3 getMouseWorldPosition()
  {
    //capture mouse position and return world point
    return mainCamera.ScreenToWorldPoint(Input.mousePosition);
  }

  void resetLaunch()
  {
    swipeState = SwipeState.None;
    swipeTimer = 0f;
  }

}
