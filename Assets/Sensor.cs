using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
  Drag dragController;

  void Start()
  {
    dragController = FindObjectOfType<Drag>();
  }

  void OnMouseDown()
  {
    //dragController.OnMouseDown();
  }

  //stops registering after swipMS milliseconds
  void OnMouseDrag()
  {
    //dragController.OnMouseDrag();
  }

  void OnMouseUp()
  {
    //dragController.OnMouseUp();
  }
}
