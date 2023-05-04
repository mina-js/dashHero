using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum BorderType
{
  Top,
  Bottom,
  Left,
  Right
}

public class BorderController : MonoBehaviour
{
  SpriteRenderer spriteRenderer;
  Rigidbody2D rb;
  [SerializeField] float delay = 1f;
  [SerializeField] BorderType borderType;
  [SerializeField] float buffer = 5f;

  void Start()
  {
    spriteRenderer = GetComponent<SpriteRenderer>();
    rb = GetComponent<Rigidbody2D>();
    setToCameraSpace();
  }

  void Update()
  {
    setToCameraSpace();
  }

  void setToCameraSpace()
  {
    float viewportWidth = Camera.main.orthographicSize * 2f * Camera.main.aspect;
    float viewportHeight = Camera.main.orthographicSize * 2f;

    if (borderType == BorderType.Left)
    {
      Vector3 leftMostWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2f, 0));
      leftMostWorldPoint.x -= buffer;
      goTowardsPoint(leftMostWorldPoint);
      transform.localScale = new Vector3(viewportHeight, 8, 1); //left border keeps letting them through
    }
    else if (borderType == BorderType.Right)
    {
      //TODO: last part is now that this line is visible on screen, want it to be just off screen hmm
      Vector3 rightMostWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height / 2f, 0));
      rightMostWorldPoint.x += buffer;
      goTowardsPoint(rightMostWorldPoint);
      transform.localScale = new Vector3(viewportHeight, 2, 1);
    }
    else if (borderType == BorderType.Top)
    {
      Vector3 topMostWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height, 0));
      topMostWorldPoint.y += buffer;
      goTowardsPoint(topMostWorldPoint);
      transform.localScale = new Vector3(viewportWidth, 2, 1);
    }
    else if (borderType == BorderType.Bottom)
    {
      Vector3 bottomMostWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, 0, 0));
      bottomMostWorldPoint.y -= buffer;
      goTowardsPoint(bottomMostWorldPoint);
      transform.localScale = new Vector3(viewportWidth, 2, 1);
    }
  }

  void goTowardsPoint(Vector3 point)
  {
    Vector2 velocity = (point - transform.position);
    rb.velocity = velocity * 10f;
  }

  void OnCollisionEnter2D(Collision2D collision)
  {
    if (delay > 0f && collision.gameObject.CompareTag("bodyPart"))
    {
      spriteRenderer.enabled = false;
      StartCoroutine(ReenableSpriteRenderer());
    }
  }

  IEnumerator ReenableSpriteRenderer()
  {
    yield return new WaitForSeconds(delay);
    spriteRenderer.enabled = true;
  }

}
