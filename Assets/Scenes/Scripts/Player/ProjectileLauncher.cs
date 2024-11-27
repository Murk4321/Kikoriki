using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ProjectileCollectingAndOrbiting;

public class ProjectileLauncher : MonoBehaviour {
    private ProjectileCollectingAndOrbiting projectileDatabase; // Доступ до бази снарядів, що знаходяться в орбіті
    private GameObject currentProjectile; // Поточний снаряд, який буде запущено

    private void Awake() {
        projectileDatabase = GetComponent<ProjectileCollectingAndOrbiting>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            LaunchProjectile(); // Запуск снаряда
        }
    }

    // Метод для запуску снаряда
    private void LaunchProjectile() {
        // Перевіряємо, чи є об'єкти, що перебувають в орбіті
        if (projectileDatabase.orbitingObjects.Count == 0) {
            return; // Якщо об'єктів немає, виходимо з функції
        }

        // Отримуємо останній снаряд в орбіті і від'єднуємо його
        OrbitingObjectData projectileData = projectileDatabase.orbitingObjects.Last();
        projectileDatabase.RemoveOrbitingObject(projectileData.Object.gameObject);

        currentProjectile = projectileData.Object.gameObject;

        // Отримуємо компоненти Rigidbody2D і ProjectileScript з снаряда
        Rigidbody2D rb = currentProjectile.GetComponent<Rigidbody2D>();
        ProjectileScript projectileScript = currentProjectile.GetComponent<ProjectileScript>();

        // Якщо компоненти не знайдені, виводимо попередження
        if (rb == null || projectileScript == null) {
            Debug.LogWarning("Projectile is missing required components!");
            return; // Якщо компоненти відсутні, виходимо з функції
        }

        // Дозволяємо снаряду завдавати шкоди
        projectileScript.canDealDamage = true;
        LaunchTowardsMouse(rb, projectileScript); // Запускаємо снаряд у напрямку миші

        // Плануємо знищення снаряда через 10 секунд
        Destroy(currentProjectile, 10);

        // Викликаємо пост-запускову логіку для снаряда
        projectileScript.AfterLaunch();
    }

    // Метод для запуску снаряда в напрямку миші
    private void LaunchTowardsMouse(Rigidbody2D rb, ProjectileScript projectile) {
        // Отримуємо позицію миші у світі
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Удаляєм Z а то все по пизді піде

        // Обчислюємо напрямок від об'єкта до позиції миші
        Vector3 direction = (mousePosition - transform.position);

        // Застосовуємо силу на снаряд для його запуску
        rb.AddForce(direction * projectile.launchForce, ForceMode2D.Impulse);
    }
}
