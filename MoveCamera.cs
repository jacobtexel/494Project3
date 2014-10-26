using UnityEngine;
using System.Collections;

public class MoveCamera : MonoBehaviour {
	public float moveSpeed = .1f;
	private Vector3 speed;
	private Vector3 prevSpeed;
	private bool stopped;
	private int direction;


	// Use this for initialization
	void Start () {
		direction = 0;
		stopped = false;
		prevSpeed = Vector3.zero;
		speed = new Vector3 (0, 0, moveSpeed);	
	}
	
	// Update is called once per frame
	void Update () {
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
		}

		transform.position += speed;
	}

	void turnLeft() {
		stopped = false;
		if(direction == 0) {
			direction = 3;
			transform.localEulerAngles = new Vector3(0,270f,0);
			speed = new Vector3(moveSpeed*-1f, 0, 0);
		} else if(direction == 1) {
			direction = 0;
			transform.localEulerAngles = new Vector3(0,0,0);
			speed = new Vector3(0, 0, moveSpeed);
		} else if(direction == 2) {
			direction = 1;
			transform.localEulerAngles = new Vector3(0,90f,0);
			speed = new Vector3(moveSpeed, 0, 0);
		} else if(direction == 3) {
			direction = 2;
			transform.localEulerAngles = new Vector3(0,180f,0);
			speed = new Vector3(0, 0, moveSpeed*-1f);
		}
	}

	void turnRight() {
		stopped = false;
		if(direction == 0) {
			direction = 1;
			transform.localEulerAngles = new Vector3(0,90f,0);
			speed = new Vector3(moveSpeed, 0, 0);
		} else if(direction == 1) {
			direction = 2;
			transform.localEulerAngles = new Vector3(0,180f,0);
			speed = new Vector3(0, 0, moveSpeed*-1f);
		} else if(direction == 2) {
			direction = 3;
			transform.localEulerAngles = new Vector3(0,270f,0);
			speed = new Vector3(moveSpeed*-1f, 0, 0);
		} else if(direction == 3) {
			direction = 0;
			transform.localEulerAngles = new Vector3(0,0,0);
			speed = new Vector3(0, 0, moveSpeed);
		}
	}

	void turnAround() {
		stopped = false;
		if(direction == 0) {
			direction = 2;
			transform.localEulerAngles = new Vector3(0,180f,0);
			speed = new Vector3(0, 0, moveSpeed*-1f);
		} else if(direction == 1) {
			direction = 3;
			transform.localEulerAngles = new Vector3(0,270f,0);
			speed = new Vector3(moveSpeed*-1, 0, 0);
		} else if(direction == 2) {
			direction = 0;
			transform.localEulerAngles = new Vector3(0,0,0);
			speed = new Vector3(0, 0, moveSpeed);
		} else if(direction == 3) {
			direction = 1;
			transform.localEulerAngles = new Vector3(0,90f,0);
			speed = new Vector3(moveSpeed, 0, 0);
		}

	}

	void continueMoving() {
		if(stopped) {
			speed = prevSpeed;
			stopped = false;
		}
	}

	void stopMoving() {
		print(stopped);
		if(!stopped) {
			prevSpeed = speed;
			speed = Vector3.zero;
			stopped = true;
		}	
	}

	void OnTriggerEnter(Collider col) {
		//Figure out wall collisions here

	}


}
