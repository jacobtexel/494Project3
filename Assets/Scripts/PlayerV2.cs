using UnityEngine;
using System.Collections;

/*This class will eventually be used to manage data and functions related to player state*/

public class PlayerV2 : MonoBehaviour {
	public int playerNum;
	public GUITexture vignette;
	public GUITexture crosshairs;
	public GUIText score;
	public AudioClip jumpSound;
	public AudioClip hitSound;

	// Use this for initialization
	void Start () {
		vignette.enabled = false;
		crosshairs.enabled = false;
		GetComponent<TrailRenderer> ().renderer.sortingLayerID = 12;
	}

	void Update() {
		score.text = GetComponent<MovementV2> ().points + " / 10";
	}

	public void playJump(){
		audio.PlayOneShot (jumpSound);	
	}

	public void playHit(){
		audio.PlayOneShot (hitSound);
	}
}
