using UnityEngine;
using System.Collections;

/* This class should handle the state and actions taken by powerup Objects */

public class PowerUpV2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		if(Physics.Raycast (transform.position, Vector3.down, out hit,  0.1f)){
			GetComponent<Rigidbody>().isKinematic = true;
		}
	}

	//Destroys the column, which should be the parent of the upgrade
	public void remove(){
		Destroy (transform.parent.gameObject);
	}
}
