using UnityEngine;
using System.Collections;

/*This class will eventually be used to manage data and functions related to player state*/

public class PlayerV2 : MonoBehaviour {
	public int playerNum;
	public GUITexture vignette;
	public GUITexture timerBar;

	// Use this for initialization
	void Start () {
		vignette.enabled = false;
	}

	void Update() {
		Vector3 barscale = timerBar.transform.localScale;
		barscale.x = .5f * ((60-GetComponent<MovementV2>().points)/60.0f);
		timerBar.transform.localScale = barscale;
	}
}
