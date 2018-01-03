using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour {
	private Health healthScript;
	private SpriteRenderer spriteRend;
	private float previousHealth;

	private Color currentColor;
	private Color targetColor;

	private bool isBlinking;
	// Use this for initialization
	void Start () {
		healthScript = GetComponent<Health> ();
		spriteRend = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (healthScript._currentHealth <= 0) {
			Destroy(gameObject);	
		}
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

		//siempre actualizamos previousHealth para que sea la vida actual...
		//todo lo que este antes de esta linea ... tendrá a previousHealth como la vida que tenia el enemigo el frame anterior
		previousHealth = healthScript._currentHealth;
	}

	void StopBlinking(){
		isBlinking = false;
		spriteRend.color = Color.black;
	}
}
