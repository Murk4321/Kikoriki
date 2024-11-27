using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollectingAndOrbiting : MonoBehaviour {
    // Основний радіус орбіти для зібраних снарядів.
    [SerializeField] private float baseOrbitRadius = 2f;

    // Швидкість обертання снарядів навколо об'єкта.
    [SerializeField] private float orbitSpeed = 400f;

    // Максимальна кількість снарядів, що можуть обертатися навколо об'єкта.
    [SerializeField] private int projectileLimit = 15;

    // Шар, в який буде переведений снаряд після того, як його заберуть (шар 3 це IgnorePlayer).
    [SerializeField] private int collectedLayer = 3;

    // Список, що зберігає всі снаряди, які обертаються навколо об'єкта.
    public List<OrbitingObjectData> orbitingObjects = new List<OrbitingObjectData>();

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Projectile")) {
            // Перевіряємо, чи є ще місце для збирання снарядів
            if (orbitingObjects.Count < projectileLimit) {
                // Якщо є місце, збираємо снаряд
                CollectProjectile(collision.gameObject);
            }
        }
    }

    private void Update() {
        // Для кожного снаряда в списку оновлюємо його орбіту
        for (int i = 0; i < orbitingObjects.Count; i++) {
            UpdateOrbit(orbitingObjects[i], i);
        }
    }

    // Метод для збору снаряда.
    private void CollectProjectile(GameObject projectile) {
        // Встановлюємо цей об'єкт батьком для снаряда та змінюємо його шар
        projectile.transform.SetParent(transform);
        projectile.gameObject.layer = collectedLayer;

        // Додаємо новий снаряд до списку обертаючихся об'єктів
        float initialAngle = Random.Range(0, 360); // Випадковий початковий кут для обертання
        orbitingObjects.Add(new OrbitingObjectData(projectile.transform, initialAngle));

        // Якщо у снаряда є свій скрипт, викликаємо метод після його збору
        ProjectileScript projectileScript = projectile.GetComponent<ProjectileScript>();
        if (projectileScript != null) {
            projectileScript.AfterPickup(); // Викликаємо спеціальну поведінку після збору
        }
    }

    // Метод для видалення снаряда з орбіти.
    public void RemoveOrbitingObject(GameObject obj) {
        // Видаляємо об'єкт зі списку обертаючихся об'єктів та скидаємо його батьківське посилання
        orbitingObjects.RemoveAll(o => o.Object == obj.transform);
        obj.transform.SetParent(null); // Скидаємо батька, повертаючи об'єкт на попереднє місце
    }

    // Метод для оновлення орбіти об'єкта.
    private void UpdateOrbit(OrbitingObjectData data, int index) {
        // Обчислюємо радіус орбіти для кожного снаряда, враховуючи його індекс
        float radius = baseOrbitRadius + (index * 0.2f); // Збільшуємо радіус орбіти для кожного наступного снаряда

        // Оновлюємо кут обертання для снаряда
        data.Angle += orbitSpeed * Time.deltaTime; // Змінюємо кут на основі швидкості обертання

        // Обчислюємо нову позицію снаряда на основі кута та радіусу 
        float x = transform.position.x + radius * Mathf.Cos(data.Angle * Mathf.Deg2Rad);
        float y = transform.position.y + radius * Mathf.Sin(data.Angle * Mathf.Deg2Rad);

        // Оновлюємо позицію снаряда
        data.Object.position = new Vector3(x, y, data.Object.position.z);
    }

    // Клас для збереження інформації про снаряд, що обертається.
    public class OrbitingObjectData {
        // Об'єкт снаряда, який обертається
        public Transform Object { get; private set; }

        // Поточний кут обертання
        public float Angle { get; set; }

        // Конструктор для ініціалізації об'єкта снаряда та його початкового кута
        public OrbitingObjectData(Transform obj, float initialAngle) {
            Object = obj;
            Angle = initialAngle;
        }
    }
}
