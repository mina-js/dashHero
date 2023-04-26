using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
  SpriteRenderer launchedArmSpriteRenderer;
  SpriteRenderer leftArmSpriteRenderer;

  bool isSlashing = false;

  void Start()
  {
    GameObject launchedArm = transform.Find("launchedArm")?.gameObject;
    launchedArmSpriteRenderer = launchedArm?.GetComponent<SpriteRenderer>();

    GameObject leftArm = transform.Find("leftUpperArm")?.gameObject;
    leftArmSpriteRenderer = leftArm?.GetComponent<SpriteRenderer>();

    EventManager.OnEventEmitted += HandleEvent;

    isSlashing = false;
  }

  void HandleEvent(string eventKey, object data)
  {
    Dictionary<string, object> dataDict = data as Dictionary<string, object>;

    if (eventKey == "launched" && launchedArmSpriteRenderer != null && leftArmSpriteRenderer != null)
    {
      launchedArmSpriteRenderer.enabled = true;
      leftArmSpriteRenderer.enabled = false;
    }
    else if (eventKey == "bodyPartCollision" && launchedArmSpriteRenderer != null && leftArmSpriteRenderer != null)
    {
      launchedArmSpriteRenderer.enabled = false;
      leftArmSpriteRenderer.enabled = true;

    }
    else if (eventKey == "enemyHit")
    {
      if (!isSlashing) FlashTrailRenderer();

    }
  }

  void FlashTrailRenderer()
  {
    //get the launchedarm
    GameObject launchedArm = transform.Find("launchedArm")?.gameObject;
    TrailRenderer trailRenderer = launchedArm?.GetComponent<TrailRenderer>();
    isSlashing = true;

    trailRenderer.enabled = true;
    trailRenderer.emitting = true;

    StartCoroutine(DisableTrailRenderer(trailRenderer));
  }

  IEnumerator DisableTrailRenderer(TrailRenderer trailRenderer)
  {
    yield return new WaitForSeconds(0.25f); //give it 0.25s of tracking for slash
    trailRenderer.emitting = false; //stop emitting, but keep visible for 1s
    yield return new WaitForSeconds(0.5f);
    trailRenderer.Clear();//reset it all
    trailRenderer.enabled = false;
    isSlashing = false;
  }
}
