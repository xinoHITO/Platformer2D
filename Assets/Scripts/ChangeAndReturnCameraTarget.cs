using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAndReturnCameraTarget : MonoBehaviour {
	public Transform _newTarget;
	public Transform _player;
	private PlatformerCamera _cameraScript;
	// Use this for initialization
	void Start () {
		_cameraScript = Camera.main.GetComponent<PlatformerCamera> ();
	}
	

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Player") {
			_cameraScript.ChangeTarget (_newTarget);
			Invoke ("ReturnCameraToPlayer", 2);
		}
	}

	void ReturnCameraToPlayer(){
		_cameraScript.ChangeTarget (_player);
	}
}
