using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
	public float runSpeed = 5;
	public float jumpForce = 8;

	public float dashTime = 0.8f;//la duracion del dash
	public float dashSpeed = 8;  //la velocidad del player mientras esta en el dash
	private float targetDashSpeed;
	private float currentDashSpeed;
	/***************
	public enum PlayerState
	{
		Normal,
		Attack,
		Dash
	}
	public PlayerState playerState;
	****************/
	public bool isPlayerDashing = false;

	public LayerMask mask;
	public float gravity = 10;
	public Vector2 verticalBoxSize;
	public Vector2 horizontalBoxSize;
	private Vector2 playerVelocity;
	private float verticalSpeed;
	private bool isGrounded;

	private Animator _animator;
	private Health _healthScript;
	private SpriteRenderer _spriteRenderer;

	// Use this for initialization
	void Start () {
		_animator = GetComponentInChildren<Animator> ();
		_healthScript = GetComponent<Health> ();
		_spriteRenderer = GetComponentInChildren<SpriteRenderer> ();
	}

	private float h;
	private bool pressedJump;
	private int jumpCounter;
	private bool pressedDash;

	public bool canControl;
	public bool canAttack;

	// Update is called once per frame
	void Update () {
		ReceiveInputs ();

		Dash ();

		ManageAnimator ();

		ManageFlipping ();

		ManageBlinking ();

		previousHealth = _healthScript._currentHealth;
	}

	void ReceiveInputs(){

		if (canControl|| isPlayerDashing) {
			//usamos el jumpCounter para saber cuantos saltos hemos hecho
			if (Input.GetKeyDown (KeyCode.Space) && jumpCounter < 2) {
				pressedJump = true;	
				jumpCounter++;
			}
		}

		if (canControl) {
			h = Input.GetAxis ("Horizontal");


			if (isGrounded) {
				if (Input.GetKeyDown(KeyCode.LeftShift)) {
					pressedDash = true;
				}
			}

		} else {
			//si pierdes el control del player... forzamos a que h valga cero para que ya no se mueva el player
			h = 0;
		}


	}

	void ManageAnimator(){
		float absH = Mathf.Abs (h);
		float speedAnim = _animator.GetFloat ("speed");
		float result = Mathf.Lerp (speedAnim, absH, Time.deltaTime * 20);
		_animator.SetFloat ("speed", result);

		//ataque del player
		if (canAttack) {
			if (Input.GetMouseButtonDown(0)) {
				_animator.SetTrigger ("attack");
			}
		}


		_animator.SetBool ("isGrounded", isGrounded);
		_animator.SetFloat ("verticalSpeed", verticalSpeed);
		if (targetDashSpeed == dashSpeed) {
			_animator.SetBool ("dash", true);	
		} else {
			_animator.SetBool ("dash", false);	
		}

	}

	void Dash(){
		if (pressedDash) {
			canControl = false;
			canAttack = false;
			isPlayerDashing = true;
			//al principio targetDashSpeed sera igual a la velocidad normal del dash
			targetDashSpeed = dashSpeed;
			//hacemos que current sea igual a target porque el cambio de velocidad debe ser instantaneo la inicio del dash
			//solo al final sea hace un cambio gradual hacia cero
			currentDashSpeed = targetDashSpeed;
			Invoke ("EndDash", dashTime);
			pressedDash = false;
		}

		//la logica de aplicar la ralentizacion solo se debe ejecutar en el piso
		//esto permite que si saltas en medio de un dash... mantienes el momentum del dash
		if (isGrounded) {
			//currentDashSpeed siempre esta tratando de acercarse a targetDashSpeed
			currentDashSpeed = Mathf.Lerp (currentDashSpeed, targetDashSpeed, Time.deltaTime * 4);
		}

		//si te sales del piso mientras haces dash... recuperas el control del player y tmb que pueda atacar (ataque en el aire)
		if (!isGrounded) {
			canControl = true;
			canAttack = true;
		}

		//cuando la velocidad de dash sea casi cero... significa que ya terminamos
		if (currentDashSpeed < 1.2f) {
			isPlayerDashing = false;
		}
	}

	void EndDash(){
		targetDashSpeed = 0;
	}

	void ManageFlipping(){
		if (canControl) {
			if (h>0) {
				GetComponentInChildren<SpriteRenderer> ().flipX = false;
			}
			if (h<0) {
				GetComponentInChildren<SpriteRenderer> ().flipX = true;
			}	
		}
	}

	private Color currentColor;
	private Color targetColor;
	private bool isBlinking;
	private float previousHealth;

	void ManageBlinking(){
		if (previousHealth > _healthScript._currentHealth) {
			isBlinking = true;
			Invoke ("StopBlinking", 0.8f);
		}

		if (isBlinking) {
			if (currentColor.a < 0.1f) {
				targetColor = Color.white;
			}
			if (currentColor.a > 0.9f) {
				Color transparentWhite = Color.white;
				transparentWhite.a = 0;
				targetColor = transparentWhite;
			}
			currentColor = Color.Lerp (currentColor, targetColor, Time.deltaTime * 14);
			_spriteRenderer.color = currentColor;
		}

	}

	void StopBlinking(){
		isBlinking = false;
		_spriteRenderer.color = Color.white;
	}

	void FixedUpdate(){
		

		playerVelocity = new Vector2 (h * runSpeed, 0);
		if (isPlayerDashing) {
			if (GetComponentInChildren<SpriteRenderer> ().flipX == true) {
				playerVelocity = new Vector2 (-currentDashSpeed, 0);
			} else {
				playerVelocity = new Vector2 (currentDashSpeed, 0);
			}
		}


		Vector2 downDirection = new Vector2 (0, -1);
		RaycastHit2D hitInfo = Physics2D.BoxCast (transform.position, verticalBoxSize, 0, downDirection, 0, mask.value);


		//chocaste tu cabeza con el techo
		if (hitInfo.normal.y < -0.5f) {
			verticalSpeed = 0;
		}


		//estas en el piso
		if (hitInfo.normal.y > 0.5f) {
			//si isGrounded estaba en falso pero el boxcast tocó el piso... quiere decir que
			//acabamos de aterrizar
			if (!isGrounded) {
				//reseteamos el contador de salto
				jumpCounter = 0;
				if (isPlayerDashing) {
					isPlayerDashing = false;
				}
			}
			isGrounded = true;
			verticalSpeed = -0.2f;
		} 

		//el salto puede darse en el aire como en el piso (gracias al doble salto)
		if (pressedJump) {
			verticalSpeed = jumpForce;
			pressedJump = false;
		}

		//estas en el aire
		if (hitInfo.normal.y <= 0.5f) {
			//si isGrounded es true pero el boxcast dice que estamos en el aire entonces significa
			//que nos acabamos de dejar caer de un precipicio
			if (isGrounded) {
				//si te dejas caer ... seteamos el contador en uno para evitar que hagas 2 saltos en el aire
				jumpCounter = 1;
			}
			isGrounded = false;
			verticalSpeed -= gravity * Time.deltaTime;
		}



		hitInfo = Physics2D.BoxCast (transform.position, horizontalBoxSize, 0, downDirection, 0, mask.value);

		if (hitInfo.normal.x < -0.5f) {//si te chocaste por la derecha
			if (h>0) {	//y estas tratando de que el player se mueva hacia la derecha
				playerVelocity.x = 0; //frenamos el movimiento horizontal
			}
		}

		if (hitInfo.normal.x > 0.5f) {//si te chocaste por la izquierda
			if (h<0) {	//y estas tratando de que el player se mueva hacia la izquierda
				playerVelocity.x = 0; //frenamos el movimiento horizontal
			}
		}

		//	Debug.Log (verticalSpeed);

		playerVelocity.y = verticalSpeed;

		GetComponent<Rigidbody2D> ().velocity = playerVelocity;
	}


	void OnDrawGizmos(){
		Gizmos.DrawWireCube (transform.position, verticalBoxSize);
		Gizmos.color = Color.red;//todos los gizmos que dibujes debajo de esta linea seran de color rojo
		Gizmos.DrawWireCube (transform.position, horizontalBoxSize);
	}
}
