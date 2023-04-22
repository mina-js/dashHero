using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderController : MonoBehaviour
{
  SpriteRenderer spriteRenderer;
  [SerializeField] float delay = 1f;

  void Start()
  {
    spriteRenderer = GetComponent<SpriteRenderer>();
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
