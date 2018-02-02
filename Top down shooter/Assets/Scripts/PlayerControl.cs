using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
	public float _speed = 5;
	public GameObject _bulletPrefab;
	public GameObject _bulletPrefab2;
	private GameObject _currentBulletPrefab;
	public Transform _bulletSpawn;
	private Rigidbody2D _rigidbody;

	private float h;
	private float v;
	private bool pressedShoot;
	private bool releasedShoot;

	private float _shootTimer;
	// Use this for initialization
	void Start () {
		_rigidbody = GetComponent<Rigidbody2D> ();
		_currentBulletPrefab = _bulletPrefab;
	}
	
	// Update is called once per frame
	void Update () {
		

		ManageInput ();

		FaceMouseCursor ();

		ChangeWeapon ();

		ShootWeapon ();

	}
		
	void FixedUpdate(){
		Vector2 playerVelocity = new Vector2 (h, v);
		//normalizamos para que al ir en diagonal el playerVelocity mantenga una 
		//magnitud igual a 1 
		playerVelocity.Normalize ();
		playerVelocity *= _speed;
		_rigidbody.velocity = playerVelocity;
	}

	void ManageInput(){
		h = Input.GetAxis ("Horizontal");
		v = Input.GetAxis ("Vertical");
		pressedShoot = Input.GetMouseButton (0);
		releasedShoot = Input.GetMouseButtonUp (0);
	}

	void FaceMouseCursor(){
		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		mousePos.z = 0;
		transform.right =mousePos - transform.position;
	}

	void ChangeWeapon(){
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			_currentBulletPrefab = _bulletPrefab;
		}else if (Input.GetKeyDown(KeyCode.Alpha2)) {
			_currentBulletPrefab = _bulletPrefab2;
		}
	}

	void ShootWeapon(){
		_shootTimer += Time.deltaTime;

		if (pressedShoot) {

			float bulletShootRate = _currentBulletPrefab.GetComponent<Bullet> ().shootRate;

			if (_shootTimer >= bulletShootRate) {
				GameObject bulletClone = Instantiate (_currentBulletPrefab, _bulletSpawn.position, Quaternion.identity);
				bulletClone.transform.up = _bulletSpawn.up;
				_shootTimer = 0;
			}
		}

	}
}
