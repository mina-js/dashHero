using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
  [SerializeField] float speed = 0.1f;
  Rigidbody2D rb;
  Transform target;
  Vector2 moveDirection;

  // Start is called before the first frame update
  void Start()
  {
    //find tagged enemyTarget, could be better but theres only one player at the moment
    target = GameObject.FindGameObjectWithTag("enemyTarget").transform;
  }

  void Awake()
  {
    rb = GetComponent<Rigidbody2D>();
  }

  // Update is called once per frame
  void Update()
  {
    if (target == null) return;

    //Calculate the direction and roation
    Vector3 direction = (target.transform.position - transform.position).normalized;
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    rb.rotation = angle;
    moveDirection = direction;
  }

  void FixedUpdate()
  {
    if (target == null) return;

    //Then actually move
    rb.velocity = new Vector2(moveDirection.x, moveDirection.y) * speed;
  }
}
