using UnityEngine;
using System.Collections;

public class TurnBehaviour : MonoBehaviour {
	public int d1;		//Direction the camera is moving, 
	public int d2;		//North = 0, East = 1, South = 2, West = 3
	public int turn1;	//Left Turn = 0, Right Turn = 1
	public int turn2;

	// Use this for initialization
	void Start () {
		gameObject.renderer.material.color = new Color(1.0f,1.0f,1.0f,0.0f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "MainCamera") {
			MoveCamera cam = col.gameObject.GetComponent<MoveCamera>();
			if(cam.direction == d1) {
				if(turn1 == 1)
					cam.turnRight();
				else
					cam.turnLeft();
			} else if(cam.direction == d2) {
				if(turn2 == 1)
					cam.turnRight();
				else
					cam.turnLeft();
			}
		}
	}
}
