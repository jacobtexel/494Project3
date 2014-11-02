using UnityEngine;
using System.Collections;

public class CameraMovementBehaviour : MonoBehaviour {
	public float speed = 3f;
	public bool isStopped = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(!isStopped)
		{
			transform.position += Vector3.back * speed * Time.deltaTime;
		}
	}
}
