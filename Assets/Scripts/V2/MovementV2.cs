using UnityEngine;
using System.Collections;

public class MovementV2 : MonoBehaviour {

	//Input strings for movement
	public string move;
	public string turn;
	public string commandA;
	public string commandB;
	public bool pointMan = false;
	public float dashTime = .2f;
	public float jumpTime = .5f;
	public float knockBackTime = .5f;
	public float timer;
	public int points = 0;

	public GameObject fireballPrefab;

	//Bools concerning state of player
	private bool dash = false;
	private bool downDash = false;
	private bool recharge;
	private bool respawning;
	private bool knockedUp;
	private bool jump;
	private bool up;

	private Vector3 knockUpDirection;

	// Use this for initialization
	void Start () {
		recharge = false;
		respawning = false;
		jump = false;
	}
	
	// Update is called once per frame
	void Update () {

		//Evaluate player actions this frame
		//Dash action
		if (!respawning && !dash && !pointMan && Input.GetButton (commandB) && !recharge) {
			dash = true;
			downDash = false;
			timer = 0.0f;
			gameObject.layer = 12+GetComponent<PlayerV2>().playerNum;
		}
		//Jump action
		else if (!respawning && !jump && !pointMan && !knockedUp && Physics.Raycast (transform.position, Vector3.down, 0.25f) && Input.GetButtonDown (commandA)) {
			jump = true;
			downDash = false;
			timer = jumpTime;
			gameObject.layer = 12+GetComponent<PlayerV2>().playerNum;
		} 
		//Downdash action
		else if (!respawning && !downDash && !jump && !pointMan && !knockedUp && !Physics.Raycast (transform.position, Vector3.down, 0.25f) && Input.GetButtonDown (commandA)) {
			downDash = true;
			gameObject.layer = 12+GetComponent<PlayerV2>().playerNum;
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
			fireball.transform.position = transform.position+transform.forward;
			fireball.GetComponent<Fireball>().direction = transform.forward;
			fireball.GetComponent<Fireball>().color = renderer.material.color;
			recharge = true;
			Invoke("rechargeSkill", 0.5f);
		}
		//Regular action
		else if(!respawning && !knockedUp && !downDash){
			transform.Rotate(100 * Vector3.up * Time.deltaTime*Input.GetAxis(turn));
			transform.position += transform.forward * 3 * Time.deltaTime * Input.GetAxis(move);
		}

		//Process position-alterring states
		if(dash){
			transform.position += transform.forward * 5 * Time.deltaTime;
			timer += Time.deltaTime;
			if(timer >= dashTime){
				dash = false;
				gameObject.layer = 12; //Layer not rendered by any player or minimap
				recharge = true;
				Invoke("rechargeSkill", 1.5f);
			}
		}
		if(jump){
			transform.position += 4	*Vector3.up*Time.deltaTime;
			timer -= Time.deltaTime;
			if(timer <= 0.0f){
				jump = false;
				gameObject.layer = 12;
			}
		}
		if(knockedUp){
			jump = false;
			if(up){
				transform.position += 5 *knockUpDirection*Time.deltaTime;
				transform.position += 4	*Vector3.up*Time.deltaTime;
				timer -= Time.deltaTime;
				if(timer <= 0.0f)
					up=false;
 			}else{
				transform.position += 3*knockUpDirection*Time.deltaTime;
				transform.position += 4*Vector3.down*Time.deltaTime;
				if(Physics.Raycast(transform.position, Vector3.down, 0.3f)){
					knockedUp = false;
					gameObject.layer = 12;
				}
			}
		}
		if (downDash) {
			transform.position += 4 * Vector3.down * Time.deltaTime;
			if(downDash && Physics.Raycast(transform.position, Vector3.down, 0.25f)){
				downDash = false;
				if(!pointMan)gameObject.layer = 12;
			}
		}

	}

	void rechargeSkill() {
		recharge = false;
	}

	public void becomePointMan(){
		pointMan = true;
		gameObject.layer = 12+GetComponent<PlayerV2>().playerNum;
		timer = 0.0f;
		dash = false;
		jump = false;
		downDash = false;
		GetComponent<PlayerV2> ().vignette.enabled = true;
		InvokeRepeating("GainPoint", 1.0f, 1.0f);
	}

	public void losePointMan(){
		pointMan = false;
		CancelInvoke ("GainPoint");
		gameObject.layer = 12;
		GetComponent<PlayerV2> ().vignette.enabled = false;
	}

	void GainPoint(){
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
		timer = 0.5f;
		up = true;
		jump = false;
		gameObject.layer = 12+GetComponent<PlayerV2>().playerNum;
	}

	void OnTriggerEnter(Collider col){
		if(col.tag == "Powerup"){
			becomePointMan();
			col.GetComponent<PowerUpV2>().remove();
			//col.GetComponent<PowerupAction>().startRespawn();
		} else if(col.tag == "Danger"){
			if(pointMan)
				losePointMan();
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

	void startRespawn() {
		GetComponent<Camera>().enabled = false;
		if(!respawning) {
			losePointMan();
			dash = false;
			downDash = false;
			recharge = false;
			knockedUp = false;
			jump = false;
			up = false;
			respawning = true;
			gameObject.layer = 12;
			Invoke("respawn", 2f);
		}
	}

	void respawn() {
		GameObject[] spawns = GameObject.FindGameObjectsWithTag ("Spawn");
		GameObject spot = spawns [Random.Range (0, spawns.Length)];
		transform.position = spot.transform.position;
		gameObject.renderer.enabled = true;
		gameObject.collider.enabled =true;
		GetComponent<Camera> ().enabled = true;
		respawning = false;

	}

}
