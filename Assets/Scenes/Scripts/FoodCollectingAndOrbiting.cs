using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCollectingAndOrbiting : MonoBehaviour
{
    public float baseOrbitRadius = 2; // Радіус орбіти
    public float orbitSpeed = 400; // Швидкість обертання об'єктів по орбіті

    private List<OrbitingObjectData> orbitingObjects = new List<OrbitingObjectData>(); // Список даних для всіх об'єктів, що обертаються.
                                                                                       // Кожен об'єкт зберігається разом із інформацією про його поточний кут обертання.

    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.CompareTag("Food")) {
            collision.transform.SetParent(transform); // Прив'язується до гравця
            Destroy(collision.GetComponent<BoxCollider2D>()); // Видаляється колайдер об'єкта, щоб він більше не взаємодіяв з персонажем + оптимізація
            AddOrbitingObject(collision.gameObject); // Додається в список орбіти
        }
    }

   
    void Update() {
        for (int i = 0; i < orbitingObjects.Count; i++) {
            UpdateOrbit(orbitingObjects[i], i); // Оновлення позицій усіх об'єктів на їхніх орбітах
        }
    }

    public void AddOrbitingObject(GameObject obj) { // Додає об'єкт до списку орбіт із початковим кутом.
        float initialAngle = Random.Range(0, 360); // Створюється випадковий початковий кут обертання
        orbitingObjects.Add(new OrbitingObjectData(obj.transform, initialAngle)); // Додається в список
    }

    public void RemoveOrbitingObject(GameObject obj) {
            orbitingObjects.RemoveAll(o => o.Object == obj.transform); // Видалення об'єкта зі списку орбіт
        }

    private void UpdateOrbit(OrbitingObjectData data, int index) { // Обчислює нову позицію об'єкта на його орбіті
        float radius = baseOrbitRadius; // Визначається радіус орбіти для цього об'єкта
        data.Angle += orbitSpeed * Time.deltaTime; // Збільшується кут обертання залежно від швидкості і часу.

        // Обчислюються координати x і y для об'єкта в новій позиції (якщо косинусоїда по Х + синусоіда по У то виходить коло)
        float x = transform.position.x + radius * Mathf.Cos(data.Angle * Mathf.Deg2Rad);
        float y = transform.position.y + radius * Mathf.Sin(data.Angle * Mathf.Deg2Rad);
        data.Object.position = new Vector3(x, y, data.Object.position.z);
    }

    private class OrbitingObjectData { // Цей клас зберігає дані для кожного об'єкта, що обертається.
        public Transform Object; 
        public float Angle; 

        public OrbitingObjectData(Transform obj, float initialAngle) { // Для кожного об'єкта зберігається його Transform і поточний кут обертання.
            Object = obj;
            Angle = initialAngle;
        }
    }
}
