using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    private float mapSize = 800; //Розмір карти

    private float foodLifetime = 200; // Тривалість життя часточки їжі в секундах
    private float maxFood = 1000; // Максимальна кількість їжі на карті
    private float spawnDelay = 0.2f; // Затримка перед повторним спавном

    private List<GameObject> foodList = new List<GameObject>(); // Список який зберігає всі поточно заспавнені часточки їжі

    [SerializeField] private GameObject food; // Префаб їжі
    private Transform foodCollector; // Об'єкт на сцені який є батьківським для усіх часточок щоб не засирати ієрархію

    private void Start() {
        foodCollector = GameObject.Find("FoodCollector").transform; // Знаходить FoodCollector на сцені
        StartCoroutine(SpawnFood()); 
    }

    IEnumerator SpawnFood() {
        while (true) { // Постійне виконання
            foodList.RemoveAll(food => food == null); // Видаляє усі null елементи зі списку
            /* Пояснення - null елементи теж займають місце та впливають на результат foodList.Count, хоча вже
             * не містять в собі об'єкта, тому перед кожною перевіркою я зачищаю список від цих елементів.
             * Вони утворюються коли елементи зі списку будь-яким чином знищуються, наприклад коли їх з'їдаєш
             * або коли їжа деспавниться. Якщо не зачищати список, то рано чи пізно увесь він буде забитий null об'єктами
             * та їжа більше взагалі не буде спавнитись */

            if (foodList.Count < maxFood) { // Якщо кількість елементів у списку не більші за максимальну
                float randomX = Random.Range(-mapSize, mapSize);
                float randomY = Random.Range(-mapSize, mapSize);

                Vector2 randomPosition = new Vector2(randomX, randomY); 
                Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0, 360)); // Генерація рандомного повороту по Z координаті

                GameObject foodInstance = Instantiate(food, randomPosition, randomRotation, foodCollector); // Спавн та запис об'єкта у змінну
                foodList.Add(foodInstance); // Додавання їжі у список
                 
                StartCoroutine(DestroyAfterDelay(foodInstance, foodLifetime)); // Корутіна видалення
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    IEnumerator DestroyAfterDelay(GameObject foodInstance, float delay) {
        yield return new WaitForSeconds(delay);
        Destroy(foodInstance); // Тут все понятно
    }
}
