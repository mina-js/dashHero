using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
  float length;
  float startPosition;

  float buffer = 2f;
  [SerializeField] Transform cameraPacer;
  [SerializeField] float parallaxEffect;
  [SerializeField] GameObject altLayer;
  GameObject activeLayer;

  // Start is called before the first frame update
  void Start()
  {
    activeLayer = gameObject;

    startPosition = cameraPacer.position.x;
    length = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
  }

  // Update is called once per frame
  void Update()
  {
    float rightMostEdgeX = activeLayer.transform.position.x + (length / 2) + buffer;
    float rightCameraEdgeX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height / 2f, 0)).x;

    if (rightCameraEdgeX > rightMostEdgeX)
    {
      Debug.Log("this " + gameObject.name + "is now not on camera" + "camera edge " + rightCameraEdgeX
      + "this edge " + rightMostEdgeX);
      //mvoe the altlayer to taht x
      if (altLayer != null)
      {
        altLayer.transform.position = new Vector3(rightMostEdgeX + (length / 2) - 2 * buffer, altLayer.transform.position.y, altLayer.transform.position.z);
        //swap the layers
        GameObject temp = activeLayer;
        activeLayer = altLayer;
        altLayer = temp;
      }

    }

    float distMoved = ((cameraPacer.position.x - startPosition) * parallaxEffect);
    activeLayer.transform.position = new Vector3(activeLayer.transform.position.x + distMoved, activeLayer.transform.position.y, activeLayer.transform.position.z);
    if (altLayer != null) altLayer.transform.position = new Vector3(altLayer.transform.position.x + distMoved, altLayer.transform.position.y, altLayer.transform.position.z);

    startPosition = cameraPacer.position.x;

  }
}
