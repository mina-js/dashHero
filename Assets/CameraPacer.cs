using UnityEngine;

public class CameraPacer : MonoBehaviour
{
  public float speed = 1f;

  void Update()
  {
    transform.position += Vector3.right * speed * 10f * Time.deltaTime;
  }
}
