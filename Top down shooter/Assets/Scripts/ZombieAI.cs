using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAI : MonoBehaviour {
	public float _detectRange = 10;
	public float _attackRange = 1;
	public float _speed =2.5f;
	public Collider2D _hitbox;

	//un enum es un tipo de variable que tu puedes crear
	//tu defines cuales son los posibles valores para el enum
	//en este caso, hemos creado un enum llamado ZombieState que puede 
	//tener 3 valores posibles (idle,pursue y attack).
	public enum ZombieStates{
		idle,
		pursue,
		attack
	}
	//aquí creamos una variable del tipo del enum que hicimos
	//recuerda... el enum es un tipo de variable.
	public ZombieStates _state;

	private Rigidbody2D _rigidbody;
	private Animator _animator;
	private Transform _player;
	private Health _healthScript;
	// Use this for initialization
	void Start () {
		_healthScript = GetComponent<Health> ();
		_rigidbody = GetComponent<Rigidbody2D> ();
		_animator = GetComponent<Animator> ();
		_player = GameObject.FindGameObjectWithTag ("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		MakeDecisions ();
	}

	void FixedUpdate(){
		DoAction ();
	}

	void MakeDecisions(){

		float distanceToPlayer = Vector3.Distance (transform.position, _player.position);
		AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo (0);

		if (_state == ZombieStates.idle ) {
			//si lastiman al zombie o el player entra en rango de deteccion
			if (_healthScript._currentHealth < _healthScript._maxHealth ||
				distanceToPlayer <= _detectRange) {
				//comenzamos a perseguir al player
				_state = ZombieStates.pursue;
				_animator.SetTrigger ("move");
			}

		}

		else if (_state == ZombieStates.pursue && distanceToPlayer <= _attackRange &&
			!stateInfo.IsName("Base Layer.zombieAttack")) {
			_state = ZombieStates.attack;
			_rigidbody.velocity = Vector2.zero;
			_rigidbody.isKinematic = true;
			_animator.SetTrigger ("attack");
		}

		else if (_state == ZombieStates.attack) {
			if (stateInfo.IsName("Base Layer.zombieAttack")) {
				if (stateInfo.normalizedTime >= 0.97f) {
					//si despues de atacar el player ya no esta en rango...
					//pasas a perseguirlo
					if (distanceToPlayer > _attackRange) {
						_state = ZombieStates.pursue;
						_rigidbody.isKinematic = false;
						_animator.SetTrigger ("move");
					}
				}
			}

		}
	}

	void DoAction(){
		if (_state == ZombieStates.pursue) {
			Vector3 dirToPlayer = _player.position - transform.position;
			dirToPlayer.Normalize ();
			_rigidbody.velocity = dirToPlayer * _speed;
			transform.right = dirToPlayer;
		}
	}

	public void HitboxOn(){
		_hitbox.enabled = true;
	}

	public void HitBoxOff(){
		_hitbox.enabled = false;
	}

	void OnDrawGizmos(){
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere (transform.position, _detectRange);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere (transform.position, _attackRange);
	}
}
