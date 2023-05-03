using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
  [SerializeField] float speed = 10f;
  public Vector2 direction;

  public float _rotation;

  GameController gameController;

  Rigidbody2D rb;

  public System.Action<GameObject> onDestroy;

  void Start()
  {
    gameController = GameObject.Find("GameController").GetComponent<GameController>();
    rb = GetComponent<Rigidbody2D>();
  }

  void Update()
  {
    if (direction == null || gameController.isGameRunning()) return;
    rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime * gameController.timeDilationFactor);
    rb.rotation = _rotation;
  }

  void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.CompareTag("bodyPart"))
    {
      HandleContactWithPlayerBody();
    }
    else if (collision.gameObject.CompareTag("wall"))
    {
      HandleContactWithWall();
    }
  }

  void HandleContactWithWall()
  {
    onDestroy?.Invoke(gameObject);
  }

  void HandleContactWithPlayerBody()
  {
    Debug.Log("HIT BY BULLET!!!!");
    onDestroy?.Invoke(gameObject);
    EventManager.EmitEvent("playerHit", null);
    //TODO: create a particle effect
  }
}
