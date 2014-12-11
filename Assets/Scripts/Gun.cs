using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

	public GameObject fireballPrefab;
	private Color parentColor;
	private float timer = 0.0f;

	public float regularInterval;
	public float superInterval;
	public float superSpread = 0.4f;
	public int superShotCount =20;

	public AudioClip superSound;
	public AudioClip normalSound;

	// Use this for initialization
	void Start () {
		parentColor = transform.parent.renderer.material.color;
	}
	
	// Update is called once per frame
	void Update () {
		if(timer > 0) timer -= Time.deltaTime;
	}

	//Returns true if a shot is fired
	public bool regularShot() {
		if(timer > 0) return false;
		audio.PlayOneShot (normalSound);
		GameObject fireball = Instantiate(fireballPrefab) as GameObject;
		fireball.GetComponent<Fireball>().player = transform.parent.GetComponent<MovementV2>();

		fireball.transform.position = transform.position;
		fireball.transform.position += transform.parent.forward * transform.localScale.y / 2;
		fireball.GetComponent<Fireball>().direction = transform.parent.transform.forward;
		fireball.GetComponent<Fireball>().color = parentColor;


		timer = regularInterval;
		return true;
	}

	//Fire 20 shots
	public bool superShot() {
		if(timer > 0) return false;
		audio.PlayOneShot (superShot);
		for(int x=0; x<superShotCount; x++){
			GameObject fireball = Instantiate(fireballPrefab) as GameObject;
			fireball.GetComponent<Fireball>().player = transform.parent.GetComponent<MovementV2>();


			fireball.transform.position = transform.position;
			fireball.transform.position += transform.parent.forward * transform.localScale.y / 2;
			fireball.GetComponent<Fireball>().direction = transform.parent.forward + Random.insideUnitSphere * superSpread;
			fireball.GetComponent<Fireball>().color = parentColor;
			//fireball.GetComponent<ParticleSystem>().enableEmission = false;
		}
		timer = superInterval;
		return true;
	}
}
