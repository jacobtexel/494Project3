using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public int playerNum;
	public int score;
	public bool cat; // Sets the player as the "Cat" so he can eat the mice
	public bool respawning;
	public GUIText scoreText;

	// Use this for initialization
	void Start () {
		score = 0;
		respawning = false;
		cat = false;
		scoreText.text = score.ToString ();
	}
	
	// Update is called once per frame
	void Update () {		
		scoreText.text = score.ToString ();
	}

	public void respawn() {
		gameObject.renderer.enabled = true;
		respawning = false;
		GameObject[] points = GameObject.FindGameObjectsWithTag ("Decision");
		transform.position = points [Random.Range (0, points.Length - 1)].transform.position;
	}

	void OnTriggerEnter(Collider col) {
		if (col.tag == "Player" && cat) {
			Player player = col.gameObject.GetComponent<Player>();
			if(!player.respawning) {
				score++;
				player.startRespawn();
			}
		} else if (col.tag == "Powerup") {
			GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
			for (int i=0; i<players.Length; i++){
				players[i].GetComponent<Player>().cat = false;
			}
			cat = true;
			col.gameObject.GetComponent<PowerupAction>().startRespawn();
		}
	}

	public void startRespawn() {
		if(!respawning) {
			respawning = true;
			gameObject.renderer.enabled = false;
			Invoke("respawn", 2f);
		}
	}
}
