using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
  //bounds timeDilationFactor to positive number

  [Range(0f, 100f)]
  public float timeDilationFactor = 1f;

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

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
