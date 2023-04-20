using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public int maxHealth = 10;
  public int currentHealth;

  // Start is called before the first frame update
  void Start()
  {
    currentHealth = maxHealth;
    EventManager.OnEventEmitted += HandleEvent;
  }

  void HandleEvent(string eventKey, object data)
  {
    if (eventKey == "playerHit")
    {
      currentHealth--;
      Debug.Log("Player health: " + currentHealth);
    }
  }
}
