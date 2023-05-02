using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
  [SerializeField] int damageAmount = 1;

  void OnCollisionEnter2D(Collision2D other)
  {
    Debug.Log("HIT " + other.gameObject);
    if (other.gameObject.CompareTag("bodyPart"))
    {
      EventManager.EmitEvent("playerHit", new Dictionary<string, object>() { { "damageAmount", damageAmount } });
    }
  }
}
