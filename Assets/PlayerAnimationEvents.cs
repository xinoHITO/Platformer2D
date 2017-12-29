using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour {
	public GameObject _player;

	public GameObject _rightHitbox;
	public GameObject _leftHitbox;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void ActivateCanControl(){
		_player.GetComponent<PlayerControl> ().canControl = true;
	}

	void DeactivateCanControl(){
		_player.GetComponent<PlayerControl> ().canControl = false;
	}

	void ActivateCanAttack(){
		_player.GetComponent<PlayerControl> ().canAttack = true;
	}

	void DeactivateCanAttack(){
		_player.GetComponent<PlayerControl> ().canAttack = false;
	}

	void ActivateHitbox(){
		bool flipX = GetComponent<SpriteRenderer> ().flipX;
		//si estas mirando a la izquierda
		if (flipX) {
			//solo prendemos el hitbox izquierdo
			_leftHitbox.SetActive (true);
		} else {//si estas mirando a la derecha
			//solo prendemos el hitbox derecho
			_rightHitbox.SetActive (true);
		}

	}

	void DeactivateHitbox(){
		//apagamos ambos hitboxes
		_leftHitbox.SetActive (false);
		_rightHitbox.SetActive (false);
	}
}
