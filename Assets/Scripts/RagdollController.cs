using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
  public float speed = 50f;
  public Camera mainCamera;

  void Start()
  {
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

    Debug.Log("ragdoll knows about collision: " + dataDict["bodyPartName"] + " collided with " + dataDict["other"]);
  }
}
