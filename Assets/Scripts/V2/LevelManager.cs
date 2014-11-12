﻿using UnityEngine;
using System.Collections;

/* This class should be attached to something like the Lights prefab or the minimap.
 * This class is intended to manage any events related to the state of a level (ex:upgrade location)*/

public class LevelManager : MonoBehaviour {
	public GameObject UpgradePrefab;


	public float upgradeRespawnTimer;

	// Use this for initialization
	void Start () {
		//GetComponent<GUIText> ().text = "Victorious Player: " + PlayerPrefs.GetString ("winner");
		string numPlayers = PlayerPrefs.GetString ("numPlayers");
		//string numPlayers = "3";
		if(numPlayers == "2") {
			GameObject player2 = GameObject.Find("Player2Cam");
			Destroy(player2);
			GameObject player3 = GameObject.Find("Player3Cam");
			Destroy(player3);
		} else if(numPlayers == "3") {
			GameObject player2 = GameObject.Find("Player2Cam");
			Destroy(player2);
		} else {
			Debug.Log("Something has went wrong and we have recieved something that made no sense in player number selection");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(upgradeRespawnTimer<=0.0f && GameObject.FindGameObjectsWithTag("Powerup").Length == 0){
			spawnPowerup();
			upgradeRespawnTimer = 5.0f;
		}
		else if(GameObject.FindGameObjectsWithTag("Powerup").Length == 0)
			upgradeRespawnTimer -= Time.deltaTime;
	}

	void spawnPowerup(){
		if(GameObject.FindGameObjectsWithTag("PowerupSpawn").Length == 0)
			print ("No power up spawn points on the level!");
		else {
			GameObject[] spawns = GameObject.FindGameObjectsWithTag ("PowerupSpawn");
			GameObject powerUp = Instantiate(UpgradePrefab) as GameObject;
			powerUp.transform.position = spawns [Random.Range (0, spawns.Length)].transform.position;
		}
	}
}
