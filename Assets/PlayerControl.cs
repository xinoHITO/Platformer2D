using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
	public float runSpeed = 5;
	public float jumpForce = 8;

//	public GameObject floorCheck;
	public LayerMask mask;
	public float gravity = 10;
	public Vector2 boxSize;
	private Vector2 playerVelocity;
	private float verticalSpeed;

	// Use this for initialization
	void Start () {
		
	}

	private float h;
	private bool pressedJump;
	// Update is called once per frame
	void Update () {
		h = Input.GetAxis ("Horizontal");
		if (Input.GetKeyDown (KeyCode.Space)) {
			pressedJump = true;	
		}

	}

	void FixedUpdate(){
		

		playerVelocity = new Vector2 (h * runSpeed, 0);

		Vector2 downDirection = new Vector2 (0, -1);
		RaycastHit2D hitInfo = Physics2D.BoxCast (transform.position, boxSize, 0, downDirection, 0, mask.value);



		Debug.Log (hitInfo.normal);

		//chocaste tu cabeza con el techo
		if (hitInfo.normal.y < -0.5f) {
			verticalSpeed = 0;
		}


		//estas en el piso
		if (hitInfo.normal.y > 0.5f) {
			verticalSpeed = -0.2f;

			if (pressedJump) {
				verticalSpeed = jumpForce;
				pressedJump = false;
			}	
		} 

		//estas en el aire
		if (hitInfo.normal.y <= 0.5f || hitInfo.collider == null) {
			verticalSpeed -= gravity * Time.deltaTime;
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
		Gizmos.DrawWireCube (transform.position, boxSize);

	}
}
