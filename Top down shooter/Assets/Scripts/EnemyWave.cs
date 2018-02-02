using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave : MonoBehaviour {
	public GameObject _enemyPrefab;
	public int _count = 5;
	// Use this for initialization
	void Start () {
		
	}
	
	void OnTriggerEnter2D (Collider2D other) {
		if(other.tag == "Player"){
			for(int i = 0;i<_count;i++){

				Vector3 pos = transform.position;
				Instantiate(_enemyPrefab,transform.position,transform.rotation);
				GetComponent<Collider2D>().enabled = false;
			}
		}
	}
}
