using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour {
	public string targetTag = "enemy";
	public int _damage = 20;
	// Use this for initialization
	void Start () {
		
	}
	
	void OnTriggerEnter2D (Collider2D other){
		//preguntamos por el tag del objeto que hemos tocado
		if (other.tag == targetTag) {
			//luego obtenemos el script Health del otro
			//y le reducimos la vida
			other.GetComponent<Health> ().ApplyDamage(_damage,gameObject);
		}
	}
}
