using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWave : MonoBehaviour {
	public GameObject _enemyPrefab;
	public int _count = 5;
	public float _spawnRangeX = 5;
	public float _spawnRangeY = 5;
	public GameObject _nextWave;
	private GameObject[] _spawnedEnemies;
	// Use this for initialization
	void Start () {
		
	}

	void Update(){
		if (_spawnedEnemies != null) {
			bool isWaveDead = true;
			for (int i = 0; i < _count; i++) {
				if (_spawnedEnemies[i] != null) {
					isWaveDead = false;
				}
			}

			if (isWaveDead) {
				if (_nextWave != null) {
					_nextWave.SetActive (true);
				}
				gameObject.SetActive (false);
			}
		}
	}

	void OnTriggerEnter2D (Collider2D other) {
		if(other.tag == "Player"){
			//creamos nuestro arreglo de enemigos creados
			_spawnedEnemies = new GameObject[_count];
			for(int i = 0;i<_count;i++){
				Vector3 randomVector = new Vector3 (
											Random.Range (-_spawnRangeX, _spawnRangeX),
											Random.Range (-_spawnRangeY, _spawnRangeY),
					                      0);
				Vector3 pos = transform.position + randomVector;
				GameObject newZombie = Instantiate(_enemyPrefab,pos,transform.rotation);
				//llenamos nuestro arreglo con las referencias
				//a los zombies que hemos spawneado
				_spawnedEnemies [i] = newZombie;
				GetComponent<Collider2D>().enabled = false;
			}
		}
	}

	public Color _debugBox;

	void OnDrawGizmos(){
		Gizmos.color = _debugBox;
		Vector3 size = new Vector3 (_spawnRangeX*2, _spawnRangeY*2, 0);
		Gizmos.DrawWireCube (transform.position, size);
	}
}
