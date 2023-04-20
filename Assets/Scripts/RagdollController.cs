using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Currently just using this to pass variables to all the body part children
public class RagdollController : MonoBehaviour
{
  public float speed = 50f;
  public Camera mainCamera;

  SpriteRenderer launchedArmSpriteRenderer;
  SpriteRenderer leftArmSpriteRenderer;

  // void Start()
  // {
  //   EventManager.OnEventEmitted += HandleEvent;
  // }

  void Start()
  {
    GameObject launchedArm = transform.Find("launchedArm").gameObject;
    launchedArmSpriteRenderer = launchedArm.GetComponent<SpriteRenderer>();

    GameObject leftArm = transform.Find("leftUpperArm").gameObject;
    leftArmSpriteRenderer = leftArm.GetComponent<SpriteRenderer>();

    EventManager.OnEventEmitted += HandleEvent;
  }

  void HandleEvent(string eventKey, object data)
  {
    Dictionary<string, object> dataDict = data as Dictionary<string, object>;

    if (dataDict == null)
    {
      Debug.Log("data is not a dictionary");
      return;
    }
    else if (eventKey == "launched")
    {
      launchedArmSpriteRenderer.enabled = true;
      leftArmSpriteRenderer.enabled = false;
    }
    else if (eventKey == "bodyPartCollision")
    {
      launchedArmSpriteRenderer.enabled = false;
      leftArmSpriteRenderer.enabled = true;
    }
  }
}
