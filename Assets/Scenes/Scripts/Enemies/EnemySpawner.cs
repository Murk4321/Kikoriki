using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    // ������������ ��� ��������
    [Header("Spawner Settings")]
    [SerializeField] private float mapSize = 800f; // ����� ������ ��� ������ (������� �����)
    [SerializeField] private int maxEnemies = 20; // ����������� ������� ������, �� ������ ���� ��������� �� ����
    [SerializeField] private float spawnDelay = 10f; // �������� �� �������� ����� ������

    // ������������ ��� ������
    [Header("Enemy Settings")]
    [SerializeField] private GameObject[] enemyPrefabs; // ������ ������� ������ ��� ����������

    // ������ ��� �������� ������
    private List<GameObject> enemyList = new List<GameObject>(); // ������ ��� ��������� �������� ������
    private bool isSpawning = true; // �������� �� �������� �������� ������

    // �����, �� ����������� ��� ������� �������
    private void Start() {
        // ��������, �� � ������� ������
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) {
            Debug.LogError("No enemy prefabs assigned to the EnemySpawner!");
            return; // ���� ���� �������, �������� ������� � ���������� ���������
        }

        // ��������� �������� ������ ������
        StartCoroutine(SpawnEnemyCoroutine());
    }

    // ��������, �� ���������� ����������� ��� ������ ������
    private IEnumerator SpawnEnemyCoroutine() {
        while (isSpawning) {
            // ��������� � ��������� ������ ������, �� ���� ������
            enemyList.RemoveAll(enemy => enemy == null);

            // ���� ������� ������ �� ���� ����� �����������, �������� ������
            if (enemyList.Count < maxEnemies) {
                SpawnEnemy();
            }

            // �������� ����� ��������� �������
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    // ����� ��� ������ ������ ������
    private void SpawnEnemy() {
        // ��������� ��������� ������� � ����� �����
        float randomX = Random.Range(-mapSize, mapSize);
        float randomY = Random.Range(-mapSize, mapSize);

        Vector2 randomPosition = new Vector2(randomX, randomY);
        Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0, 360)); // ���������� ��� ��������� ������

        // ���� ����������� ������� ������ � ������
        GameObject chosenEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        // ������������ ������ � ��������� ������� �� ������ ���� � ������
        GameObject enemyInstance = Instantiate(chosenEnemy, randomPosition, randomRotation);
        enemyList.Add(enemyInstance);
    }

    // ����� ��� ������� ������ ������
    public void StopSpawning() {
        isSpawning = false; // ��������� �����
    }

    // ����� ��� ���������� ������ ������
    public void ResumeSpawning() {
        // ���� ����� �� ��������, ��������� ����
        if (!isSpawning) {
            isSpawning = true;
            StartCoroutine(SpawnEnemyCoroutine());
        }
    }
}
