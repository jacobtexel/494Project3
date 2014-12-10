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
	Color tempColor;
	bool mUnderSet;
	bool pUnderSet;

	// Use this for initialization
	void Start () {
		pUnder.transform.position = p4.transform.position;
		mUnder.transform.position = m1.transform.position;
		playerLevel = true;
		mapLevel = false;
		mUnderSet = false;
		pUnderSet = false;
		current = players;
		tempColor = current.color;
		current.color = Color.yellow;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.UpArrow)) {
			if(playerLevel)
				playerLevel = false;
			else if(mapLevel) {
				mapLevel = false;
				playerLevel = true;
			}
			else if(current == start || current == controls)
				mapLevel = true;
			if(pUnderSet)
				pUnder.color = tempColor;
			if(mUnderSet)
				mUnder.color = tempColor;
			current.color = tempColor;
			current = current.GetComponent<TextMeshScript>().up;
			current.color = Color.yellow;
		}
		if(Input.GetKeyDown(KeyCode.LeftArrow)) {
			if(pUnderSet)
				pUnder.color = tempColor;
			if(mUnderSet)
				mUnder.color = tempColor;
			current.color = tempColor;
			current = current.GetComponent<TextMeshScript>().left;
			current.color = Color.yellow;
		}
		if(Input.GetKeyDown(KeyCode.RightArrow)) {
			if(pUnderSet)
				pUnder.color = tempColor;
			if(mUnderSet)
				mUnder.color = tempColor;
			current.color = tempColor;
			current = current.GetComponent<TextMeshScript>().right;
			current.color = Color.yellow;
		}
		if(Input.GetKeyDown(KeyCode.DownArrow)) {
			if(pUnderSet)
				pUnder.color = tempColor;
			if(mUnderSet)
				mUnder.color = tempColor;
			if(playerLevel) {
				playerLevel = false;
				mapLevel = true;
			}
			else if(mapLevel) {
				mapLevel = false;
			}
			if(current == start || current == controls)
				playerLevel = true;
			current.color = tempColor;
			current = current.GetComponent<TextMeshScript>().down;
			current.color = Color.yellow;
		}
		if(Input.GetKeyDown(KeyCode.Return)) {
			if(current != players && current != map && current != start && current != controls) {
				if(playerLevel) {
					pUnder.transform.position = current.transform.position;
					pUnder.color = Color.yellow;
					pUnderSet = true;
				}
				else if(mapLevel) {
					mUnder.transform.position = current.transform.position;
					mUnder.color = Color.yellow;
					mUnderSet = true;
				}
			}
			if(current == start) {
				if(pUnder.transform.position == p2.transform.position) {
					//print ("2");
					PlayerPrefs.SetString("numPlayers", "2");
				}
				else if(pUnder.transform.position == p3.transform.position) {
					//print("3");
					PlayerPrefs.SetString("numPlayers", "3");
				}
				else if(pUnder.transform.position == p4.transform.position) {
					//print("4");
					PlayerPrefs.SetString("numPlayers", "4");
				}
				if(mUnder.transform.position == m1.transform.position)
					Application.LoadLevel(1);
				else if(mUnder.transform.position == m2.transform.position)
					Application.LoadLevel(2);
				else {
					Application.LoadLevel (Random.Range(1,3));
				}
			}
			else if(current == controls) {
				//load the controls page
			}
		}
	}

	//give up, down, left and right functionality in a script to each TextMesh. On these directions, call current's
	//direction
	//turn the color of the current TextMesh yellow

	//depending on the position of pUnder, call playerPreferences("numplayers", that number)
	//this can be found in startScreen script
}
