using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    private float mapSize = 800; //����� �����

    private float foodLifetime = 200; // ��������� ����� �������� �� � ��������
    private float maxFood = 1000; // ����������� ������� �� �� ����
    private float spawnDelay = 0.2f; // �������� ����� ��������� �������

    private List<GameObject> foodList = new List<GameObject>(); // ������ ���� ������ �� ������� ��������� �������� ��

    [SerializeField] private GameObject food; // ������ ��
    private Transform foodCollector; // ��'��� �� ���� ���� � ����������� ��� ��� �������� ��� �� �������� ��������

    private void Start() {
        foodCollector = GameObject.Find("FoodCollector").transform; // ��������� FoodCollector �� ����
        StartCoroutine(SpawnFood()); 
    }

    IEnumerator SpawnFood() {
        while (true) { // ������� ���������
            foodList.RemoveAll(food => food == null); // ������� �� null �������� � ������
            /* ��������� - null �������� ��� �������� ���� �� ��������� �� ��������� foodList.Count, ���� ���
             * �� ������ � ��� ��'����, ���� ����� ������ ��������� � ������� ������ �� ��� ��������.
             * ���� ����������� ���� �������� � ������ ����-���� ����� ����������, ��������� ���� �� �'����
             * ��� ���� ��� ������������. ���� �� �������� ������, �� ���� �� ���� ����� �� ���� ������� null ��'������
             * �� ��� ����� ������ �� ���� ���������� */

            if (foodList.Count < maxFood) { // ���� ������� �������� � ������ �� ����� �� �����������
                float randomX = Random.Range(-mapSize, mapSize);
                float randomY = Random.Range(-mapSize, mapSize);

                Vector2 randomPosition = new Vector2(randomX, randomY); 
                Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0, 360)); // ��������� ���������� �������� �� Z ���������

                GameObject foodInstance = Instantiate(food, randomPosition, randomRotation, foodCollector); // ����� �� ����� ��'���� � �����
                foodList.Add(foodInstance); // ��������� �� � ������
                 
                StartCoroutine(DestroyAfterDelay(foodInstance, foodLifetime)); // ������� ���������
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    IEnumerator DestroyAfterDelay(GameObject foodInstance, float delay) {
        yield return new WaitForSeconds(delay);
        Destroy(foodInstance); // ��� ��� �������
    }
}
