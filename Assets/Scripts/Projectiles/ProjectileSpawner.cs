using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour {
    // ����� ������, �� ������ ���������� �������
    [SerializeField] private float mapSize = 800f; // 800 x 800

    // ����������� ������� �������, ��� ���� �������� ���������
    [SerializeField] private int maxProjectiles = 1000;

    // �������� �� �������� �������
    [SerializeField] private float spawnDelay = 0.2f; 

    // ��������� ������� �������, �� ������ ���������� ��� �����
    [SerializeField] private int projectilesOnStart = 20;

    // ����� ������� �������, � ���� ������ ���������� ��� ������
    [SerializeField] private GameObject[] projectiles;

    // ����������� ��'��� ��� ����������� �������
    [SerializeField] private Transform projectileCollector;

    // ������ �������� �������
    private readonly List<GameObject> projectileList = new List<GameObject>();

    private void Start() {
        // �������� �������� ������� � �����
        if (projectileCollector == null) {
            Debug.LogError("ProjectileCollector �� ����������� � ���������!");
            return; // ���� ����������� ��'��� �� ������, ������� ������� �� ��������� ���������
        }

        if (projectiles == null || projectiles.Length == 0) {
            Debug.LogError("������� ������� �� ���������!");
            return; // ���� ����� ������� ������� ������, ������� ������� �� ��������� ���������
        }

        SpawnOnStart(); // �������� �������� �������
        StartCoroutine(SpawnProjectileCoroutine()); // ��������� �������� ��� ������������� ������
    }

    private void SpawnOnStart() {
        // ��������� ������� �������, ��������� � projectilesOnStart
        for (int i = 0; i < projectilesOnStart; i++) {
            SpawnProjectile(); // �������� ������
        }
    }

    // ������� ��� ������������� ������ �������
    private IEnumerator SpawnProjectileCoroutine() {
        while (true) {
            // ��������� �� null-�������� � ������ (��� �������, �� ���� ������ ��� �����)
            projectileList.RemoveAll(projectile => projectile == null);

            // ���� ������� �������� ������� ����� ����������� ���������, �������� �����
            if (projectileList.Count < maxProjectiles) {
                GameObject newProjectile = SpawnProjectile(); // �������� ����� ������
                projectileList.Add(newProjectile); // ������ ���� � ������ �������� �������
            }

            yield return new WaitForSeconds(spawnDelay); // �������� ����� ��������� �������
        }
    }

    private GameObject SpawnProjectile() {
        // ��������� ��������� ������� �� ��������� ��� �������
        Vector2 randomPosition = new Vector2(
            Random.Range(-mapSize, mapSize), // ��������� �������� ��� ���������� X � ����� mapSize
            Random.Range(-mapSize, mapSize)  // ��������� �������� ��� ���������� Y � ����� mapSize
        );
        Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0, 360)); // ���������� ��� ��� ��������� �������

        // ���� ������� ��� ������
        GameObject selectedProjectile = ChooseProjectile(); // ���� ������� ����� ����� ChooseProjectile
        GameObject newProjectile = Instantiate(selectedProjectile, randomPosition, randomRotation, projectileCollector); // �������� ������ � �������� ���������� � ����������

        return newProjectile; // ��������� ����������� ������
    }

    // ����� ��� ������ ������� � ������ � ���������
    private GameObject ChooseProjectile() {
        // �������� �� �������� ��������� ������� �������
        if (projectiles == null || projectiles.Length == 0) {
            Debug.LogError("���� ��������� ������� ��� ������!");
            return null; // ���� ���� ��������� �������, ������� ������� �� ��������� null
        }

        // ���������� ���� ������� � ���������
        float randomValue = Random.value; // �������� ��������� �������� �� 0 �� 1
        // ���� ��������� �������� ����� �� 0.2, �������� ������ ��� ������� (���� �� �)
        if (randomValue < 0.2f) { // 20% ��������� ��� ������� ���� �������
            return projectiles[1];
        } else {  // � ������ ������� �������� ������ ��� �������
            return projectiles[0]; 
        }
    }
}
