using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    // Налаштування для спавнера
    [Header("Spawner Settings")]
    [SerializeField] private float mapSize = 800f; // Розмір області для спавна (границі карти)
    [SerializeField] private int maxEnemies = 20; // Максимальна кількість ворогів, які можуть бути одночасно на карті
    [SerializeField] private float spawnDelay = 10f; // Затримка між спавнами нових ворогів

    // Налаштування для ворогів
    [Header("Enemy Settings")]
    [SerializeField] private GameObject[] enemyPrefabs; // Массив префабів ворогів для різноманіття

    // Список для активних ворогів
    private List<GameObject> enemyList = new List<GameObject>(); // Список для зберігання активних ворогів
    private bool isSpawning = true; // Контроль за запуском корутини спавна

    // Метод, що викликається при запуску скрипта
    private void Start() {
        // Перевірка, чи є префаби ворогів
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) {
            Debug.LogError("No enemy prefabs assigned to the EnemySpawner!");
            return; // Якщо немає префабів, виводимо помилку і припиняємо виконання
        }

        // Запускаємо корутину спавна ворогів
        StartCoroutine(SpawnEnemyCoroutine());
    }

    // Корутина, що виконується безперервно для спавна ворогів
    private IEnumerator SpawnEnemyCoroutine() {
        while (isSpawning) {
            // Видаляємо з активного списку ворогів, які були знищені
            enemyList.RemoveAll(enemy => enemy == null);

            // Якщо кількість ворогів на карті менше максимальної, спавнимо нового
            if (enemyList.Count < maxEnemies) {
                SpawnEnemy();
            }

            // Затримка перед наступним спавном
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    // Метод для спауна нового ворога
    private void SpawnEnemy() {
        // Генерація випадкової позиції в межах карти
        float randomX = Random.Range(-mapSize, mapSize);
        float randomY = Random.Range(-mapSize, mapSize);

        Vector2 randomPosition = new Vector2(randomX, randomY);
        Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0, 360)); // Випадковий кут обертання ворога

        // Вибір випадкового префабу ворога зі списку
        GameObject chosenEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        // Інстанціюємо ворога в випадковій позиції та додамо його в список
        GameObject enemyInstance = Instantiate(chosenEnemy, randomPosition, randomRotation);
        enemyList.Add(enemyInstance);
    }

    // Метод для зупинки спауна ворогів
    public void StopSpawning() {
        isSpawning = false; // Зупиняємо спаун
    }

    // Метод для відновлення спауна ворогів
    public void ResumeSpawning() {
        // Якщо спаун не активний, запускаємо його
        if (!isSpawning) {
            isSpawning = true;
            StartCoroutine(SpawnEnemyCoroutine());
        }
    }
}
