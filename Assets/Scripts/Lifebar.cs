using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Lifebar : MonoBehaviour {

	private Image _image;

	public Health _playerHealth;
	// Use this for initialization
	void Start () {
		_image = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		//multiplicamos por 1.0f para que los int se transformen a float
		float lifePercentage = 1.0f*_playerHealth._currentHealth / _playerHealth._maxHealth;
		_image.fillAmount = lifePercentage;

	}
}
