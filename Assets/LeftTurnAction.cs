using UnityEngine;
using System.Collections;

public class LeftTurnAction : Action {
	public void Start() {
		location = Vector2.zero;
		Namestr = "Go Left";
	}

	override public void startAction() {
		MoveCamera cam = FindObjectOfType<MoveCamera> ();
		cam.turnLeft ();
	}
}
