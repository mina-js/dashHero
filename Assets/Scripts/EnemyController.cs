using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
  [SerializeField] private GameObject bulletPrefab;
  [SerializeField] float speed = 10f;

  [SerializeField] Vector3 startPosition;
  Rigidbody2D rb;
  Transform target;
  private Vector2 direction;
  public float frequencyOfFire = 1f;

  public float maxHP = 1f;
  public float hp;

  GameController gameController;

  // Start is called before the first frame update
  void Start()
  {
    gameController = GameObject.Find("GameController").GetComponent<GameController>();

    //find tagged enemyTarget, could be better but theres only one player at the moment
    target = GameObject.FindGameObjectWithTag("bodyPart")?.transform;
    rb = GetComponent<Rigidbody2D>();
    direction = Random.insideUnitCircle.normalized;

    InvokeRepeating("launchBullet", 0f, frequencyOfFire);
  }

  // Update is called once per frame
  void Update()
  {
    if (!gameController.isGameRunning()) return;
    //move forward by speed amount
    rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime * gameController.timeDilationFactor);
  }

  public void resetPosition()
  {
    hp = maxHP;

    transform.position = startPosition;

    GetComponent<SpriteRenderer>().enabled = true;
    GetComponent<Collider2D>().enabled = true;

    rb.constraints = RigidbodyConstraints2D.None;
  }

  void launchBullet()
  {
    if (frequencyOfFire == 0 || !gameController.isGameRunning()) return;

    Vector3 bulletDir = (target.transform.position - transform.position).normalized;
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

    //get a bullet from the pool
    GameObject bullet = getNewBullet();
    if (bullet == null)
    {
      Debug.Log("No bullets available?!?!?!?!?");
      return;
    }
    bullet.SetActive(true);

    //set the position of the bullet to the position of the enemy
    bullet.transform.position = transform.position;
    //point it at target
    BulletController bulletController = bullet.GetComponent<BulletController>();
    bulletController.direction = bulletDir;
    bulletController._rotation = angle;
    bulletController.onDestroy = destroyBullet;
  }

  void OnCollisionEnter2D(Collision2D collision)
  {
    if (!collision.gameObject.CompareTag("wall")) return; //dont bounce off anything but walls

    Vector2 normal = collision.contacts[0].normal;
    direction = Vector2.Reflect(direction, normal);
  }

  GameObject getNewBullet()
  {
    return BulletPool.instance.GetPooledBullet();
    // return Instantiate(bulletPrefab, transform.position, transform.rotation);
  }

  void destroyBullet(GameObject bulletToRemove)
  {
    bulletToRemove.SetActive(false);
    // Destroy(bulletToRemove);
  }

  //called from any other object that wants to kill this enemy
  public void hit(int amountDamage)
  {
    Debug.Log("HIT " + maxHP + " " + hp + " " + amountDamage);
    hp -= amountDamage;

    GetComponent<SpriteRenderer>().color = Color.red;
    rb.constraints = RigidbodyConstraints2D.FreezeAll;

    Invoke("handleHitEffects", 0.5f);
  }

  void handleHitEffects()
  {
    if (hp <= 0)
    {
      //make spriterenderer invisible and remove from physics
      GetComponent<SpriteRenderer>().enabled = false;
      GetComponent<Collider2D>().enabled = false;
      frequencyOfFire = 0f;
    }
    else
    {
      GetComponent<SpriteRenderer>().color = Color.white;
      rb.constraints = RigidbodyConstraints2D.None;
    }
  }
}
