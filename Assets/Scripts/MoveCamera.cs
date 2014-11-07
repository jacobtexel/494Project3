using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {
	public float moveSpeed = -.1f;
	public Vector3 speed;
	private Vector3 prevSpeed;
	private bool stopped;
	public int direction;
	public int moves = 50;
	//public GUIText movesText;
	public GUIText messageText;
	public bool restarting;

	//Rotate Camera
	public bool checkToCalculateLeft = false;
	public bool checkToCalculateRight = false;
	public bool checkToCalculateTurnAround = false;
	public bool turning = false;
	public float timeStart;
	public float timeDuration = .5f;
	public float startAngle = 0;
	public float endAngle = 0;
	private GameObject deadEnd;

	// Use this for initialization
	void Start () {
		restarting = false;
		direction = 0;
		stopped = false;
		prevSpeed = Vector3.zero;
		speed = new Vector3 (0, 0, moveSpeed);
		//movesText.text = "";
		messageText.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		/*
		// Most of this can go away once added to the application
		if(Input.GetButtonDown("stop")) {
			stopMoving();
		} else if(Input.GetButtonDown ("backward")) {
			turnAround();
		} else if(Input.GetButtonDown("left")) {
			turnLeft ();
		} else if(Input.GetButtonDown("right")) {
			turnRight ();
		} else if(Input.GetButtonDown("forward")) {
			continueMoving();
		}*/
		//movesText.text = "Moves Remaining: " + moves;
		if(!turning)
		{
			transform.position += speed;
		}

		if(checkToCalculateLeft)
		{
			checkToCalculateLeft = false;
			turning = true;
			timeStart = Time.time;
			startAngle = transform.localEulerAngles.y;
			endAngle = startAngle - 90;
		}
		if(checkToCalculateRight)
		{
			checkToCalculateRight = false;
			turning = true;
			timeStart = Time.time;
			startAngle = transform.localEulerAngles.y;
			endAngle = startAngle + 90;
		}
		if(checkToCalculateTurnAround)
		{
			checkToCalculateTurnAround = false;
			turning = true;
			timeStart = Time.time;
			startAngle = transform.localEulerAngles.y;
			endAngle = startAngle - 180;
		}
		if(turning)
		{
			float u = (Time.time - timeStart) / timeDuration;
			if(u>=1)
			{
				u = 1;
				turning = false;
			}
			Vector3 v = transform.localEulerAngles;
			v.y = (1-u)*startAngle + u*endAngle;

			transform.localEulerAngles = v;
		}
	}

	public void turnLeft() {
		stopped = false;
		checkToCalculateLeft = true;
		if(direction == 0) {
			direction = 3;
			//transform.localEulerAngles = new Vector3(0,-270f,0);
			speed = new Vector3(moveSpeed*-1f, 0, 0);
		} else if(direction == 1) {
			direction = 0;
			//transform.localEulerAngles = new Vector3(0,-180f,0);
			speed = new Vector3(0, 0, moveSpeed);
		} else if(direction == 2) {
			direction = 1;
			//transform.localEulerAngles = new Vector3(0,-90f,0);
			speed = new Vector3(moveSpeed, 0, 0);
		} else if(direction == 3) {
			direction = 2;
			//transform.localEulerAngles = new Vector3(0,0,0);
			speed = new Vector3(0, 0, moveSpeed*-1f);
		}
	}

	public void turnRight() {
		stopped = false;
		checkToCalculateRight = true;
		if(direction == 0) {
			direction = 1;
			//transform.localEulerAngles = new Vector3(0,-90f,0);
			speed = new Vector3(moveSpeed, 0, 0);
		} else if(direction == 1) {
			direction = 2;
			//transform.localEulerAngles = new Vector3(0,0,0);
			speed = new Vector3(0, 0, moveSpeed*-1f);
		} else if(direction == 2) {
			direction = 3;
			//transform.localEulerAngles = new Vector3(0,-270f,0);
			speed = new Vector3(moveSpeed*-1f, 0, 0);
		} else if(direction == 3) {
			direction = 0;
			//transform.localEulerAngles = new Vector3(0,-180f,0);
			speed = new Vector3(0, 0, moveSpeed);
		}
	}

	public void turnAround() {
		stopped = false;
		checkToCalculateTurnAround = true; 
		if(direction == 0) {
			direction = 2;
			//transform.localEulerAngles = new Vector3(0,0,0);
			speed = new Vector3(0, 0, moveSpeed*-1f);
		} else if(direction == 1) {
			direction = 3;
			//transform.localEulerAngles = new Vector3(0,90f,0);
			speed = new Vector3(moveSpeed*-1f, 0, 0);
		} else if(direction == 2) {
			direction = 0;
			//transform.localEulerAngles = new Vector3(0,-180f,0);
			speed = new Vector3(0, 0, moveSpeed);
		} else if(direction == 3) {
			print ("Turning around");
			direction = 1;
			print("Direction is now " + direction);
			//transform.localEulerAngles = new Vector3(0,270f,0);
			speed = new Vector3(moveSpeed, 0, 0);
		}

	}

	public void continueMoving() {
		if(stopped) {
			speed = prevSpeed;
			stopped = false;
		}
	}

	public void stopMoving() {
		if(!stopped) {
			prevSpeed = speed;
			speed = Vector3.zero;
			stopped = true;
		}	
	}

	void OnTriggerEnter(Collider col) {
		if(col.tag == "Decision") {
			stopMoving ();
			transform.position = col.gameObject.transform.position;
			if(moves <= 0 && !restarting) {
				messageText.text = "You Lose... :(";
				restarting = true;
				Invoke("restart", 2);
			}
		}
		else if(col.tag == "DeadEnd" && !turning) {
			turnAround();
			deadEnd = col.gameObject;
		}
		else if(col.tag == "VictoryPoint" && moves > 0) {
			stopMoving();
			messageText.text = "You Win!";
			Invoke("restart", 3);
		} else if(col.tag == "MainCamera") {
			stopMoving ();
			if(name == "Player1Cam")
				messageText.text = "You Win!";
			else
				messageText.text = "You Lose... :(";
			Invoke("restart", 3);
		}
	}

	void restart() {
		Application.LoadLevel (0);
		restarting = false;
	}


}
