using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour {
    // Перелічення для типів снарядів
    public enum ProjectileType {
        Standard, // Стандартний снаряд
        Homing    // Самонаводка
    }

    // Налаштування для снарядів
    [Header("Projectile Settings")]
    public float launchForce = 10f; // Сила запуску снаряда
    public bool canDealDamage = false; // Чи може снаряд завдавати шкоди
    public ProjectileType type; // Тип снаряда (стандартний чи самонаводка)

    // Налаштування для самонаводящих снарядів
    [Header("Homing Settings")]
    [SerializeField] private float homingSpeed = 50f; // Швидкість руху снаряда
    [SerializeField] private float rotationSpeed = 300f; // Швидкість обертання снаряда

    // Візуальні ефекти та ефекти знищення снаряда
    [Header("Visuals and Effects")]
    [SerializeField] private GameObject destroyParticles; // Префаб частинок для знищення снаряда

    private SpriteRenderer sr; // Компонент для управління спрайтами
    private Rigidbody2D rb; // Компонент фізики для снаряда
    private bool isHoming = false; // Чи є снаряд самонаводящімся
    private Transform target; // Ціль для самонаводки

    // Метод, що викликається при ініціалізації об'єкта
    private void Awake() {
        sr = GetComponent<SpriteRenderer>(); // Отримуємо компонент SpriteRenderer
        rb = GetComponent<Rigidbody2D>(); // Отримуємо компонент Rigidbody2D

        // Якщо префаб частинок не призначений, виводимо попередження в консоль
        if (destroyParticles == null) {
            Debug.LogWarning("DestroyParticles prefab is not assigned.");
        }
    }

    // Метод, що викликається при зіткненні снаряда з іншим об'єктом
    private void OnTriggerEnter2D(Collider2D collision) {
        // Якщо снаряд може завдавати шкоди і зіткнувся з ворогом
        if (canDealDamage && collision.CompareTag("Enemy")) {
            HitEnemy(collision); // Викликаємо метод, що обробляє удар по ворогу
        }
    }

    // Метод, що обробляє удар снаряда по ворогу
    private void HitEnemy(Collider2D collision) {
        // Отримуємо скрипт EnemyScript у ворога
        EnemyScript enemy = collision.GetComponent<EnemyScript>();
        if (enemy != null) {
            enemy.Hit(); // Викликаємо метод Hit у ворога, щоб він отримав шкоду
        }
        DestroyProjectile(); // Знищуємо снаряд після попадання
    }

    // Метод для знищення снаряда
    private void DestroyProjectile() {
        // Якщо є префаб частинок для знищення
        if (destroyParticles != null) {
            // Створюємо частинки в місці знищення снаряда
            GameObject particlesInstance = Instantiate(destroyParticles, transform.position, Quaternion.identity);
            ParticleSystem particles = particlesInstance.GetComponent<ParticleSystem>();
            if (particles != null) {
                var main = particles.main; // Отримуємо основні налаштування частинок
                main.startColor = sr.color; // Встановлюємо колір частинок відповідно до кольору снаряда
            }
        }

        // Знищуємо сам снаряд
        Destroy(gameObject);
    }

    // Метод, що викликається після підбирання снаряда (логіка для подальшого використання)
    public void AfterPickup() {
        // Тут можна додати логіку для того, що відбувається після підбирання снаряда
    }

    // Метод, що викликається після запуску снаряда
    public void AfterLaunch() {
        // Якщо снаряд самонаводящіся, налаштовуємо його на пошук цілі
        if (type == ProjectileType.Homing) {
            SetupHomingProjectile();
        }
    }

    private void SetupHomingProjectile() {
        // Знаходимо найближчого ворога, на який снаряд буде націлений
        target = FindNearestEnemy();
        if (target != null) {
            isHoming = true; // Встановлюємо, що снаряд орієнтується на ціль
        }
    }

    // Оновлення стану снаряда кожен кадр
    private void FixedUpdate() {
        // Якщо снаряд націлений на ворога, оновлюємо його рух
        if (isHoming) {
            UpdateHomingMovement();
        }
    }

    private void UpdateHomingMovement() {
        // Якщо немає цілі, зупиняємо рух
        if (target == null) {
            isHoming = false;
            return;
        }

        // Визначаємо напрямок до цілі та обчислюємо кут повороту для снаряда
        Vector2 direction = ((Vector2)target.position - rb.position).normalized;
        float rotateAmount = Vector3.Cross(direction, transform.up).z; // Перехресний добуток для обертання

        // Задаємо швидкість обертання снаряда
        rb.angularVelocity = -rotateAmount * rotationSpeed;
        // Задаємо швидкість руху снаряда в напрямку цілі
        rb.velocity = transform.up * homingSpeed;
    }

    // Метод для пошуку найближчого ворога
    private Transform FindNearestEnemy() {
        // Отримуємо всі об'єкти з тегом "Enemy"
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        // Для кожного ворога в сцені визначаємо відстань і обираємо найближчого
        foreach (GameObject enemy in enemies) {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance) {
                shortestDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }

        // Повертаємо найближчого ворога
        return nearestEnemy;
    }
}
