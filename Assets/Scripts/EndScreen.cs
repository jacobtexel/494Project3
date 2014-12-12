using UnityEngine;
using System.Collections;

public class EndScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		int winner = PlayerPrefs.GetInt ("winner");
		switch(winner){
		case 1:
			GetComponent<GUIText> ().text = "Red Player Wins!";
			break;
		case 2:
			GetComponent<GUIText>().text = "Blue Player Wins!";
			break;
		case 3:
			GetComponent<GUIText>().text = "Yellow Player Wins!";
			break;
		case 4:
			GetComponent<GUIText>().text = "Green Player Wins!";
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("one"))
			Application.LoadLevel(0);
		else if(Input.GetButtonDown ("two"))
			Application.Quit();
	}
}
