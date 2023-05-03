using UnityEngine;

public class CameraPacer : MonoBehaviour
{
  public float speed = 1f;

  GameController gameController;

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
    transform.position += Vector3.right * speed * 10f * Time.deltaTime * gameController.timeDilationFactor;

    //find Edge R
    GameObject edgeR = GameObject.Find("Edge R");
    //if x is greater than the x of edge R, then emit that gameEnded
    if (transform.position.x > edgeR.transform.position.x)
    {
      EventManager.EmitEvent("gameEnded", null);
    }
  }
}
