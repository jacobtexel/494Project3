using UnityEngine;
using System.Collections;

public class RightTurnAction : Action {
	public void Start() {
		Namestr = "Go Right";
	}
	
	override public void startAction() {
		MoveCamera cam = FindObjectOfType<MoveCamera> ();
		cam.turnRight();
	}
}