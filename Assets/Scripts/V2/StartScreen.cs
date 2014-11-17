using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Alpha2)){
			PlayerPrefs.SetString("numPlayers", "2");
			disableText();
			Application.LoadLevel("iceyWeiner");
		} else if(Input.GetKeyDown(KeyCode.Alpha3)){
			PlayerPrefs.SetString("numPlayers", "3");
			disableText();
			Application.LoadLevel("iceyWeiner");
		} else if(Input.GetKeyDown(KeyCode.Alpha4)) {
			PlayerPrefs.SetString("numPlayers", "4");
			disableText();
			Application.LoadLevel("iceyWeiner");
		}
	}

	void disableText() {
		foreach (GUIText text in GameObject.FindObjectsOfType<GUIText>()) {
				text.enabled = false;
		}
	}
}
