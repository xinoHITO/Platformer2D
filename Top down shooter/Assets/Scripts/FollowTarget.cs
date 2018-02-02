using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour {
	public Transform _target;
	public float _lerpSpeed = 10;
	// Use this for initialization
	void Start () {
		
	}

	void LateUpdate () {
		Vector3 targetPos = _target.position;
		targetPos.z = -10;
		transform.position = targetPos;

	//	transform.position = Vector3.Lerp (transform.position, targetPos, Time.deltaTime * _lerpSpeed);
	}
}
