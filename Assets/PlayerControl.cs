using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
	public float runSpeed = 5;
	public float jumpForce = 8;

//	public GameObject floorCheck;
	public LayerMask mask;
	public float gravity = 10;
	public Vector2 verticalBoxSize;
	public Vector2 horizontalBoxSize;
	private Vector2 playerVelocity;
	private float verticalSpeed;
	private bool isGrounded;

	private Animator _animator;

	// Use this for initialization
	void Start () {
		_animator = GetComponentInChildren<Animator> ();
	}

	private float h;
	private bool pressedJump;
	private int jumpCounter;

	public bool canControl;
	public bool canAttack;

	// Update is called once per frame
	void Update () {
		ReceiveInputs ();

		ManageAnimator ();

		ManageFlipping ();
	}

	void ReceiveInputs(){

		if (canControl) {
			h = Input.GetAxis ("Horizontal");
			//usamos el jumpCounter para saber cuantos saltos hemos hecho
			if (Input.GetKeyDown (KeyCode.Space) && jumpCounter < 2) {
				pressedJump = true;	
				jumpCounter++;
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
	}

	void ManageFlipping(){
		if (h>0) {
			GetComponentInChildren<SpriteRenderer> ().flipX = false;
		}
		if (h<0) {
			GetComponentInChildren<SpriteRenderer> ().flipX = true;
		}
	}

	void FixedUpdate(){
		

		playerVelocity = new Vector2 (h * runSpeed, 0);

		Vector2 downDirection = new Vector2 (0, -1);
		RaycastHit2D hitInfo = Physics2D.BoxCast (transform.position, verticalBoxSize, 0, downDirection, 0, mask.value);



//		Debug.Log (hitInfo.normal);

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
