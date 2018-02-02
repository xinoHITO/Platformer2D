using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour {
	public float damage = 20;
	public string targetTag = "Enemy";
	public bool destroyOnTrigger = true;
	// Use this for initialization
	void Start () {
		
	}
	
	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == targetTag) {
			other.GetComponent<Health> ()._currentHealth -= damage;
			if (destroyOnTrigger) {
				Destroy (gameObject);	
			}
		}
	}
}
