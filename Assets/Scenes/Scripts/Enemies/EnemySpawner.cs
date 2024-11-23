using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [Header("Spawner Settings")]
    [SerializeField] private float mapSize = 800f; // Size of the spawn area
    [SerializeField] private int maxEnemies = 20; // Maximum number of enemies at a time
    [SerializeField] private float spawnDelay = 10f; // Delay between spawns

    [Header("Enemy Settings")]
    [SerializeField] private GameObject[] enemyPrefabs; // Array of enemy prefabs for variety

    private List<GameObject> enemyList = new List<GameObject>(); // List of active enemies
    private bool isSpawning = true; // Control for spawning coroutine

    private void Start() {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) {
            Debug.LogError("No enemy prefabs assigned to the EnemySpawner!");
            return;
        }

        StartCoroutine(SpawnEnemyCoroutine());
    }

    private IEnumerator SpawnEnemyCoroutine() {
        while (isSpawning) {
            // Remove destroyed enemies from the list
            enemyList.RemoveAll(enemy => enemy == null);

            // Spawn new enemies if under the limit
            if (enemyList.Count < maxEnemies) {
                SpawnEnemy();
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void SpawnEnemy() {
        // Generate random position and rotation within map bounds
        float randomX = Random.Range(-mapSize, mapSize);
        float randomY = Random.Range(-mapSize, mapSize);

        Vector2 randomPosition = new Vector2(randomX, randomY);
        Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));

        // Choose a random enemy prefab
        GameObject chosenEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        // Instantiate enemy and add to the list
        GameObject enemyInstance = Instantiate(chosenEnemy, randomPosition, randomRotation);
        enemyList.Add(enemyInstance);
    }

    public void StopSpawning() {
        isSpawning = false;
    }

    public void ResumeSpawning() {
        if (!isSpawning) {
            isSpawning = true;
            StartCoroutine(SpawnEnemyCoroutine());
        }
    }
}
