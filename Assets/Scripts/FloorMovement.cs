using UnityEngine;
using System.Collections;

public class FloorMovement : MonoBehaviour {
	public float speed = 4f;
	public float upperBound = 10f;
	public float lowerBound = -10f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//Basic Movement
		Vector3 pos = transform.position; 
		pos.y += speed * Time.deltaTime; 
		transform.position = pos;
		
		//Changing Directions
		if ( pos.y < lowerBound ) 
		{ 
			speed = Mathf.Abs( speed); // Move up 
		} 
		else if ( pos.y > upperBound ) 
		{
			speed = -Mathf.Abs( speed); // Move down 
		}
	}
}
