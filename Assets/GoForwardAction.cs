using UnityEngine;
using System.Collections;

public class GoForwardAction : Action {
	public void Start() {
		Namestr = "Go Forward";
	}
	
	override public void startAction() {
		MoveCamera cam = FindObjectOfType<MoveCamera> ();
		cam.continueMoving();
	}
}
