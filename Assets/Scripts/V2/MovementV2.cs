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

	public GameObject fireballPrefab;
	public GameObject knifePrefab;
	
	public GameObject myKnife;

	//Bools concerning state of player
	private bool dash = false;
	private bool downDash = false;
	private bool recharge;
	private bool respawning;
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
		getKnife ();
	}
	
	// Update is called once per frame
	void Update () {

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
		//Fireball action
		else if(!respawning && pointMan && !recharge && Input.GetButton (commandB)){
			GameObject fireball = Instantiate(fireballPrefab) as GameObject;
			fireball.GetComponent<Fireball>().player = GetComponent<MovementV2>();

			fireball.transform.position = transform.position+(transform.forward*(transform.localScale.x/2.0f+0.2f));
			Vector3 pos = fireball.transform.position;
			pos.y = .5f;
			fireball.transform.position = pos;
			fireball.GetComponent<Fireball>().direction = transform.forward;
			fireball.GetComponent<Fireball>().color = renderer.material.color;

			recharge = true;
			Invoke("rechargeSkill", 0.5f);
		}
		//Regular action
		else if(!respawning && !knockedUp && !downDash){
			//Rotate
			transform.Rotate(rotMult * Vector3.up * Time.deltaTime*Input.GetAxis(turn));
			//Forward/backward motion
			Vector3 vel = rigidbody.velocity;
			vel.x = 0;
			vel.z = 0;
			vel += transform.forward * moveMult * Input.GetAxis(move);
			//Left/right strafe
			vel += transform.right * moveMult * Input.GetAxis(strafe);
			rigidbody.velocity = vel;
			//transform.position += transform.forward * moveMult * Time.deltaTime * Input.GetAxis(move);
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
		jump = false;
		downDash = false;
		moveMult = moveMult / 3f;
		if(knockedUp){
			knockedUp = false;
			GetComponent<PlayerV2>().vignette.enabled = false;
		}
		loseKnife ();
	}

	public void losePointMan(){
		pointMan = false;
		transform.localScale = new Vector3 (1f, 1f, 1f);
		getKnife ();
		resetSize ();
	}

	public void GainPoint(){
		points++;
		if(points >= 60){
			PlayerPrefs.SetString("winner", GetComponent<PlayerV2>().playerNum.ToString());
			Application.LoadLevel("_End_screen");
		}
	}

	// Lol great method name
	public void GetKnockedUp(Vector3 source){
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
		} else if(col.tag == "Danger"){
			if(pointMan) {
				losePointMan();
				GameObject.FindGameObjectWithTag("Minimap").GetComponent<LevelManager>().spawnPowerup();
			}
			startRespawn();
		}
	}

	void OnCollisionEnter(Collision col){
		if(col.gameObject.tag == "MainCamera"){
			if(dash && col.gameObject.GetComponent<MovementV2>().pointMan){
				col.gameObject.GetComponent<MovementV2>().losePointMan();
				col.gameObject.GetComponent<MovementV2>().GetKnockedUp(transform.position);
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
			losePointMan();
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
		GameObject[] spawns = GameObject.FindGameObjectsWithTag ("Spawn");
		gameObject.renderer.enabled = true;

		GameObject spot = spawns [Random.Range (0, spawns.Length)];
		transform.position = spot.transform.position;
		gameObject.renderer.enabled = true;
		gameObject.collider.enabled =true;
		GetComponent<Camera> ().enabled = true;
		respawning = false;
	}

	void resetSize() {
		rotMult = 100f;
		moveMult = 3f;
		this.transform.localScale = startingSize;
	}

	void getKnife() {
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
	
	void loseKnife() {
		
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
