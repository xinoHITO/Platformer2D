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
	// Update is called once per frame
	void Update () {
		h = Input.GetAxis ("Horizontal");
		if (Input.GetKeyDown (KeyCode.Space) && isGrounded) {
			pressedJump = true;	
		}
		float absH = Mathf.Abs (h);
		float speedAnim = GetComponent<Animator> ().GetFloat ("speed");
		float result = Mathf.Lerp (speedAnim, absH, Time.deltaTime * 20);
		GetComponent<Animator> ().SetFloat ("speed", result);

		if (h>0) {
			GetComponent<SpriteRenderer> ().flipX = false;
		}
		if (h<0) {
			GetComponent<SpriteRenderer> ().flipX = true;
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
		/*
		if (Input.GetKey(KeyCode.D)) {
			playerVelocity = new Vector2 (2, 0);
		}
		if (Input.GetKey(KeyCode.A)) {
			playerVelocity = new Vector2 (-2, 0);
		}
		*/
		GetComponent<Rigidbody2D> ().velocity = playerVelocity;
	}


	void OnDrawGizmos(){
		Gizmos.DrawWireCube (transform.position, verticalBoxSize);
		Gizmos.color = Color.red;//todos los gizmos que dibujes debajo de esta linea seran de color rojo
		Gizmos.DrawWireCube (transform.position, horizontalBoxSize);
	}
}
