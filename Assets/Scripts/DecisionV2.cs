using UnityEngine;
using System.Collections;
using System;





public class DecisionV2 : MonoBehaviour {
	//Option set chosen based on direction moving
	public Options NActionSet;
	public Options EActionSet;
	public Options SActionSet;
	public Options WActionSet;

	// Spawning direction
	public int spawnDir = 0;

	// Use this for initialization
	void Start () {
		gameObject.renderer.material.color = new Color(1.0f,1.0f,1.0f,0.0f);
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider col){

	}

}
