using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
  [SerializeField] Camera mainCamera;
  Rigidbody2D rb;
  [SerializeField] float speed; //TODO: use this for the speed when its been launched
  SwipeState swipeState;
  public float effectsCutoff = 0.1f; //how fast you have to be moving to run effects
  [SerializeField] private InputAction press, screenPos;
  Vector3 currentScreenPos;
  Vector3 startScreenPos;
  Vector2 launchVector;
  [SerializeField] GameObject movementArrow;
  [SerializeField] bool inverseControls = true;
  bool isDragging;
  bool isPressedOn
  {
    get
    {
      Collider2D hit = Physics2D.OverlapPoint(WorldPos);

      if (hit == null) return false;

      return hit.transform == transform;
    }
  }

  Vector3 WorldPos
  {
    get
    {
      float z = mainCamera.WorldToScreenPoint(transform.position).z;
      return mainCamera.ScreenToWorldPoint(currentScreenPos + new Vector3(0, 0, z));
    }
  }


  void Awake()
  {
    swipeState = SwipeState.None;

    // swipeTimer = 0f;

    rb = GetComponent<Rigidbody2D>();

    EventManager.OnEventEmitted += HandleEvent;

    screenPos.Enable();
    press.Enable();

    screenPos.performed += context => currentScreenPos = context.ReadValue<Vector2>();

    press.performed += _ => { StartCoroutine(OnTouch()); };
    press.canceled += _ => { isDragging = false; };
  }


  IEnumerator OnTouch()
  {

    OnGrab();
    while (isDragging)
    {
      OnDrag();
      yield return null;
    }
    OnDrop();
  }

  void OnGrab()
  {
    isDragging = true;

    startScreenPos = WorldPos;

    //freeze the rigidbody
    rb.constraints = RigidbodyConstraints2D.FreezeAll;
    rb.velocity = Vector2.zero;

    swipeState = SwipeState.Launching;

    movementArrow.transform.position = transform.position;
  }

  void OnDrag()
  {

    //TODO: theres an issue here if you click to the left of it?

    //Point the arrow in direction of launchvector with scaling
    launchVector = Mathf.Sign(inverseControls ? -1f : 1f) * (WorldPos - startScreenPos);
    movementArrow.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, launchVector));
    movementArrow.transform.localScale = new Vector3(1, launchVector.magnitude * 0.1f, 1);
    //movementArrow.transform.position = transform.position;
  }

  void OnDrop()
  {
    // Debug.Log("DROP");
    if (swipeState != SwipeState.Launched) Launch();
  }

  void Update()
  {
    if (swipeState == SwipeState.None) return;
    //TODO one day maybe launchING effects?
    if (swipeState == SwipeState.Launched) HandleLaunchEffects();
  }

  void HandleLaunchEffects()
  {
    DetectEnemyStrike();
    //if velocity magnitude is below launching threshold dont run effects
    if (rb.velocity.magnitude < 0.1f) return;

    //todo: do other effects that might happened while launching (before hitting an obstacle that turns it off)

    OrientInDirectionOfMovement();
  }

  void DetectEnemyStrike()
  {
    RaycastHit2D hit = Physics2D.Raycast(transform.position, rb.velocity, Mathf.Infinity, LayerMask.GetMask("enemyLayer"));


    if (hit.collider == null) return;

    Debug.Log("hit data! " + hit.collider?.gameObject?.name + " at dist " + hit.distance);

    float timeToHit = hit.distance / rb.velocity.magnitude;
    Debug.Log("time to hit " + timeToHit);
    if (timeToHit < 0.06)
    {
      EventManager.EmitEvent("enemyHit", null);
      hit.collider?.gameObject.GetComponent<EnemyController>()?.kill();
    }
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
      // Debug.Log("COLLIDED RESETTING");
      if (dataDict["other"].ToString().ToLower().Contains("bullet")) return;
      resetLaunch();
      //TODO: have some types of collisions that dont stop launch process
    }
  }

  void Launch()
  {
    // Debug.Log("LAUNCH");
    //TODO play launch animation

    swipeState = SwipeState.Launched;

    EventManager.EmitEvent("launched", null);

    //add the launch vector to the rigidbody
    rb.AddForce(launchVector * speed, ForceMode2D.Impulse);

    //remove constraints on movement
    rb.constraints = RigidbodyConstraints2D.None;

    //reset arrow trnsform
    movementArrow.transform.localScale = Vector3.zero;
    movementArrow.transform.position = transform.position;
  }

  void resetLaunch()
  {
    swipeState = SwipeState.None;
  }

}
