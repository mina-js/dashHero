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
  [SerializeField] float delay = 1f;
  [SerializeField] BorderType borderType;

  void Start()
  {
    spriteRenderer = GetComponent<SpriteRenderer>();
  }

  void Update()
  {
    float viewportWidth = Camera.main.orthographicSize * 2f * Camera.main.aspect;
    float viewportHeight = Camera.main.orthographicSize * 2f;

    if (borderType == BorderType.Left)
    {
      Vector3 leftMostWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2f, 0));
      transform.position = leftMostWorldPoint;
      transform.localScale = new Vector3(viewportHeight, 8, 1); //left border keeps letting them through
    }
    else if (borderType == BorderType.Right)
    {
      Vector3 rightMostWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height / 2f, 0));
      transform.position = rightMostWorldPoint;
      transform.localScale = new Vector3(viewportHeight, 2, 1);
    }
    else if (borderType == BorderType.Top)
    {
      Vector3 topMostWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height, 0));
      transform.position = topMostWorldPoint;
      transform.localScale = new Vector3(viewportWidth, 2, 1);
    }
    else if (borderType == BorderType.Bottom)
    {
      Vector3 bottomMostWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, 0, 0));
      transform.position = bottomMostWorldPoint;
      transform.localScale = new Vector3(viewportWidth, 2, 1);
    }
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
