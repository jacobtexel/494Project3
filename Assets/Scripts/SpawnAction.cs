using UnityEngine;
using System.Collections;

public class SpawnAction : MonoBehaviour {

	public bool occupied;

	void Start() {
		occupied = false;
	}

	void OnTriggerEnter(Collider col) {
		if(col.tag == "MainCamera") {
			occupied = true;
		}
	}

	void OnTriggerExit(Collider col) {
			occupied = false;
	}
}
