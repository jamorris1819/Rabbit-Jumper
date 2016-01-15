using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pool {

    List<GameObject> objects;

    public Pool(GameObject type, int count)
    {
        objects = new List<GameObject>(count);
        for(int i = 0; i < count; i++)
        {
            GameObject tempObj = GameObject.Instantiate(type);
            tempObj.SetActive(false);
            objects.Add(tempObj);
        }
    }

    public GameObject Grab()
    {
        if (objects.Count <= 0)
        {    // We can't let more objects be placed than the pool size
            return null;
        }
        GameObject obj = objects[0];
        obj.SetActive(true);
        objects.RemoveAt(0);
        return obj;
    }

    public GameObject Place(float x, float y)
    {
        if (objects.Count <= 0)
        {    // We can't let more objects be placed than the pool size
            Debug.Log("none");
            return null;
        }
        GameObject obj = objects[0];
        obj.SetActive(true);
        obj.transform.position = new Vector3(x, y);
        objects.RemoveAt(0);
        return obj;
    }

    public GameObject Place(Vector3 position)
    {
        return Place(position.x, position.y);
    }

    public void Remove(GameObject gameObject)
    {
        gameObject.SetActive(false);
        objects.Add(gameObject);
    }
}
