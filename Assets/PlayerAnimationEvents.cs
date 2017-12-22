using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour {
	public GameObject _player;

	public GameObject _rightHitbox;
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

	void ActivateHitbox(){
		_rightHitbox.SetActive (true);
	}

	void DeactivateHitbox(){
		_rightHitbox.SetActive (false);
	}
}
