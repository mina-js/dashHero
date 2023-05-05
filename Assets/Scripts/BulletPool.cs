using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
  public static BulletPool instance;

  List<GameObject> bullets = new List<GameObject>();
  public int amountToPool = 20;

  [SerializeField] List<GameObject> bulletPrefab;

  void Awake()
  {
    if (instance == null)
    {
      instance = this;
    }
  }

  void Start()
  {
    for (int i = 0; i < amountToPool; i++)
    {
      for (int j = 0; j < bulletPrefab.Count; j++)
      {
        GameObject obj = Instantiate(bulletPrefab[j]);
        //parent it under this gameobject
        obj.transform.parent = transform;
        obj.SetActive(false);
        bullets.Add(obj);
      }
    }
  }

  public GameObject GetPooledBullet(GameObject prefabToUse)
  {
    string name = prefabToUse.name;

    for (int i = 0; i < bullets.Count; i++)
    {
      if (!bullets[i].activeInHierarchy && bullets[i].name.Contains(name))
      {
        return bullets[i];
      }
    }

    return null;
  }

  public void ResetBullets()
  {
    foreach (GameObject bullet in bullets)
    {
      bullet.SetActive(false);
    }
  }
}
