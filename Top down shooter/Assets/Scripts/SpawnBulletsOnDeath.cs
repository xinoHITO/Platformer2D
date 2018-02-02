using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBulletsOnDeath : MonoBehaviour {
	public GameObject _bulletPrefab;
	public int _bulletCount = 4;
	void OnDestroy () {
		for (int i = 0; i < _bulletCount; i++) {
			GameObject spawnedBullet = Instantiate (_bulletPrefab, transform.position, transform.rotation);
			spawnedBullet.transform.up = Random.insideUnitCircle;
		}
	}

}
