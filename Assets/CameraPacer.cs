using UnityEngine;

public class CameraPacer : MonoBehaviour
{
  public float speed = 1f;

  GameController gameController;
  [SerializeField] GameObject player;

  void Start()
  {
    gameController = GameObject.Find("GameController").GetComponent<GameController>();
  }

  void Update()
  {
    if (gameController.isGameRunning())
    {
      PaceCamera();
    }
  }

  void PaceCamera()
  {

    //TODO: move a lil forward but not super fast which is what this does
    // if (player.transform.position.x > transform.position.x)
    // {
    //   transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
    // }
    // else
    // {
    transform.position += Vector3.right * speed * 10f * Time.deltaTime * gameController.timeDilationFactor;
    //}

    //find Edge R
    GameObject edgeR = GameObject.Find("Edge R");
    //if x is greater than the x of edge R, then emit that gameEnded
    if (transform.position.x > edgeR.transform.position.x)
    {
      EventManager.EmitEvent("gameEnded", null);
    }
  }
}
