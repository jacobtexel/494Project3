using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {
	public float moveSpeed = -.1f;
	public Vector3 speed;
	public float speedMult = 1f;
	public int direction;
	public GUIText messageText;
	public bool restarting;

	//Rotate Camera
	public bool turning = false;
	public float timeStart;
	public float timeDuration = .5f;
	public float startAngle = 0;
	public float endAngle = 0;

	// Use this for initialization
	void Start () {
		restarting = false;
		direction = 0;
		speed = new Vector3 (0, 0, moveSpeed);
		messageText.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		if(turning)
		{
			if(endAngle - startAngle > 180)
				endAngle -= 360;
			float u = (Time.time - timeStart) / timeDuration;
			if(u>=1)
			{
				u = 1;
				turning = false;
			}
			Vector3 v = transform.localEulerAngles;
			v.y = (1-u)*startAngle + u*endAngle;

			transform.localEulerAngles = v;
		} else {
			transform.position += speed * speedMult;
		}
	}

	public void turnLeft() {
		speedMult = 1f;
		if(direction == 0) {
			faceWest();
		} else if(direction == 1) {
			faceNorth();
		} else if(direction == 2) {
			faceEast();
		} else if(direction == 3) {
			faceSouth();
		}
	}

	public void turnRight() {
		speedMult = 1f;
		if(direction == 0) {
			faceEast();
		} else if(direction == 1) {
			faceSouth();
		} else if(direction == 2) {
			faceWest();
		} else if(direction == 3) {
			faceNorth();
		}
	}

	public void turnAround() {
		speedMult = 1;
		if(direction == 0) {
			faceSouth();
		} else if(direction == 1) {
			faceWest();
		} else if(direction == 2) {
			faceNorth();
		} else if(direction == 3) {
			faceEast();
		}

	}

	public void continueMoving() {
		if(speedMult == 0) {
			speedMult = 1;
		}
	}

	public void stopMoving() {
		if(speedMult == 1) {
			speedMult = 0;
		}	
	}

	void OnTriggerEnter(Collider col) {
		if(col.tag == "Decision" && !turning) {
			stopMoving ();
		}
	}

	public void faceEast() {
		speed = new Vector3(moveSpeed, 0, 0);
		direction = 1;
		endAngle = 270f;
		startAngle = transform.localEulerAngles.y;
		timeStart = Time.time;
		turning = true;
	}

	public void faceWest() {
		speed = new Vector3(moveSpeed*-1f, 0, 0);
		direction = 3;
		endAngle = 90f;
		startAngle = transform.localEulerAngles.y;
		timeStart = Time.time;
		turning = true;
	}

	public void faceNorth() {
		speed = new Vector3(0, 0, moveSpeed);
		direction = 0;
		endAngle = 180f;
		startAngle = transform.localEulerAngles.y;
		timeStart = Time.time;
		turning = true;
	}

	public void faceSouth() {
		speed = new Vector3(0, 0, moveSpeed*-1f);
		direction = 2;
		endAngle = 0f;
		startAngle = transform.localEulerAngles.y;
		timeStart = Time.time;
		turning = true;
	}

}
