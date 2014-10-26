using UnityEngine;
using System.Collections;


public abstract class Action : MonoBehaviour {
	public string Namestr;

	public abstract void startAction();

	void OnGUI(){
		if(GUI.Button(new Rect(transform.position.x, transform.position.y, 200, 20), Namestr)){
			startAction();
		}

	}
}
