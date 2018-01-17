using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCameraTarget : MonoBehaviour {
	public PlatformerCamera _camera;
	public Transform _newTarget;
	// Use this for initialization
	void Start () {
		
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag =="Player") {
			_camera.ChangeTarget (_newTarget);
			Destroy (gameObject);
		}
	}
}
