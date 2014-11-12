using UnityEngine;
using System.Collections;

public class MovementV2 : MonoBehaviour {

	//Input strings for movement
	public string forward;
	public string left;
	public string right;
	public string backward;
	public bool pointMan = false;
	public float dashTime = .2f;
	public float jumpTime = .5f;
	public float knockBackTime = .5f;
	public float timer;
	public int points = 0;

	public GameObject fireballPrefab;

	//Bools concerning state of player
	private bool dash = false;
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
		if(transform.position.y <= 0){
			startRespawn();
			return;
		}
		//Dash action
		if (!dash && !pointMan && Input.GetButton (left) && Input.GetButton (right) && !recharge) {
			dash = true;
			timer = 0.0f;
			gameObject.layer = 0;
		}
		//Jump action
		else if(!jump && !pointMan && !knockedUp && Physics.Raycast(transform.position, Vector3.down, 0.25f) && Input.GetButton (forward) && Input.GetButton (backward)){
			jump = true;
			timer = jumpTime;
			gameObject.layer = 0;
		}
		//Fireball action
		else if(pointMan && !recharge && Input.GetButton (left) && Input.GetButton (right)){
			GameObject fireball = Instantiate(fireballPrefab) as GameObject;
			fireball.transform.position = transform.position+transform.forward;
			fireball.GetComponent<Fireball>().direction = transform.forward;
			fireball.GetComponent<Fireball>().color = renderer.material.color;
			recharge = true;
			Invoke("rechargeSkill", 0.5f);
		}
		//Regular action
		else if(!knockedUp){
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
		if(dash && !recharge){
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
	}

	void rechargeSkill() {
		recharge = false;
	}

	public void becomePointMan(){
		pointMan = true;
		gameObject.layer = 0;
		timer = 0.0f;
		dash = false;
		GameObject.FindGameObjectWithTag ("Vignette").layer = GetComponent<PlayerV2> ().playerNum + 7;
		GameObject.FindGameObjectWithTag ("Vignette").guiTexture.color = renderer.material.color;
		InvokeRepeating("GainPoint", 1.0f, 1.0f);
	}

	public void losePointMan(){
		pointMan = false;
		CancelInvoke ("GainPoint");
		gameObject.layer = 12;
		GameObject.FindGameObjectWithTag ("Vignette").layer = 12;
		//startRespawn ();
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
		knockUpDirection = transform.position - source;
		knockedUp = true;
		timer = 0.5f;
		up = true;
		jump = false;
		gameObject.layer = 0;
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
			dash = false;
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
