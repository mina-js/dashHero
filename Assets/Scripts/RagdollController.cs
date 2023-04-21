using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
  SpriteRenderer launchedArmSpriteRenderer;
  SpriteRenderer leftArmSpriteRenderer;

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

    if (eventKey == "launched")
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
