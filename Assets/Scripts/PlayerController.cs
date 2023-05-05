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
  public float timeDilationFactor = 1f;
  [Range(0f, 1f)]
  public float dodgeChance = 0.5f;

  private GameObject canvas;

  // Start is called before the first frame update
  void Start()
  {
    currentHealth = maxHealth;
    EventManager.OnEventEmitted += HandleEvent;

    canvas = GameObject.Find("HUD");
  }

  public void TakeDamage(int damageAmount)
  {
    currentHealth -= (float)(damageAmount / defenseScore);
    updateHealthHUD();
  }

  void HandleEvent(string eventKey, object data)
  {
    Dictionary<string, object> dataDict = data as Dictionary<string, object>;

    if (eventKey == "playerHit")
    {
      int damageAmount = data == null ? 1 : (int)(dataDict["damageAmount"] ?? 1);
      bool isDodgeable = data == null ? true : ((bool)(dataDict["isDodgeable"]) && true);
      bool isDodge = Random.Range(0f, 1f) < dodgeChance;

      if (isDodgeable && isDodge)
      {
        // Debug.Log("Player dodged!");
        //TODO: dodge animation
        return;
      }

      //TODO: account for defense score maybe take 1/defenseScore damage
      //Debug.Log("Player health: " + currentHealth);
      TakeDamage(damageAmount);
      //update the HP textmeshpro child of canvas

    }
    else if (eventKey == "redirected")
    {
      int numRedirects = (int)dataDict["numRedirects"];
      setJumpsHUD(numRedirects);
    }
    else if (eventKey == "grabTimer")
    {
      float timeLeft = (float)dataDict["timeLeft"];
      setGrabTimerHUD(timeLeft);
    }
    else if (eventKey == "speedChanged")
    {
      float newSpeed = (float)dataDict["speed"];
      speed = newSpeed;
    }
    else if (eventKey == "inverseControlsChanged")
    {
      inverseControls = (bool)dataDict["inverseControls"];
    }
  }

  public void ResetPlayer()
  {
    currentHealth = maxHealth;
    updateHealthHUD();
    setJumpsHUD(numRedirectsPerLaunch);
    setGrabTimerHUD(timeToLaunch);
  }

  void updateHealthHUD()
  {
    TMPro.TextMeshProUGUI textMesh = canvas.GetComponentInChildren<TMPro.TextMeshProUGUI>();
    textMesh.text = "HP: " + currentHealth;
  }

  void setJumpsHUD(int val)
  {
    TMPro.TextMeshProUGUI textMesh = canvas.transform.Find("Jumps")?.GetComponent<TMPro.TextMeshProUGUI>();
    textMesh.text = "Jumps: " + val;
  }

  void setGrabTimerHUD(float val)
  {
    TMPro.TextMeshProUGUI textMesh = canvas.transform.Find("Timer")?.GetComponent<TMPro.TextMeshProUGUI>();
    textMesh.text = val.ToString("F2") + "s";
  }
}
