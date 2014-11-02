using UnityEngine;
using System.Collections;

public class BackwardAction : Action {
	public void Start() {
		Namestr = "Go Back";
	}
	
	override public void startAction() {
		MoveCamera cam = FindObjectOfType<MoveCamera> ();
		cam.turnAround();
	}
}
