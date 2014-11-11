using UnityEngine;
using System.Collections;

public class SpawnAction : MonoBehaviour {

	void OnTriggerEnter(Collider col) {
		if(col.tag == "MainCamera") {
			gameObject.tag = "Untagged";
		}
	}

	void OnTriggerExit(Collider col) {
		if(col.tag == "MainCamera") {
			gameObject.tag = "Spawn";
		}
	}
}
