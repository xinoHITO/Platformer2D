using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	public float force = 500;
	public float deathTimer = 3;
	public float shootRate = 0.1f;
	public float damage = 20;

	public bool destroyOnTrigger = true;
	// Use this for initialization
	void Start () {
		if (force > 0) {
			GetComponent<Rigidbody2D> ().AddForce (transform.up * force);	
		}
		Invoke ("DestroyBullet", deathTimer);


	}
	
	void DestroyBullet () {
		Destroy (gameObject);
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Enemy") {
			other.GetComponent<Health> ()._currentHealth -= damage;
			if (destroyOnTrigger) {
				Destroy (gameObject);	
			}
		}
	}
}
