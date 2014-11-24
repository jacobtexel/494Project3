using UnityEngine;
using System.Collections;

public class MovementV2 : MonoBehaviour {

	//Input strings for movement
	public string move;
	public string strafe;
	public string turn;
	public string commandA;
	public string commandB;
	public bool pointMan = false;
	public float dashTime = .2f;
	public float jumpTime = .5f;
	public float knockBackTime = .5f;
	public float timer;
	public int points = 0;
	public float moveMult = 3f;
	public float rotMult = 100f;
	public float slowRotMult = 40f;
	private float slowTimer = 0.0f;

	public GameObject fireballPrefab;
	public GameObject knifePrefab;

	public GameObject myKnife;

	//Bools concerning state of player
	private bool dash = false;
	private bool downDash = false;
	private bool recharge;
	public bool respawning;
	private bool knockedUp;
	private bool jump;
	public Vector3 startingSize = new Vector3 (.5f, .5f, .5f);
	public float minMove = 1f;
	public float minRot = 25f;
	public float maxSize = 2f;

	private Vector3 knockUpDirection;

	// Use this for initialization
	void Start () {
		recharge = false;
		respawning = false;
		jump = false;
		makeKnife ();
	}
	
	// Update is called once per frame
	void Update () {
		if(slowTimer > 0)
			slowTimer -= Time.deltaTime;
		//Evaluate player actions this frame
		//Dash action
		if (!respawning && !dash && !pointMan && Input.GetButton (commandB) && !recharge) {
			dash = true;
			downDash = false;
			timer = 0.0f;
		}
		//Jump action
		else if (!respawning && !jump && !pointMan && !knockedUp && Physics.Raycast (transform.position, Vector3.down, 0.25f) && Input.GetButtonDown (commandA)) {
			jump = true;
			downDash = false;
			timer = jumpTime;
		} 
		//Downdash action
		else if (!respawning && !downDash && !jump && !pointMan && !knockedUp && !Physics.Raycast (transform.position, Vector3.down, 0.25f) && Input.GetButtonDown (commandA)) {
			downDash = true;
			jump = false;
			if(dash){
				dash = false;
				recharge = true;
				Invoke("rechargeSkill", 1.5f);
			}
		}
		//Shooting action
		if(!respawning && pointMan && Input.GetButton (commandB)){
			if(transform.GetComponentInChildren<Gun>().regularShot()){
				slowTimer = .5f;
			}
		} else if(!respawning && pointMan && Input.GetButton (commandA)) {
			transform.GetComponentInChildren<Gun>().superShot();
		}
		//Regular action
		if(!respawning && !knockedUp && !downDash){
			//Rotate
			if(slowTimer > 0)
				transform.Rotate(slowRotMult * Vector3.up * Time.deltaTime*Input.GetAxis(turn));
			else
				transform.Rotate(rotMult * Vector3.up * Time.deltaTime*Input.GetAxis(turn));
			//Forward/backward motion
			Vector3 vel = rigidbody.velocity;
			vel.x = 0;
			vel.z = 0;
			vel += transform.forward * moveMult * Input.GetAxis(move);
			//Left/right strafe
			vel += transform.right * moveMult * Input.GetAxis(strafe);
			rigidbody.velocity = vel;
		}

		//Process position-alterring states
		if(dash){
			transform.position += transform.forward * 5 * Time.deltaTime;
			timer += Time.deltaTime;
			if(timer >= dashTime){
				dash = false;
				recharge = true;
				Invoke("rechargeSkill", 1.5f);
			}
		}
		if(jump){
			gameObject.rigidbody.velocity += Vector3.up*6.5f;
			jump = false;
		}
		if(knockedUp){
			jump = false;
			transform.position += 5 *knockUpDirection*Time.deltaTime;
			transform.position += 4	*Vector3.up*Time.deltaTime;
			timer -= Time.deltaTime;
			if(timer <= 0.0f){
				GetComponent<PlayerV2>().vignette.enabled = false;
				knockedUp = false;
			}
		}
		if (downDash) {
			transform.position += 4 * Vector3.down * Time.deltaTime;
			if(downDash && Physics.Raycast(transform.position, Vector3.down, 0.25f)){
				downDash = false;
			}
		}

	}

	void rechargeSkill() {
		recharge = false;
	}

	public void becomePointMan(){
		pointMan = true;
		timer = 0.0f;
		dash = false;
		downDash = false;
		moveMult = moveMult / 2f;
		GameObject.FindGameObjectWithTag ("Minimap").GetComponent<LevelManager> ().setHeavy (gameObject);
		if(knockedUp){
			knockedUp = false;
			GetComponent<PlayerV2>().vignette.enabled = false;
		}
		loseKnife ();
		transform.FindChild ("Gun").renderer.enabled = true;
	}

	public void losePointMan(){
		pointMan = false;
		transform.localScale = new Vector3 (1f, 1f, 1f);
		transform.FindChild ("Gun").renderer.enabled = false;
		getKnife ();
		rotMult = 100f;
		moveMult = 3f;
		this.transform.localScale = startingSize;
		startRespawn ();
	}

	public void GainPoint(){
		points++;
		if(points >= 10){
			PlayerPrefs.SetString("winner", GetComponent<PlayerV2>().playerNum.ToString());
			Application.LoadLevel("_End_screen");
		}
	}

	// Lol great method name
	public void GetKnockedUp(Vector3 source){
		if(pointMan)
			losePointMan ();
		knockUpDirection = transform.position - source;
		knockedUp = true;
		GetComponent<PlayerV2> ().vignette.enabled = true;
		timer = knockBackTime;
		jump = false;
	}

	void OnTriggerEnter(Collider col){
		if(col.tag == "Powerup"){
			becomePointMan();
			col.GetComponent<PowerUpV2>().remove();
			//col.GetComponent<PowerupAction>().startRespawn();
		}
		if(col.tag == "Danger"){
			if(pointMan) {
				losePointMan();
				GameObject.FindGameObjectWithTag("Minimap").GetComponent<LevelManager>().spawnPowerup();
			}
			startRespawn();
		}
	}

	void OnTriggerStay(Collider col) {
		OnTriggerEnter (col);
	}

	void OnCollisionEnter(Collision col){
		if(col.gameObject.tag == "MainCamera"){
			if(dash && col.gameObject.GetComponent<MovementV2>().pointMan){
				col.gameObject.GetComponent<MovementV2>().losePointMan();
				//col.gameObject.GetComponent<MovementV2>().GetKnockedUp(transform.position);
				becomePointMan();
			} else if(dash){
				col.gameObject.GetComponent<MovementV2>().GetKnockedUp(transform.position);
			}else if(col.gameObject.GetComponent<MovementV2>().pointMan) {
				GetKnockedUp(col.gameObject.transform.position);
			}
		}
	}

	public void startRespawn() {
		GetComponent<Camera>().enabled = false;
		gameObject.renderer.enabled = false;
		gameObject.collider.enabled = false;
		if(!respawning) {
			transform.position -= new Vector3(0, 100f, 0);
			dash = false;
			downDash = false;
			recharge = false;
			knockedUp = false;
			jump = false;
			respawning = true;
			Invoke("respawn", 2f);
		}
	}

	void respawn() {
		GameObject.FindGameObjectWithTag ("Minimap").GetComponent<LevelManager> ().respawnPlayer (gameObject);
		gameObject.renderer.enabled = true;
		gameObject.collider.enabled =true;
		GetComponent<Camera> ().enabled = true;
		respawning = false;
	}

	void makeKnife() {
		GameObject knife = Instantiate(knifePrefab) as GameObject;
		knife.transform.parent = transform;

		Vector3 v = displacementVector(.2f, .3f, transform.position.y-.1f, transform.position, Mathf.Deg2Rad*transform.eulerAngles.y+.587981f);

		knife.gameObject.renderer.material.color = gameObject.renderer.material.color;
		knife.transform.position = v;

		Vector3 rot = knife.transform.localRotation.eulerAngles;
		rot.y = 0;
		knife.transform.localRotation = Quaternion.Euler(rot);

		myKnife = knife;
	}

	void getKnife() {
		myKnife.renderer.enabled = true;
		GetComponent<PlayerV2> ().crosshairs.enabled = false;
	}
	void loseKnife() {
		myKnife.renderer.enabled = false;
		GetComponent<PlayerV2> ().crosshairs.enabled = true;
	}

	//uses parametric equations of a circle. x and z are the wanted values at an angle of 0
	Vector3 displacementVector(float x, float z, float height, Vector3 position, float angle){
		float radius = Mathf.Sqrt (Mathf.Pow (x, 2f) + Mathf.Pow (z, 2f));
		Vector3 ret = new Vector3 (0,0,0);
		ret.x = position.x + radius * Mathf.Sin (angle);
		ret.z = position.z + radius * Mathf.Cos (angle);
		ret.y = height;
		return ret;
	}
}
