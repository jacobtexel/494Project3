using UnityEngine;
using System.Collections;


public abstract class Action : MonoBehaviour {
	public string Namestr;
	public Vector2 location;

	public abstract void startAction();

	public void OnGUI(){
		if(GUI.Button(new Rect(location.x, location.y, 200, 20), Namestr)){
			FindObjectOfType<MoveCamera>().moves--;
			startAction();
			GameObject obj = GameObject.FindGameObjectWithTag("Decision");
			obj.GetComponent<DecisionBehaviour>().DisableActions();
		}

	}
}
