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
	private int playerNum;
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
			if(current.backward && (Input.GetButtonDown ("backward") && playerNum == 1 || Input.GetButtonDown ("backward2") && playerNum == 2)) {
				playerCol.GetComponent<MoveCamera>().turnAround();
				actionTaken ();
			} else if(current.left && (Input.GetButtonDown("left") && playerNum == 1 || Input.GetButtonDown("left2") && playerNum == 2)) {
				playerCol.GetComponent<MoveCamera>().turnLeft ();
				actionTaken ();
			} else if(current.right && (Input.GetButtonDown("right") && playerNum == 1 || Input.GetButtonDown("right2") && playerNum == 2)) {
				playerCol.GetComponent<MoveCamera>().turnRight ();
				actionTaken ();
			} else if(current.forward && (Input.GetButtonDown("forward") && playerNum == 1 || Input.GetButtonDown("forward2") && playerNum == 2)) {
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
		} else if(current.left) {
			playerCol.gameObject.GetComponent<MoveCamera>().turnLeft();
		} else if(current.right){
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
			if(col.gameObject.name == "Player1Cam")
				playerNum = 1;
			else {
				playerNum = 2;
			}
			print("Collision!");
			playerCol = col;
			dir = col.gameObject.GetComponent<MoveCamera>().direction;
			text = col.gameObject.GetComponent<MoveCamera>().messageText;
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
