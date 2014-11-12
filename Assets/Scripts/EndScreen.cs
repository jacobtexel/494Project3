﻿using UnityEngine;
using System.Collections;

public class EndScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<GUIText> ().text = "Victorious Player: " + PlayerPrefs.GetString ("winner");
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("forward"))
			Application.LoadLevel("_Start_Scene");
	}
}
