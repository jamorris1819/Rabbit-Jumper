using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
    public new GameObject camera;
	void Update () {
        if (GameObject.Find("Player").transform.position.y > camera.transform.position.y)
            camera.transform.position = new Vector3(0, GameObject.Find("Player").transform.position.y, camera.transform.position.z);
	}
}
