using UnityEngine;
using System.Collections;

public class DecisionBehaviour : MonoBehaviour {
	//Array chosen based on which way you are facing
	public GameObject[] NActionSet;
	public GameObject[] EActionSet;
	public GameObject[] SActionSet;
	public GameObject[] WActionSet;
	public int example = 5;

	// Stuff for the countdown
	public float pickTime = 3f;
	public GUIText text;
	private bool collided;
	private float timer;
	private int dir;

	// Use this for initialization
	void Start () {
		collided = false;
		DisableActions ();
		gameObject.renderer.material.color = new Color(1.0f,1.0f,1.0f,0.0f);
	}
	
	// Update is called once per frame
	void Update () {
		if(collided) {
			timer -= Time.deltaTime;
			text.text = timer.ToString ();
			if (timer <= 0)
			{
				collided = false;
				TakeAction(dir);
			}
		}
	}

	public void EnableActions(int direction){
		print ("Actions Enabled: " + direction);
		switch(direction){
		case 0:
			foreach(GameObject a in NActionSet){
				GameObject newObject = Instantiate (a) as GameObject;
			}
			break;
		case 1:
			foreach(GameObject a in EActionSet){
				GameObject newObject = Instantiate (a) as GameObject;
			}
			break;
		case 2:
			foreach(GameObject a in SActionSet){
				GameObject newObject = Instantiate (a) as GameObject;
			}
			break;
		case 3:
			foreach(GameObject a in WActionSet){
				GameObject newObject = Instantiate (a) as GameObject;
			}
			break;
		}
	}

	public void TakeAction(int direction) {
		switch(direction){
		case 0:
			NActionSet[0].GetComponent<Action>().startAction();
			break;
		case 1:
			EActionSet[0].GetComponent<Action>().startAction();
			break;
		case 2:
			SActionSet[0].GetComponent<Action>().startAction();
			break;
		case 3:
			WActionSet[0].GetComponent<Action>().startAction();
			break;
		}
		DisableActions ();
		FindObjectOfType<MoveCamera>().moves--;
	}

	public void DisableActions(){
		foreach(GameObject a in GameObject.FindGameObjectsWithTag("Action"))
			Destroy (a);
	}

	void OnTriggerExit(Collider col) {
		collided = false;
		text.text = "";

	}

	void OnTriggerEnter(Collider col){
		if(col.gameObject.tag == "MainCamera"){
			print("Collision!");
			dir = col.gameObject.GetComponent<MoveCamera>().direction;
			EnableActions(dir);
			collided = true;
			timer = pickTime;
		}
	}
}
