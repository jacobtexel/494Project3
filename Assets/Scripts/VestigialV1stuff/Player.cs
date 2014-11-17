using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public int playerNum;
	public int score;
	public bool cat; // Sets the player as the "Cat" so he can eat the mice
	public bool respawning;
	public bool collided;
	public GUIText scoreText;
	public Options current;
	private float timer;
	public float pickTime = 3f;
	public GUITexture timerBar;

	//Input strings for movement
	public string forward;
	public string left;
	public string right;
	public string backward;


	// Use this for initialization
	void Start () {
		score = 0;
		respawning = false;
		cat = false;
		scoreText.text = score.ToString ();
		timerBar.enabled = false;
		// respawn ();
	}
	
	// Update is called once per frame
	void Update () {		
		scoreText.text = score.ToString ();
		if(collided) {
			timerBar.enabled = true;
			Vector3 barscale = timerBar.transform.localScale;
			barscale.x = .5f * (timer/pickTime);
			timerBar.transform.localScale = barscale;
			if(current == null){
				Debug.LogError("Player options currently null");
				return;
			}
			if(current.backward && Input.GetButtonDown (backward)) {
				GetComponent<MoveCamera>().turnAround();
				actionTaken ();
			} else if(current.left && Input.GetButtonDown(left)) {
				GetComponent<MoveCamera>().turnLeft ();
				actionTaken ();
			} else if(current.right && Input.GetButtonDown(right)) {
				GetComponent<MoveCamera>().turnRight ();
				actionTaken ();
			} else if(current.forward && Input.GetButtonDown(forward)) {
				GetComponent<MoveCamera>().continueMoving();
				actionTaken ();
			} else {
				timer -= Time.deltaTime;
				if (timer <= 0)
				{
					collided = false;
					TakeAction();
				}
			}
		} else {
			timerBar.enabled = false;
		}
	}

	void OnTriggerStay(Collider col) {
		if(col.tag == "Decision" && collided){
			transform.position = Vector3.MoveTowards(transform.position, col.transform.position, 3f);
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.tag == "Player" && cat) {
			Player player = col.gameObject.GetComponent<Player>();
			if(!player.respawning) {
				score++;
				player.startRespawn();
				if(score >= 10){
					PlayerPrefs.SetString("winner", playerNum.ToString());
					Application.LoadLevel("_End_screen");
				}
			}
		} else if (col.tag == "Powerup") {
			GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
			for (int i=0; i<players.Length; i++){
				players[i].GetComponent<Player>().cat = false;
				players[i].GetComponent<Player>().transform.localScale = new Vector3(1, 1, 1);
			}
			cat = true;
			col.gameObject.GetComponent<PowerupAction>().startRespawn();
			transform.localScale = new Vector3(2, 2, 2);
			GameObject.FindGameObjectWithTag("Vignette").layer = 7 + playerNum;
			GameObject.FindGameObjectWithTag("Vignette").guiTexture.color = this.gameObject.renderer.material.color;
		} else if (col.tag == "Decision") {
			print("Collision!");
			switch(GetComponent<MoveCamera>().direction){
			case 0:
				current = col.gameObject.GetComponent<DecisionV2>().NActionSet;
				break;
			case 1:
				current = col.gameObject.GetComponent<DecisionV2>().EActionSet;
				break;
			case 2:
				current = col.gameObject.GetComponent<DecisionV2>().SActionSet;
				break;
			case 3:
				current = col.gameObject.GetComponent<DecisionV2>().WActionSet;
				break;
			default:
				Debug.Log ("Unrecognized direction for player collision");
				break;
			}
			collided = true;
			timer = pickTime;
		}
	}

	public void startRespawn() {
		GetComponent<MoveCamera> ().stopMoving ();
		GetComponent<Camera> ().enabled = false;
		actionTaken ();
		if(!respawning) {
			respawning = true;
			gameObject.renderer.enabled = false;
			Invoke("respawn", 2f);
		}
	}

	public void respawn() {
		gameObject.renderer.enabled = true;
		GameObject[] points = GameObject.FindGameObjectsWithTag ("Decision");
		GameObject newLoc = points [Random.Range (0, points.Length - 1)];
		int dir = newLoc.GetComponent<DecisionV2> ().spawnDir;
		switch (dir) {
		case 0:
			gameObject.GetComponent<MoveCamera>().faceNorth();
			break;
		case 1:
			gameObject.GetComponent<MoveCamera>().faceEast();
			break;
		case 2:
			gameObject.GetComponent<MoveCamera>().faceSouth();
			break;
		case 3:
			gameObject.GetComponent<MoveCamera>().faceWest();
			break;
		}
		transform.position = newLoc.transform.position;
		GetComponent<Camera> ().enabled = true;

		respawning = false;
		
	}

	public void TakeAction() {
		//Forward, right, left, backward
		if (current.forward) {
			GetComponent<MoveCamera>().continueMoving();
		} else if(current.left) {
			GetComponent<MoveCamera>().turnLeft();
		} else if(current.right){
			GetComponent<MoveCamera>().turnRight();
		} else if(current.backward){
			GetComponent<MoveCamera>().turnAround();
		} else {
			Debug.Log("No movement option was available to player "+name);
		}
		actionTaken ();
	}
	
	void actionTaken(){
		collided = false;
	}

}
