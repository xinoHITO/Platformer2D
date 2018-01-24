using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	public float force = 500;
	public float deathTimer = 3;
	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody2D> ().AddForce (transform.up * force);
		Invoke ("DestroyBullet", deathTimer);


	}
	
	void DestroyBullet () {
		Destroy (gameObject);
	}
}
