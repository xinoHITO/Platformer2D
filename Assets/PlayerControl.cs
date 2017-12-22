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

	// Use this for initialization
	void Start () {
		
	}

	private float h;
	private bool pressedJump;

	public bool canControl;
	// Update is called once per frame
	void Update () {
		ReceiveInputs ();

		ManageAnimator ();

		ManageFlipping ();
	}

	void ReceiveInputs(){
		if (canControl) {
			h = Input.GetAxis ("Horizontal");
			if (Input.GetKeyDown (KeyCode.Space) && isGrounded) {
				pressedJump = true;	
			}
		} else {
			//si pierdes el control del player... forzamos a que h valga cero para que ya no se mueva el player
			h = 0;
		}

	}

	void ManageAnimator(){
		float absH = Mathf.Abs (h);
		float speedAnim = GetComponentInChildren<Animator> ().GetFloat ("speed");
		float result = Mathf.Lerp (speedAnim, absH, Time.deltaTime * 20);
		GetComponentInChildren<Animator> ().SetFloat ("speed", result);

		//ataque del player
		if (canControl) {
			if (Input.GetMouseButtonDown(0)) {
				if (isGrounded) {
					GetComponentInChildren<Animator> ().SetTrigger ("attack");	
				}
			}
		}

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



		Debug.Log (hitInfo.normal);

		//chocaste tu cabeza con el techo
		if (hitInfo.normal.y < -0.5f) {
			verticalSpeed = 0;
		}


		//estas en el piso
		if (hitInfo.normal.y > 0.5f) {
			isGrounded = true;
			verticalSpeed = -0.2f;
			if (pressedJump) {
				verticalSpeed = jumpForce;
				pressedJump = false;
			}	
		} 

		//estas en el aire
		if (hitInfo.normal.y <= 0.5f) {
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
