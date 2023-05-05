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
  GameObject deadPool;

  [SerializeField] int damageAmount = 1;
  [SerializeField] bool isDodgeable = false;

  void Start()
  {
    gameController = GameObject.Find("GameController").GetComponent<GameController>();
    rb = GetComponent<Rigidbody2D>();

    //find gameobject tagged deadpool
    deadPool = GameObject.FindWithTag("deadPool");
  }

  void Update()
  {
    if (inDeadpool())
    {
      HandleContactWithWall();
      return;
    }

    if (direction == null || !gameController.isGameRunning())
    {
      rb.bodyType = RigidbodyType2D.Static;
      return;
    }
    else
    {
      rb.bodyType = RigidbodyType2D.Dynamic;
    }
    rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime * gameController.timeDilationFactor);
    rb.rotation = _rotation;
  }

  bool inDeadpool()
  {
    return transform.position.x <= deadPool.transform.position.x;
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

    Dictionary<string, object> data = new Dictionary<string, object>() { { "damageAmount", damageAmount }, { "isDodgeable", isDodgeable } };

    EventManager.EmitEvent("playerHit", null);
    //TODO: create a particle effect
  }
}
