using UnityEngine;
using System.Collections;

public class MovementV2 : MonoBehaviour {

	//Input strings for movement
	public string forward;
	public string left;
	public string right;
	public string backward;
	private bool dash = false;
	public bool pointMan = false;
	public float dashTime = .2f;
	public float timer;
	public int points = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!dash && !pointMan && Input.GetButton (left) && Input.GetButton (right)) {
			dash = true;
			timer = 0.0f;
			gameObject.layer = 0;
		}else{
			if(Input.GetButton(left)){
				transform.Rotate(100 * Vector3.down * Time.deltaTime);
			}
			if(Input.GetButton (right)) {
				transform.Rotate(100 * Vector3.up * Time.deltaTime);
			}
			if(Input.GetButton (forward)){
				transform.position += transform.forward * 3 * Time.deltaTime;
			}
			if(Input.GetButton (backward)){
				transform.position -= transform.forward * 3 * Time.deltaTime;
			}
		}
		if(dash){
			transform.position += transform.forward * 5 * Time.deltaTime;
			timer += Time.deltaTime;
			if(timer >= dashTime){
				dash = false;
				gameObject.layer = 12; //Layer not rendered by any player or minimap
			}
		}
	}

	public void becomePointMan(){
		print ("Called by "+name);
		pointMan = true;
		gameObject.layer = 0;
		timer = 0.0f;
		dash = false;
		InvokeRepeating("GainPoint", 1.0f, 1.0f);
		print (gameObject.layer);
	}

	public void losePointMan(){
		print ("Point lost by " + name);
		pointMan = false;
		CancelInvoke ("GainPoint");
		gameObject.layer = 12;
	}

	void GainPoint(){
		points++;
	}


	void OnTriggerEnter(Collider col){
		if(col.tag == "Powerup"){
			becomePointMan();
			Destroy(col.gameObject);
			//col.GetComponent<PowerupAction>().startRespawn();
		} 
	}

	void OnCollisionEnter(Collision col){
		if(col.gameObject.tag == "MainCamera"){
			if(dash && col.gameObject.GetComponent<MovementV2>().pointMan){
				becomePointMan();
				col.gameObject.GetComponent<MovementV2>().losePointMan();
			}
		}
	}
}
