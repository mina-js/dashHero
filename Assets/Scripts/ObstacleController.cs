using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
  [SerializeField] int damageAmount = 1;
  [SerializeField] bool isDodgeable = false;

  void OnCollisionEnter2D(Collision2D other)
  {
    if (other.gameObject.CompareTag("bodyPart"))
    {
      EventManager.EmitEvent("playerHit", new Dictionary<string, object>() { { "damageAmount", damageAmount }, { "isDodgeable", isDodgeable } });
    }
  }
}
