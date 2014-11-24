using UnityEngine;
using System.Collections;

public class EndScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<GUIText> ().text = "Victorious Player: " + PlayerPrefs.GetString ("winner");
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("one"))
			Application.LoadLevel(0);
		else if(Input.GetButtonDown ("two"))
			Application.Quit();
	}
}
