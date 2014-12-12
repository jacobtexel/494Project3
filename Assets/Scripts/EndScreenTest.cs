using UnityEngine;
using System.Collections;

public class EndScreenTest : MonoBehaviour {
	public GUIText player;
	private int winner;


	// Use this for initialization
	void Start () {
		winner = PlayerPrefs.GetInt ("winner");
		winner = 4;
		if (winner == 1) {
			player.text = "Red Player";
			player.color = Color.red;
		}
		else if(winner == 2) {
			player.text = "Blue Player";
			player.color = Color.blue;
		}
		else if(winner == 3) {
			player.text = "Yellow Player";
			player.color = Color.yellow;
		}
		else if(winner == 4) {
			player.text = "Green Player";
			player.material.color = Color.green;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Return)) {
			print ("help");
			Application.LoadLevel(0);
		}
	}
}
