using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
	public GameObject floorCheck;
	public LayerMask mask;
	public float gravity = 10;
	public Vector2 boxSize;
	private Vector2 playerVelocity;
	private float verticalSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float h = Input.GetAxis ("Horizontal");

		playerVelocity = new Vector2 (h * 2, 0);

		Collider2D theCollider = Physics2D.OverlapBox (transform.position, boxSize, 0,mask.value);

	//	RaycastHit2D hitInfo = Physics2D.Linecast (transform.position, floorCheck.transform.position,mask.value);

		//estas en el aire
		if (theCollider == null) {
			verticalSpeed -= gravity * Time.deltaTime;
		} else {//estas en el piso
			verticalSpeed = -0.2f;
		}



		Debug.Log (verticalSpeed);

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
