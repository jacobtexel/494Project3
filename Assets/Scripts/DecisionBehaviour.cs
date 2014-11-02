using UnityEngine;
using System.Collections;

public class DecisionBehaviour : MonoBehaviour {
	//Array chosen based on which way you are facing
	public GameObject[] NActionSet;
	public GameObject[] EActionSet;
	public GameObject[] SActionSet;
	public GameObject[] WActionSet;
	public int example = 5;


	// Use this for initialization
	void Start () {
		DisableActions ();
		gameObject.renderer.material.color = new Color(1.0f,1.0f,1.0f,0.0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void EnableActions(int direction){
		print ("Actions Enabled: " + direction);
		switch(direction){
		case 0:
			foreach(GameObject a in NActionSet){
				GameObject newObject = Instantiate (a) as GameObject;
			}
			break;
		case 1:
			foreach(GameObject a in EActionSet){
				GameObject newObject = Instantiate (a) as GameObject;
			}
			break;
		case 2:
			foreach(GameObject a in SActionSet){
				GameObject newObject = Instantiate (a) as GameObject;
			}
			break;
		case 3:
			foreach(GameObject a in WActionSet){
				GameObject newObject = Instantiate (a) as GameObject;
			}
			break;
		}
	}

	public void DisableActions(){
		foreach(GameObject a in GameObject.FindGameObjectsWithTag("Action"))
			Destroy (a);
	}

	void OnTriggerEnter(Collider col){
		if(col.gameObject.tag == "MainCamera"){
			print("Collision!");
			EnableActions(col.gameObject.GetComponent<MoveCamera>().direction);
		}
	}
}
