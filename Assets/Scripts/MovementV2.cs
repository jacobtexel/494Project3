using UnityEngine;
using System.Collections;

public class MovementV2 : MonoBehaviour {

	//Input strings for movement
	public string move;
	public string strafe;
	public string turn;
	public string commandA;
	public string commandB;
	public string commandV;

	//Timer variables
	private float dashTime = .5f;
	private float knockBackTime = .5f;
	private float slowTimer = 0.0f;
	private float timer;
	private float timeBetweenVoices = .5f;
	private float lastVoiceMessage = -10f;


	//Multipliers for movement
	private float moveMult = 3f;
	private float dashMult = 8f;
	private	float rotMult = 100f;
	private float slowRotMult = 40f;

	//Gameobjects
	public GameObject fireballPrefab;
	public GameObject knifePrefab;
	public GameObject myKnife;

	//Bools concerning state of player
	public bool pointMan = false;
	private bool dash = false;
	private bool downDash = false;
	private bool recharge;
	public  bool respawning;
	public bool knockedUp;
	public int points = 0;
	private Vector3 knockUpDirection;



	public float lastRespawn = -10f;
	public float invincibilityPeriod = 2f;
	public float reloadBonusMultiplier = 0.9f;


	//elevator properties
	public bool onElevator = false;
	public GameObject elevator;

	//Voice Messages
	private AudioClip[] voices;

	// Use this for initialization
	void Start () {
		recharge = false;
		respawning = false;

		GameObject knife = Instantiate(knifePrefab) as GameObject;
		knife.transform.parent = transform;
		
		Vector3 v = displacementVector(.2f, .3f, transform.position.y-.1f, transform.position, Mathf.Deg2Rad*transform.eulerAngles.y+.587981f);
		
		knife.gameObject.renderer.material.color = gameObject.renderer.material.color;
		knife.transform.position = v;
		
		Vector3 rot = knife.transform.localRotation.eulerAngles;
		rot.y = 0;
		knife.transform.localRotation = Quaternion.Euler(rot);
		
		myKnife = knife;

		voices = new AudioClip[]{(AudioClip)Resources.Load ("Sound/Cookie1"),
								(AudioClip)Resources.Load ("Sound/Cookie2"),
								(AudioClip)Resources.Load ("Sound/Cookie3"),
								(AudioClip)Resources.Load ("Sound/Fart1"),
								(AudioClip)Resources.Load ("Sound/Fart2"),
								(AudioClip)Resources.Load ("Sound/Fart3"),
								(AudioClip)Resources.Load ("Sound/Halo1"),
								(AudioClip)Resources.Load ("Sound/Halo2"),
								(AudioClip)Resources.Load ("Sound/Halo3"),
								(AudioClip)Resources.Load ("Sound/Haveyou1"),
								(AudioClip)Resources.Load ("Sound/Haveyoukiddo1"),
								(AudioClip)Resources.Load ("Sound/HeyBabayyy"),
								(AudioClip)Resources.Load ("Sound/Highground1"),
								(AudioClip)Resources.Load ("Sound/Highground2"),
								(AudioClip)Resources.Load ("Sound/NiceOn"),
								(AudioClip)Resources.Load ("Sound/NiceOn2"),
								(AudioClip)Resources.Load ("Sound/Smash1"),
								(AudioClip)Resources.Load ("Sound/Smash2"),
								(AudioClip)Resources.Load ("Sound/Smash3"),
								(AudioClip)Resources.Load ("Sound/Smash4"),
								(AudioClip)Resources.Load ("Sound/SpicyTaco")};
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown(commandV) && Time.time - lastVoiceMessage > timeBetweenVoices)
		{
			lastVoiceMessage = Time.time;
			audio.PlayOneShot(voices[Random.Range (0, voices.Length)]);
		}
		if(slowTimer > 0)
			slowTimer -= Time.deltaTime;
		//Evaluate player actions this frame
		//Dash action
		if (!respawning && !dash && !pointMan && Input.GetButton (commandB) && !recharge) {
			GetComponent<PlayerV2>().playDash();
			dash = true;
			timer = 0.0f;
		}
		//Jump action
		if (Input.GetButtonDown (commandA) && canJump()) {
			jumpAction();
		} 
		//Downdash action
		else if (Input.GetButtonDown (commandA) && !respawning && !pointMan && !knockedUp && inAir()) {
			downDash = true;
			Vector3 vel = rigidbody.velocity;
			vel.y = -dashMult;
			rigidbody.velocity = vel;
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
		if(!dash && !respawning && !knockedUp){
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
			transform.Rotate(rotMult * Vector3.up * Time.deltaTime*Input.GetAxis(turn));
			//Forward/backward motion
			Vector3 vel = rigidbody.velocity;
			vel.x = 0;
			vel.z = 0;
			vel += transform.forward * dashMult;
			//Left/right strafe
			vel += transform.right * moveMult * Input.GetAxis(strafe);
			rigidbody.velocity = vel;

			timer += Time.deltaTime;
			if(timer >= dashTime){
				dash = false;
				recharge = true;
				Invoke("rechargeSkill", 1.5f);
			}
		}
		if(knockedUp){
			transform.position += 5 *knockUpDirection*Time.deltaTime;
			transform.position += 4	*Vector3.up*Time.deltaTime;
			timer -= Time.deltaTime;
			if(timer <= 0.0f){
				GetComponent<PlayerV2>().vignette.enabled = false;
				knockedUp = false;
			}
		}
		if (downDash && !inAir ()) {
			downDash = false;
		}
		
		if(onElevator)
		{
			Vector3 pos = transform.position; 
			pos.y += elevator.GetComponent<FloorMovement>().speed * Time.deltaTime; 
			transform.position = pos;
		}
	}

	bool canJump(){
		if (!respawning && !pointMan && !knockedUp) {
			return !inAir() || onElevator;
		}
		return false;
	}

	//Returns true if it appears the player is in the air
	bool inAir(){
		Vector3 raycastOrigin = transform.position;
		RaycastHit hit;
		if(Physics.Raycast (raycastOrigin, Vector3.down, out hit, 0.25f) && hit.collider.tag!="MainCamera")
			return false;
		else if(Physics.Raycast (raycastOrigin + Vector3.left*.2f, Vector3.down, out hit, 0.25f) && hit.collider.tag!="MainCamera")
			return false;
		else if(Physics.Raycast (raycastOrigin + Vector3.right*.2f, Vector3.down, out hit, 0.25f) && hit.collider.tag!="MainCamera")
			return false;
		else if(Physics.Raycast (raycastOrigin + Vector3.forward*.2f, Vector3.down, out hit, 0.25f) && hit.collider.tag!="MainCamera")
			return false;
		else if(Physics.Raycast (raycastOrigin + Vector3.back*.2f, Vector3.down, out hit, 0.25f) && hit.collider.tag!="MainCamera")
			return false;
		return true;
	}
	
	void jumpAction(){
		downDash = false;
		GetComponent<PlayerV2> ().playJump ();
		gameObject.rigidbody.velocity += Vector3.up*6.5f;
	}


	void rechargeSkill() {
		recharge = false;
	}

	public void becomePointMan(){
		if(!pointMan)
		{
			pointMan = true;
			moveMult = moveMult / 2f;
		}
		else
		{
			transform.GetComponentInChildren<Gun>().multiplier *= reloadBonusMultiplier;
		}
		timer = 0.0f;
		dash = false;
		downDash = false;
		GameObject.FindGameObjectWithTag ("Minimap").GetComponent<LevelManager> ().setHeavy (gameObject);
		if(knockedUp){
			knockedUp = false;
			GetComponent<PlayerV2>().vignette.enabled = false;
		}
		loseKnife ();
		transform.FindChild ("Gun").renderer.enabled = true;
		Component halo = GetComponent("Halo");
		halo.GetType().GetProperty("enabled").SetValue(halo, true, null);
	}

	public void losePointMan(){
		pointMan = false;
		transform.FindChild ("Gun").renderer.enabled = false;
		transform.GetComponentInChildren<Gun> ().multiplier = 1;
		getKnife ();
		rotMult = 100f;
		moveMult = 3f;
		Component halo = GetComponent("Halo");
		halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
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
			} else {
				startRespawn();
			}
		}
	}

	void OnTriggerStay(Collider col) {
		OnTriggerEnter (col);
	}

	void OnCollisionEnter(Collision col){
		if(col.gameObject.tag == "MainCamera" && Time.time - col.gameObject.GetComponent<MovementV2>().lastRespawn > col.gameObject.GetComponent<MovementV2>().invincibilityPeriod){
			if((dash || downDash) && col.gameObject.GetComponent<MovementV2>().pointMan){
				col.gameObject.GetComponent<MovementV2>().losePointMan();
				col.gameObject.GetComponent<PlayerV2>().playHit();
				GainPoint();
				becomePointMan();
			} else if(dash || downDash){
				col.gameObject.GetComponent<MovementV2>().GetKnockedUp(transform.position);
				col.gameObject.GetComponent<PlayerV2>().playHit();
			}else if(col.gameObject.GetComponent<MovementV2>().pointMan) {
				GetKnockedUp(col.gameObject.transform.position);
				GetComponent<PlayerV2>().playHit();
			}
		}
		else if(col.gameObject.tag == "Elevator")
		{
			onElevator = true;
			elevator = col.gameObject;
		}
	}

	void OnCollisionExit(Collision col){
		if(col.gameObject.tag == "Elevator")
		{
			onElevator = false;
			elevator = null;
		}
	}

	public void startRespawn() {
		gameObject.renderer.enabled = false;
		if(!respawning) {
			transform.position = GameObject.FindGameObjectWithTag("HoldingZone").transform.position;
			myKnife.renderer.enabled = false;

			dash = false;
			downDash = false;
			recharge = false;
			knockedUp = false;
			respawning = true;

			Invoke("respawn", 2f);
		}
	}

	void respawn() {
		GameObject.FindGameObjectWithTag ("Minimap").GetComponent<LevelManager> ().respawnPlayer (gameObject);
		myKnife.renderer.enabled = true;
		gameObject.renderer.enabled = true;
		gameObject.collider.enabled =true;
		GetComponent<Camera> ().enabled = true;
		respawning = false;
		lastRespawn = Time.time;
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
