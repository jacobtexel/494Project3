using UnityEngine;
using System.Collections;

public class PowerupAction : MonoBehaviour {
	void Start() {
		respawn ();
	}

	public void startRespawn() {
		Vector3 pos = transform.position;
		pos.y = -100f;
		transform.position = pos;
		Invoke("respawn", Random.Range(1f, 7f));

	}

	void respawn() {
		GameObject[] spawns = GameObject.FindGameObjectsWithTag ("PowerupSpawn");
		transform.position = spawns [Random.Range (0, spawns.Length)].transform.position;
	}
}
