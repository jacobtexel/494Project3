using UnityEngine;
using System.Collections;
using System;





public class DecisionV2 : MonoBehaviour {
	//Option set chosen based on direction moving
	public Options NActionSet;
	public Options EActionSet;
	public Options SActionSet;
	public Options WActionSet;
	private Options current;


	// Stuff for the countdown
	public float pickTime = 3f;
	public GUIText text;
	private float timer;
	private int dir;

	// Information about collider
	private bool collided;
	private Collider playerCol;


	// Use this for initialization
	void Start () {
		collided = false;
		gameObject.renderer.material.color = new Color(1.0f,1.0f,1.0f,0.0f);
	}
	
	// Update is called once per frame
	void Update () {
		if(collided) {
			if(current == null){
				Debug.LogError("Player options currently null");
				return;
			}
			if(Input.GetButton ("backward") && current.backward) {
				playerCol.GetComponent<MoveCamera>().turnAround();
				actionTaken ();
			} else if(Input.GetButton("left") && current.left) {
				playerCol.GetComponent<MoveCamera>().turnLeft ();
				actionTaken ();
			} else if(Input.GetButton("right") && current.right) {
				playerCol.GetComponent<MoveCamera>().turnRight ();
				actionTaken ();
			} else if(Input.GetButton("forward") && current.forward) {
				playerCol.GetComponent<MoveCamera>().continueMoving();
				actionTaken ();
			} else {
				timer -= Time.deltaTime;
				text.text = timer.ToString ();
				if (timer <= 0)
				{
					collided = false;
					TakeAction(dir);
				}
			}
		}
	}

	public void TakeAction(int direction) {
		//Forward, right, left, backward
		if (current.forward) {
			playerCol.gameObject.GetComponent<MoveCamera>().continueMoving();
		} else if(current.right) {
			playerCol.gameObject.GetComponent<MoveCamera>().turnLeft();
		} else if(current.left){
			playerCol.GetComponent<MoveCamera>().turnRight();
		} else if(current.backward){
			playerCol.GetComponent<MoveCamera>().turnAround();
		} else {
			Debug.Log("No movement option was available to player "+playerCol.gameObject.name);
		}
		actionTaken ();
	}

	void OnTriggerExit(Collider col) {
		actionTaken ();
	}

	void OnTriggerEnter(Collider col){
		if(col.gameObject.tag == "MainCamera"){
			print("Collision!");
			playerCol = col;
			dir = col.gameObject.GetComponent<MoveCamera>().direction;
			switch(dir){
			case 0:
				current = NActionSet;
				break;
			case 1:
				current = EActionSet;
				break;
			case 2:
				current = SActionSet;
				break;
			case 3:
				current = WActionSet;
				break;
			default:
				Debug.Log ("Unrecognized direction for player collision");
				break;
			}
			collided = true;
			timer = pickTime;
		}
	}

	void actionTaken(){
		collided = false;
		text.text = "";
	}

}
