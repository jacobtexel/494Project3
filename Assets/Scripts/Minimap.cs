using UnityEngine;
using System.Collections;

public class Minimap : MonoBehaviour {
	public bool flashing = false;
	private bool mapIsUp = true;
	private float lastTime = -10f;
	private float durationMapIsUp = 5f;
	private float durationMapIsGone = 7f;
	// Use this for initialization
	void Start () {
		flashing = true;
	}
	
	// Update is called once per frame
	void Update () {
	
		if(flashing)
		{
			if(mapIsUp && Time.time - lastTime > durationMapIsUp)
			{
				this.camera.enabled = false;
				mapIsUp = false;
				lastTime = Time.time;
			}
			else if(!mapIsUp && Time.time - lastTime > durationMapIsGone)
			{
				this.camera.enabled = true;
				mapIsUp = true;
				lastTime = Time.time;
			}
		}

	}
}
