using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBossLifeBar : MonoBehaviour {
	public GameObject _bossLifeBar;
	// Use this for initialization
	void Start () {
		
	}
	
	void OnTriggerEnter2D (Collider2D other){	
		if (other.tag == "Player") {
			_bossLifeBar.SetActive (true);		
		}
	}
}
