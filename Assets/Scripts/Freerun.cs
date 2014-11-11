using UnityEngine;
using System.Collections;

public class Freerun : MonoBehaviour {
	//Input strings for movement
	public string forward;
	public string left;
	public string right;
	public string backward;


	// movement things
	private bool moveForward;
	private bool moveBackward;
	private bool turnLeft;
	private bool turnRight;

	// Use this for initialization
	void Start () {
		moveForward = false;
		moveBackward = false;
		turnLeft = false;
		turnRight = false;
	}
	
	// Update is called once per frame
	void Update () {
		//Forward and backward key things
		if(Input.GetButtonDown(forward) && ! moveBackward)
		{
			moveForward = true;
		}
		else if (Input.GetButtonDown(backward) && !moveForward) 
		{
			moveBackward = true;
		}
		else if (Input.GetButtonUp(forward))
		{
			moveForward = false;
		}
		else if (Input.GetButtonUp(backward))
		{
			moveBackward = false;
		}

		// Rotation key things
		if(Input.GetButtonDown(left) && !turnRight)
		{
			turnLeft = true;
		}
		else if (Input.GetButtonDown(right) && !turnLeft) 
		{
			turnRight = true;
		}
		else if (Input.GetButtonUp(left))
		{
			turnLeft = false;
		}
		else if (Input.GetButtonUp(right))
		{
			turnRight = false;
		}

		// DO the movement
		Vector3 camEuler = transform.rotation.eulerAngles;
		camEuler.x = 0f;
		Quaternion normal = Quaternion.Euler (camEuler);
		if(moveForward) 
		{
			transform.position += normal * Vector3.forward;
		} 
		else if(moveBackward)
		{
			transform.position -= normal * Vector3.forward;
		}

		// Add some rotation in
		if(turnLeft)
		{
			Vector3 rotation = transform.localEulerAngles;
			rotation.y -= 1f;
			transform.localEulerAngles = rotation;
		}
		else if (turnRight)
		{
			Vector3 rotation = transform.localEulerAngles;
			rotation.y += 1f;
			transform.localEulerAngles = rotation;
		}
	}

	void OnTriggerEnter(Collider col)
	{
		print ("A");
	}
}
