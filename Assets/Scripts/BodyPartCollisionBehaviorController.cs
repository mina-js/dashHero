using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartCollisionBehaviorController : MonoBehaviour
{
  //Exists to handle collision events concerning this body part object

  void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.tag == "bodyPart") return; //For now, no behavior when touching other body parts, may change

    Dictionary<string, object> collisionEventData = new Dictionary<string, object>();

    collisionEventData.Add("bodyPartName", gameObject.name);
    collisionEventData.Add("other", collision.gameObject.name);

    EventManager.EmitEvent("bodyPartCollision", collisionEventData);
  }
}
