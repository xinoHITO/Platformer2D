using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
	//vida actual del personaje
	public int _currentHealth = 100;
	//vida maxima del personaje
	public int _maxHealth = 100;

	public GameObject _lastAttacker;

	// Use this for initialization
	void Start () {
		
	}
	
	public void ApplyDamage(int damage,GameObject attacker){
		//reducimos la cantidad de vida en base al dano
		_currentHealth -= damage;
		//guardamos la referencia de quien te ha hecho dano
		_lastAttacker = attacker;
	}
}
