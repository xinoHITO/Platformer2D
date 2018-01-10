using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour {
	public GameObject _hitbox;
	private Health healthScript;
	private SpriteRenderer spriteRend;
	private Animator _animator;
	private float previousHealth;

	private Color currentColor;
	private Color targetColor;

	private bool isBlinking;

	private GameObject _playerGO;
	// Use this for initialization
	void Start () {

		_playerGO = GameObject.FindGameObjectWithTag ("Player");

		healthScript = GetComponent<Health> ();
		spriteRend = GetComponent<SpriteRenderer> ();
		_animator = GetComponent<Animator> ();
		InvokeRepeating ("Attack", 2.0f, 2.0f);
	}
	
	// Update is called once per frame
	void Update () {
		if (healthScript._currentHealth <= 0) {
			Destroy(gameObject);	
		}

		ManageFlippping ();

		ManageBlinking ();

		//siempre actualizamos previousHealth para que sea la vida actual...
		//todo lo que este antes de esta linea ... tendrá a previousHealth como la vida que tenia el enemigo el frame anterior
		previousHealth = healthScript._currentHealth;
	}

	void ManageFlippping(){
		Vector3 newScale = transform.localScale;
		if (_playerGO.transform.position.x < transform.position.x) {
			newScale.x = Mathf.Abs (newScale.x) * -1;
		} else {
			newScale.x = Mathf.Abs (newScale.x);
		}

		transform.localScale = newScale;
	}

	void ManageBlinking(){
		//si hace un frame teniamos más vida que ahora... entonces hemos perdido vida
		if (previousHealth > healthScript._currentHealth) {
			isBlinking = true;
			Invoke ("StopBlinking", 0.8f);
		}

		if (isBlinking) {
			if (currentColor.r < 0.1f) {
				targetColor = Color.red;
			}
			if (currentColor.r > 0.9f) {
				targetColor = Color.black;
			}

			currentColor = Color.Lerp (currentColor, targetColor, Time.deltaTime * 13.5f);
			spriteRend.color = currentColor;				
		}
	}

	void StopBlinking(){
		isBlinking = false;
		spriteRend.color = Color.black;
	}

	void Attack(){
		_animator.SetTrigger ("attack");
	}

	void ActivateHitbox(){
		_hitbox.SetActive(true);
	}

	void DeactivateHitbox(){
		_hitbox.SetActive(false);
	}
}
