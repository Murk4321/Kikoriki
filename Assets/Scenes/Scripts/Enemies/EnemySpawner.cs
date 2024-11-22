using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    private float mapSize = 800;

    private float maxEnemies = 20;
    private float spawnDelay = 10f;

    public List<GameObject> enemyList = new List<GameObject>();

    [SerializeField] private GameObject enemy;

    private void Start() {
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy() {
        while (true) {
            enemyList.RemoveAll(enemy => enemy == null);

            if (enemyList.Count < maxEnemies) {
                float randomX = Random.Range(-mapSize, mapSize);
                float randomY = Random.Range(-mapSize, mapSize);

                Vector2 randomPosition = new Vector2(randomX, randomY);
                Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));

                GameObject enemyInstance = Instantiate(enemy, randomPosition, randomRotation);
                enemyList.Add(enemyInstance);
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
