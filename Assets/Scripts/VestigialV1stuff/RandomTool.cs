using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomTool : MonoBehaviour {

	public bool chasePoints;
	public float speed = 1;
	bool foundNow = true;
	Vector3 nextPoint;
	Vector3 currentPos;
	public List<string> usedPoints = new List<string> ();
	public List<Vector3> spawnPoints = new List<Vector3>();
	public Vector3 thePoint;
	public int xValue;
	public int yValue;
	public int zValue;
	
	// Use this for initialization
	void Start () {
		this.transform.position = getRandomLocation ();
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public Vector3 getRandomLocation() {
		int where;
		where = Random.Range (0, spawnPoints.Count);
		return spawnPoints [where];
	}
	
	public void addPoint() {
		string check = thePoint.x.ToString () + ',' + thePoint.y.ToString () + ',' + thePoint.z.ToString ();
		bool found = false;
		foreach (string name in usedPoints) {
			if (name == check)
				found = true;
		}
		if (!found) {
			Vector3 adder = new Vector3();
			adder = thePoint;
			spawnPoints.Add (adder);
			usedPoints.Add (check);
		}
	}
	
	public void removePoint() {
		string remove = thePoint.x.ToString () + ',' + thePoint.y.ToString () + ',' + thePoint.z.ToString ();
		for (int i = 0; i < usedPoints.Count; i++) {
			if (usedPoints [i] == remove) {
				spawnPoints.RemoveAt (i);
				usedPoints.RemoveAt (i);
				break;
			}
		}
	}
	
	public void getPoints() {
		foreach (string word in usedPoints) {
			print(word);
		}
	}
	
	public void deletePoints() {
		usedPoints.Clear ();
		spawnPoints.Clear ();
	}
}
