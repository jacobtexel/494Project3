using UnityEngine;
using System.Collections;

public class GUICrossHair : MonoBehaviour {

	public Texture2D crosshairTexture;
	public Rect position;
	static bool OriginalOn = true;

	// Use this for initialization
	void Start () {
		//position = Rect((Screen.width - crosshairTexture.width) / 2, (Screen.height - crosshairTexture.height) /2, crosshairTexture.width, crosshairTexture.height);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		if(OriginalOn == true) { 
			GUI.DrawTexture(position, crosshairTexture); 
		}
	}
}
