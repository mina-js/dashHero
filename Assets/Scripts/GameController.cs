using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum GameState { MainMenu, Paused, Playing, End, Settings }

public class GameController : MonoBehaviour
{
  //bounds timeDilationFactor to positive number

  [Range(0f, 100f)]
  public float timeDilationFactor = 1f;
  public GameState gameState = GameState.MainMenu;

  // Start is called before the first frame update
  void Start()
  {
    EventManager.OnEventEmitted += HandleEvent;
  }

  void HandleEvent(string eventKey, object data)
  {
    Dictionary<string, object> dataDict = data as Dictionary<string, object>;

    if (eventKey == "startGame")
    {
      startGame();
    }
    else if (eventKey == "endGame")
    {
      endGame();
    }
    else if (eventKey == "gameResumed")
    {
      gameState = GameState.Playing;
    }
    else if (eventKey == "pauseGame")
    {
      pauseGame();
    }
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void resetGame()
  {
    GameObject player = GameObject.Find("Roleplayer");

    Rigidbody2D[] rigidBodies = player.GetComponentsInChildren<Rigidbody2D>();

    rigidBodies.ToList().ForEach(rb => rb.isKinematic = true);

    rigidBodies.ToList().ForEach(rb =>
    {
      rb.position = new Vector3(0, 0, 0);
      rb.rotation = 0;
    });

    rigidBodies.ToList().ForEach(rb => rb.isKinematic = false);

    Transform cameraPacer = GameObject.Find("CameraPacer").transform;
    cameraPacer.position = new Vector3(0, 0, 0);

    List<EnemyController> enemies = new List<EnemyController>();
    enemies.AddRange(GameObject.FindGameObjectsWithTag("enemy").Select(enemy => enemy.GetComponent<EnemyController>()));
    enemies.ForEach(enemy => enemy.resetPosition());

    BulletPool bulletPool = GameObject.Find("Bullet Pool").GetComponent<BulletPool>();
    bulletPool.ResetBullets();

    PlayerController playerController = GameObject.Find("Roleplayer").GetComponent<PlayerController>();
    playerController.ResetPlayer();
  }

  public bool isGameRunning()
  {
    return gameState == GameState.Playing;
  }

  public void startGame()
  {
    Debug.Log("STARTING");
    resetGame();

    gameState = GameState.Playing;
    EventManager.EmitEvent("gameStarted");
  }

  public void endGame()
  {
    gameState = GameState.End;
  }

  public void pauseGame()
  {
    Debug.Log("PAUSING IN GAME CONTROLLER");
    if (gameState == GameState.Paused)
    {
      gameState = GameState.Playing;
      EventManager.EmitEvent("gameResumed");
      return;
    }
    else
    {
      gameState = GameState.Paused;
      EventManager.EmitEvent("gamePaused");
    }

  }

  public void slowTime(float factor)
  {
    timeDilationFactor = 0.25f * factor;
  }

  public void resetTimeDilation()
  {
    timeDilationFactor = 1f;
  }
}
