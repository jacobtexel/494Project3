using UnityEngine;
using System.Collections;

/* This class should handle the state and actions taken by powerup Objects */

public class PowerUpV2 : MonoBehaviour {

	private float startHeight;
	private bool bobUp;
	public float speed = 20f;

	// Use this for initialization
	void Start () {
		transform.position = transform.parent.transform.position;
		startHeight = transform.position.y;
		bobUp = false;
	}
	
	// Update is called once per frame
	void Update () {
		//Lets make it bob
		makeBob ();

		// and rotate!
		transform.Rotate (Vector3.up * speed * Time.deltaTime);
	}

	//Destroys the column, which should be the parent of the upgrade
	public void remove(){
		Destroy (transform.parent.gameObject);
	}

	void makeBob() {
		if(bobUp) {
			//transform.position += new Vector3(0,Random.Range(.001f,.02f),0);
			transform.position += new Vector3(0,.01f,0);
			if(transform.position.y >= startHeight + .1f)
				bobUp = false;
		
		} else { 
			//transform.position -= new Vector3(0,Random.Range(.001f,.01f),0);
			transform.position -= new Vector3(0,.01f,0);
			if(transform.position.y <= startHeight - .1f)
				bobUp = true;
		}
	}

	void makeRotate() {
		
	}
}
