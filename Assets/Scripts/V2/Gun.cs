using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

	public GameObject fireballPrefab;
	private Color parentColor;

	public float regularInterval;
	public float superInterval;
	public float timer = 0.0f;

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

		GameObject fireball = Instantiate(fireballPrefab) as GameObject;
		fireball.GetComponent<Fireball>().player = transform.parent.GetComponent<MovementV2>();
		
		//fireball.transform.position = transform.position+(transform.forward*(transform.localScale.x/2.0f+0.2f));
		//Vector3 pos = fireball.transform.position;
		//fireball.transform.position = pos;
		fireball.transform.position = transform.position;
		fireball.transform.position += transform.parent.forward * transform.localScale.y / 2;
		fireball.GetComponent<Fireball>().direction = transform.parent.transform.forward;
		fireball.GetComponent<Fireball>().color = parentColor;

		timer = regularInterval;
		return true;
	}

	public bool superShot() {

		timer = superInterval;
		return true;
	}
}
