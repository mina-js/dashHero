using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartCollisionBehaviorController : MonoBehaviour
{
  PlayerController playerController;
  Rigidbody2D rb;

  void Start()
  {
    playerController = transform.parent.GetComponent<PlayerController>();
    //get rb of torso
    rb = transform.parent.Find("torso").GetComponent<Rigidbody2D>();
  }
  //Exists to handle collision events concerning this body part object

  void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.tag == "bodyPart") return; //For now, no behavior when touching other body parts, may change

    if (collision.gameObject.tag == "wall")
    {
      bounceOffWall(collision);
    }

    Dictionary<string, object> collisionEventData = new Dictionary<string, object>();

    collisionEventData.Add("bodyPartName", gameObject.name);
    collisionEventData.Add("other", collision.gameObject.name);

    EventManager.EmitEvent("bodyPartCollision", collisionEventData);
  }

  void bounceOffWall(Collision2D collision)
  {
    Debug.Log("bouncing off the wall!");
    Vector2 normal = collision.GetContact(0).normal;
    Vector2 newDirection = Vector2.Reflect(rb.velocity, normal);
    newDirection *= playerController.bounciness;
    rb.velocity = newDirection;
  }

}
