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
  GameController gameController;
  PlayerController playerController;
  int numRedirects = 0;
  float timeGrabbed = 0f;
  [SerializeField] Camera mainCamera;
  Rigidbody2D rb;
  public SwipeState swipeState;
  public float effectsCutoff = 0.1f; //how fast you have to be moving to run effects
  [SerializeField] private InputAction press, screenPos;
  Vector3 currentScreenPos;
  Vector3 startScreenPos;
  Vector2 launchVector;
  [SerializeField] GameObject movementArrow;
  bool isSlashing;
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
    isSlashing = false;

    gameController = GameObject.Find("GameController").GetComponent<GameController>();
    playerController = transform.parent.GetComponent<PlayerController>();

    swipeState = SwipeState.None;

    // swipeTimer = 0f;

    rb = GetComponent<Rigidbody2D>();

    EventManager.OnEventEmitted += HandleEvent;

    screenPos.Enable();
    press.Enable();

    screenPos.performed += context => currentScreenPos = context.ReadValue<Vector2>();

    press.performed += _ => { StartCoroutine(OnTouch()); };
    press.canceled += _ => { isDragging = false; };

    timeGrabbed = 0f;

    UpdateUIRedirects();
  }


  IEnumerator OnTouch()
  {
    if (!gameController.isGameRunning()) yield break;

    OnGrab();
    while (isDragging)
    {
      OnDrag();
      yield return null;
    }
    OnDrop();
  }

  void UpdateUIRedirects()
  {
    EventManager.EmitEvent("redirected", new Dictionary<string, object> { { "numRedirects", playerController.numRedirectsPerLaunch - numRedirects } });
  }

  void UpdateUIGrabTimer()
  {
    EventManager.EmitEvent("grabTimer", new Dictionary<string, object> { { "timeLeft", playerController.timeToLaunch - timeGrabbed } });
  }

  void OnGrab()
  {
    numRedirects++;

    Debug.Log("GRABBED");
    if (numRedirects > playerController.numRedirectsPerLaunch) return;

    timeGrabbed = 0f;
    UpdateUIGrabTimer();

    gameController.slowTime(playerController.timeDilationFactor);

    isDragging = true;

    startScreenPos = WorldPos;

    Debug.Log("FREEZING! " + rb.constraints.ToString());
    //freeze the rigidbody
    rb.velocity = Vector2.zero;
    rb.bodyType = RigidbodyType2D.Static;
    // rb.constraints = RigidbodyConstraints2D.FreezeAll;
    Debug.Log("frozen???? " + rb.constraints.ToString());

    swipeState = SwipeState.Launching;

    movementArrow.transform.position = transform.position;
  }

  void OnDrag()
  {
    timeGrabbed += Time.deltaTime;
    UpdateUIGrabTimer();

    //TODO: theres an issue here if you click to the left of it?

    //Point the arrow in direction of launchvector with scaling
    launchVector = Mathf.Sign(playerController.inverseControls ? -1f : 1f) * (WorldPos - startScreenPos);
    movementArrow.transform.rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, launchVector));
    movementArrow.transform.localScale = new Vector3(1, launchVector.magnitude * 0.01f, 1);
    movementArrow.transform.position = transform.position;

    if (timeGrabbed > playerController.timeToLaunch) Launch();

  }

  void OnDrop()
  {
    // Debug.Log("DROP");
    if (swipeState != SwipeState.Launched) Launch();
  }

  void Update()
  {
    if (!gameController.isGameRunning()) return;

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

  private void DetectEnemyStrike()
  {
    RaycastHit2D hit = Physics2D.Raycast(transform.position, rb.velocity, Mathf.Infinity, LayerMask.GetMask("enemyLayer"));

    if (hit.collider == null)
    {
      isSlashing = false;
      return;
    };

    float timeToHit = hit.distance / rb.velocity.magnitude;

    if (timeToHit < 0.06 && !isSlashing)
    {
      hit.collider?.gameObject.GetComponent<EnemyController>()?.hit(playerController.damagePerStrike);
      isSlashing = true;
      EventManager.EmitEvent("enemyHit", null);
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
      if (dataDict["other"].ToString().ToLower().Contains("bullet")) return;

      resetLaunch();
    }
  }

  void Launch()
  {
    Debug.Log("LAUNCH");
    gameController.resetTimeDilation();

    swipeState = SwipeState.Launched;

    EventManager.EmitEvent("launched", null);
    UpdateUIRedirects();

    //add the launch vector to the rigidbody

    //remove constraints on movement
    rb.bodyType = RigidbodyType2D.Dynamic;
    rb.AddForce(launchVector * playerController.speed, ForceMode2D.Impulse);

    //reset arrow trnsform
    movementArrow.transform.localScale = Vector3.zero;
    movementArrow.transform.position = transform.position;

    timeGrabbed = 0f;
    isDragging = false;

    UpdateUIGrabTimer();
  }

  void resetLaunch()
  {
    swipeState = SwipeState.None;
    numRedirects = 0;
    UpdateUIRedirects();
  }

}
