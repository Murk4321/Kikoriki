using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour {
    // Розмір області, де будуть спавнитися снаряди
    [SerializeField] private float mapSize = 800f; // 800 x 800

    // Максимальна кількість снарядів, яка може існувати одночасно
    [SerializeField] private int maxProjectiles = 1000;

    // Затримка між спавнами снарядів
    [SerializeField] private float spawnDelay = 0.2f; 

    // Початкова кількість снарядів, які будуть спавнитися при старті
    [SerializeField] private int projectilesOnStart = 20;

    // Масив префабів снарядів, з яких будуть вибиратися для спавну
    [SerializeField] private GameObject[] projectiles;

    // Батьківський об'єкт для заспавнених снарядів
    [SerializeField] private Transform projectileCollector;

    // Список активних снарядів
    private readonly List<GameObject> projectileList = new List<GameObject>();

    private void Start() {
        // Перевірка наявності значень у полях
        if (projectileCollector == null) {
            Debug.LogError("ProjectileCollector не призначений у інспекторі!");
            return; // Якщо батьківський об'єкт не задано, вивести помилку та припинити виконання
        }

        if (projectiles == null || projectiles.Length == 0) {
            Debug.LogError("Префаби снарядів не призначені!");
            return; // Якщо масив префабів снарядів пустий, вивести помилку та припинити виконання
        }

        SpawnOnStart(); // Спавнимо початкові снаряди
        StartCoroutine(SpawnProjectileCoroutine()); // Запускаємо корутину для безперервного спавну
    }

    private void SpawnOnStart() {
        // Створюємо кількість снарядів, зазначену в projectilesOnStart
        for (int i = 0; i < projectilesOnStart; i++) {
            SpawnProjectile(); // Спавнимо снаряд
        }
    }

    // Корутіна для безперервного спавну снарядів
    private IEnumerator SpawnProjectileCoroutine() {
        while (true) {
            // Видаляємо всі null-елементи з списку (для снарядів, які були знищені або зібрані)
            projectileList.RemoveAll(projectile => projectile == null);

            // Якщо кількість активних снарядів менше максимально дозволеної, спавнимо новий
            if (projectileList.Count < maxProjectiles) {
                GameObject newProjectile = SpawnProjectile(); // Спавнимо новий снаряд
                projectileList.Add(newProjectile); // Додаємо його в список активних снарядів
            }

            yield return new WaitForSeconds(spawnDelay); // Затримка перед наступним спавном
        }
    }

    private GameObject SpawnProjectile() {
        // Генерація випадкової позиції та обертання для снаряда
        Vector2 randomPosition = new Vector2(
            Random.Range(-mapSize, mapSize), // Випадкове значення для координати X в межах mapSize
            Random.Range(-mapSize, mapSize)  // Випадкове значення для координати Y в межах mapSize
        );
        Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0, 360)); // Випадковий кут для обертання снаряда

        // Вибір снаряда для спавну
        GameObject selectedProjectile = ChooseProjectile(); // Вибір снаряда через метод ChooseProjectile
        GameObject newProjectile = Instantiate(selectedProjectile, randomPosition, randomRotation, projectileCollector); // Спавнимо снаряд з вибраним положенням і обертанням

        return newProjectile; // Повертаємо заспавнений снаряд
    }

    // Метод для вибору снаряда з масиву з ймовірністю
    private GameObject ChooseProjectile() {
        // Перевірка на наявність доступних префабів снарядів
        if (projectiles == null || projectiles.Length == 0) {
            Debug.LogError("Немає доступних снарядів для спавну!");
            return null; // Якщо немає доступних снарядів, вивести помилку та повернути null
        }

        // Випадковий вибір снаряда з ймовірністю
        float randomValue = Random.value; // Генеруємо випадкове значення від 0 до 1
        // Якщо випадкове значення менше за 0.2, вибираємо другий тип снаряда (якщо він є)
        if (randomValue < 0.2f) { // 20% ймовірність для другого типу снаряда
            return projectiles[1];
        } else {  // В іншому випадку вибираємо перший тип снаряда
            return projectiles[0]; 
        }
    }
}
