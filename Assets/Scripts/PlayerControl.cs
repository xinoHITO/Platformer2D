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

	private float knockback;
	private bool knockBackToTheLeft;

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

	private Vector2 horBoxcastNormal;


	private bool isHugginWall;
	private bool stickPlayerToWall;
	private bool isInWallJump;

	// Update is called once per frame
	void Update () {
		ReceiveInputs ();

		Dash ();

		ManageAnimator ();

		ManageFlipping ();

		HugWall ();

		ManageKnockback ();

		ManageBlinking ();

		previousHealth = _healthScript._currentHealth;
	}

	void ReceiveInputs(){

		if (canControl|| isPlayerDashing) {
			//usamos el jumpCounter para saber cuantos saltos hemos hecho
			if (Input.GetKeyDown (KeyCode.Space) && jumpCounter < 2) {
				pressedJump = true;	
				jumpCounter++;
//				Debug.Log ("pressed jump");
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

		if (knockback > 0) {
			_animator.SetBool ("hurt", true);
		} else {
			_animator.SetBool ("hurt", false);
		}

		_animator.SetBool ("hugWall", isHugginWall);

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
		if (!isGrounded && isPlayerDashing) {
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

	void HugWall(){

		bool newIsHuggingWall = false;

		//solo puedes estar agarrado del muro en el aire
		if (isGrounded == false) {
			if (horBoxcastNormal.x < -0.5f) {//si te chocaste por la derecha
				if (h>0) {	//y estas tratando de que el player se mueva hacia la derecha
					newIsHuggingWall = true;
				}
			}

			if (horBoxcastNormal.x > 0.5f) {//si te chocaste por la izquierda
				if (h<0) {	//y estas tratando de que el player se mueva hacia la izquierda
					newIsHuggingWall = true;
				}
			}	
		}


		if (stickPlayerToWall) {
			newIsHuggingWall = true;
		}

		//si el frame anterior estaba pegado a la pared y ahora no...
		if (isHugginWall == true && newIsHuggingWall == false) {
			stickPlayerToWall = true;
			newIsHuggingWall = true;
			Invoke ("CancelStickToWall", 0.5f);
			Debug.Log ("stick player to wall");
		}


		isHugginWall = newIsHuggingWall;

		if (isHugginWall) {
			jumpCounter = 1;
		}
		if (isHugginWall && pressedJump) {
			canControl = false;
			isInWallJump = true;
			Invoke ("CancelWallJump", 0.2f);
		}
	}

	void CancelStickToWall(){
		stickPlayerToWall = false;
		isHugginWall = false;
		Debug.Log ("CANCEL stick player to wall");

	}

	void CancelWallJump(){
		canControl = true;
		isInWallJump = false;
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


	void ManageKnockback(){
		//hemos detectado que te han golpeado
		if (previousHealth > _healthScript._currentHealth) {
			//comienza el knockback
			knockback = 5;

			//si te golpeas estando en el aire hay un leve rebote
			if (!isGrounded) {
				verticalSpeed = 2;
			}

			//pierdes el control del player
			canControl = false;
			//revisamos nuestra posicion con respecto al que nos hizo dano
			//para saber si debemos empujarnos hacia la derecha o izquierda
			if (_healthScript._lastAttacker.transform.position.x > transform.position.x) {
				knockBackToTheLeft = true;
			} else {
				knockBackToTheLeft = false;
			}
		}

		//si existe knockback
		if (knockback > 0) {
			//decrementamos el knockback
			knockback -= 12 * Time.deltaTime;
			//preguntamos si ya termino el knockback
			if (knockback <= 0) {
				//recuperamos el control del player
				canControl = true;
				knockback = 0;
			}	
		}

	}

	private Color currentColor;
	private Color targetColor;
	private bool isBlinking;
	private float previousHealth;

	void ManageBlinking(){

		//hemos detectado que te han golpeado
		if (previousHealth > _healthScript._currentHealth) {
			gameObject.layer = 9;//entras al layer de invulnerabilidad
			isBlinking = true;
			Invoke ("StopBlinking", 1.6f);
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
		gameObject.layer = 0;//vuelves a ser vulnerable
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

		//estas pegado a la pared
		if (stickPlayerToWall) {
			playerVelocity = new Vector2 (0,0);
		}

		if (isInWallJump) {
			if (_spriteRenderer.flipX) {
				playerVelocity = new Vector2 (5, 0);
			} else {
				playerVelocity = new Vector2 (-5, 0);
			}
		}

		if (knockback > 0) {
			if (knockBackToTheLeft) {
				playerVelocity = new Vector2 (-knockback, 0);	
			} else {
				playerVelocity = new Vector2 (knockback, 0);
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
			if (isHugginWall) {
				verticalSpeed = -2;
			}


		}


		//el salto puede darse en el aire como en el piso (gracias al doble salto)
		if (pressedJump) {
			verticalSpeed = jumpForce;
			pressedJump = false;

			if (isHugginWall) {
				//apagamos el canControl para que isHugginWall se vuelva falso
				canControl = false;
				isHugginWall = false;
				CancelStickToWall ();
			}
		}


		hitInfo = Physics2D.BoxCast (transform.position, horizontalBoxSize, 0, downDirection, 0, mask.value);
		//almacenamos la informacion de la normal en una variable global 
		//porque la necesitaremos en otras funciones
		horBoxcastNormal = hitInfo.normal;

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
