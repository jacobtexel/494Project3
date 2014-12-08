using UnityEngine;
using System.Collections;

public class MenuEngine : MonoBehaviour {
	bool mapLevel;
	bool playerLevel;
	public TextMesh players;
	public TextMesh map;
	public TextMesh current;
	public TextMesh p2;
	public TextMesh p3;
	public TextMesh p4;
	public TextMesh m1;
	public TextMesh m2;
	public TextMesh mR;
	public TextMesh start;
	public TextMesh controls;
	public TextMesh pUnder;
	public TextMesh mUnder;
	int playerNumber = 4;
	int mapNumber = 1;

	// Use this for initialization
	void Start () {
		pUnder.transform.position = p4.transform.position;
		mUnder.transform.position = m1.transform.position;
		playerLevel = true;
		mapLevel = false;
		players.color = Color.yellow;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//give up, down, left and right functionality in a script to each TextMesh. On these directions, call current's
	//direction
	//turn the color of the current TextMesh yellow

	//depending on the position of pUnder, call playerPreferences("numplayers", that number)
	//this can be found in startScreen script
}
