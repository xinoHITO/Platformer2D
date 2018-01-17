using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerCamera : MonoBehaviour {
	public Transform _target;
	public Vector2 _offset;
	private Vector2 _currentOffset;
	private bool _enterTransition;
	// Use this for initialization
	void Start () {
		
	}
	
	// LateUpdate se ejecuta justo despues de Update
	void LateUpdate () {

		_currentOffset = Vector2.Lerp (_currentOffset, _offset, Time.deltaTime * 5);

		Vector3 pos = _target.position;
		pos.z = -5;
		pos.x += _currentOffset.x;
		pos.y += _currentOffset.y;

		if (_enterTransition) {
			transform.position = Vector3.Lerp (transform.position, pos, Time.deltaTime * 7);

			Vector3 cameraPos = transform.position;
			cameraPos.z = 0;
			Vector3 targetPos = pos;
			targetPos.z = 0;
			float distance = Vector3.Distance (cameraPos, targetPos);
			if (distance < 0.05f) {
				_enterTransition = false;
			}
		} else {
			transform.position = pos;
		}


	}
	//esta funcion será llamada desde otro script por eso le ponemos
	//pública
	public void ChangeTarget(Transform newTarget){
		_target = newTarget;
		_enterTransition = true;
	}
}
