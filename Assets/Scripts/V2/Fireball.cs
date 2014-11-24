using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {

	public MovementV2 player;
	public Vector3 direction;
	public Color color;
	public float speed = 15.0f;
	public float lifeTimer = 5.0f;

	// Use this for initialization
	void Start () {
		//Ensure direction vector is a unit vector
		direction = direction / direction.magnitude;
		GetComponent<ParticleSystem> ().startColor = color;
		renderer.material.color = color;
	}
	
	// Update is called once per frame
	void Update () {
		lifeTimer -= Time.deltaTime;
		if (lifeTimer < 0)
			Destroy (this.gameObject);
		transform.position += direction * speed * Time.deltaTime;
	}

	void OnCollisionEnter(Collision col){
		print (col.gameObject.name);
		if(col.gameObject.tag == "Fireball")
			return;

		if (col.gameObject.tag == "MainCamera" && Time.time - col.gameObject.GetComponent<MovementV2>().lastRespawn > col.gameObject.GetComponent<MovementV2>().invincibilityPeriod) {
			Debug.Log (Time.time);
			Debug.Log (col.gameObject.GetComponent<MovementV2> ().lastRespawn);
			Debug.Log(col.gameObject.GetComponent<MovementV2>().invincibilityPeriod);
			col.gameObject.GetComponent<MovementV2>().startRespawn();

			player.GainPoint();
		}
		print ("called");
		Destroy (this.gameObject);
	}
}
