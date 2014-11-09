using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {
	public float moveSpeed = -.1f;
	public Vector3 speed;
	public float speedMult = 1f;
	private bool stopped;
	public int direction;
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

	// Use this for initialization
	void Start () {
		restarting = false;
		direction = 0;
		stopped = false;
		speed = new Vector3 (0, 0, moveSpeed);
		messageText.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		if(!turning)
		{
			transform.position += speed * speedMult;
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
		speedMult = 1f;
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
		speedMult = 1f;
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
		speedMult = 1;
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
			direction = 1;
			//transform.localEulerAngles = new Vector3(0,270f,0);
			speed = new Vector3(moveSpeed, 0, 0);
		}

	}

	public void continueMoving() {
		if(stopped) {
			speedMult = 1;
			stopped = false;
		}
	}

	public void stopMoving() {
		if(!stopped) {
			speedMult = 0;
			stopped = true;
		}	
	}

	void OnTriggerEnter(Collider col) {
		if(col.tag == "Decision" && !turning) {
			stopMoving ();
			transform.position = col.transform.position;
		}
	}

}
