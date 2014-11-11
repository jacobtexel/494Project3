using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {

	public Vector3 direction;
	public Color color;
	public float speed = 5.0f;

	// Use this for initialization
	void Start () {
		//Ensure direction vector is a unit vector
		direction = direction / direction.magnitude;
		GetComponent<ParticleSystem> ().startColor = color;
		renderer.material.color = color;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += direction * speed * Time.deltaTime;
	}

	void OnCollisionEnter(Collision col){
		if (col.gameObject.tag == "MainCamera") {
			col.gameObject.GetComponent<MovementV2>().GetKnockedUp(transform.position);
		}
		print ("called");
		Destroy (this.gameObject);
	}
}
