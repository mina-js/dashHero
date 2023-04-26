using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  //These are the RPG characteristics that we can play around with
  public float speed;
  public bool inverseControls = true;
  public float timeToLaunch = 1f;
  public int numRedirectsPerLaunch = 3;
  public int damagePerStrike = 1;
  public int maxHealth = 10;
  public float currentHealth;
  public int defenseScore = 1;
  public float bounciness = 0.5f;

  private GameObject canvas;

  // Start is called before the first frame update
  void Start()
  {
    currentHealth = maxHealth;
    EventManager.OnEventEmitted += HandleEvent;

    canvas = GameObject.Find("Canvas");
  }

  void HandleEvent(string eventKey, object data)
  {
    Dictionary<string, object> dataDict = data as Dictionary<string, object>;

    if (eventKey == "playerHit")
    {
      //TODO: account for defense score maybe take 1/defenseScore damage
      currentHealth -= (1f / defenseScore);
      //Debug.Log("Player health: " + currentHealth);

      //update the HP textmeshpro child of canvas
      TMPro.TextMeshProUGUI textMesh = canvas.GetComponentInChildren<TMPro.TextMeshProUGUI>();
      textMesh.text = "HP: " + currentHealth;
    }
    else if (eventKey == "redirected")
    {
      int numRedirects = (int)dataDict["numRedirects"];
      //update the Jumps textmeshpro child of canvas
      TMPro.TextMeshProUGUI textMesh = canvas.transform.Find("Jumps")?.GetComponent<TMPro.TextMeshProUGUI>();
      textMesh.text = "Jumps: " + numRedirects;
    }
    else if (eventKey == "grabTimer")
    {
      float timeLeft = (float)dataDict["timeLeft"];
      TMPro.TextMeshProUGUI textMesh = canvas.transform.Find("Timer")?.GetComponent<TMPro.TextMeshProUGUI>();
      textMesh.text = timeLeft.ToString("F2") + "s";
    }
  }
}
