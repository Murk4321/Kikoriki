using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollectingAndOrbiting : MonoBehaviour {
    // �������� ����� ����� ��� ������� �������.
    [SerializeField] private float baseOrbitRadius = 2f;

    // �������� ��������� ������� ������� ��'����.
    [SerializeField] private float orbitSpeed = 400f;

    // ����������� ������� �������, �� ������ ���������� ������� ��'����.
    [SerializeField] private int projectileLimit = 15;

    // ���, � ���� ���� ����������� ������ ���� ����, �� ���� �������� (��� 3 �� IgnorePlayer).
    [SerializeField] private int collectedLayer = 3;

    // ������, �� ������ �� �������, �� ����������� ������� ��'����.
    public List<OrbitingObjectData> orbitingObjects = new List<OrbitingObjectData>();

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Projectile")) {
            // ����������, �� � �� ���� ��� �������� �������
            if (orbitingObjects.Count < projectileLimit) {
                // ���� � ����, ������� ������
                CollectProjectile(collision.gameObject);
            }
        }
    }

    private void Update() {
        // ��� ������� ������� � ������ ��������� ���� �����
        for (int i = 0; i < orbitingObjects.Count; i++) {
            UpdateOrbit(orbitingObjects[i], i);
        }
    }

    // ����� ��� ����� �������.
    private void CollectProjectile(GameObject projectile) {
        // ������������ ��� ��'��� ������� ��� ������� �� ������� ���� ���
        projectile.transform.SetParent(transform);
        projectile.gameObject.layer = collectedLayer;

        // ������ ����� ������ �� ������ ������������ ��'����
        float initialAngle = Random.Range(0, 360); // ���������� ���������� ��� ��� ���������
        orbitingObjects.Add(new OrbitingObjectData(projectile.transform, initialAngle));

        // ���� � ������� � ��� ������, ��������� ����� ���� ���� �����
        ProjectileScript projectileScript = projectile.GetComponent<ProjectileScript>();
        if (projectileScript != null) {
            projectileScript.AfterPickup(); // ��������� ���������� �������� ���� �����
        }
    }

    // ����� ��� ��������� ������� � �����.
    public void RemoveOrbitingObject(GameObject obj) {
        // ��������� ��'��� � ������ ������������ ��'���� �� ������� ���� ���������� ���������
        orbitingObjects.RemoveAll(o => o.Object == obj.transform);
        obj.transform.SetParent(null); // ������� ������, ���������� ��'��� �� �������� ����
    }

    // ����� ��� ��������� ����� ��'����.
    private void UpdateOrbit(OrbitingObjectData data, int index) {
        // ���������� ����� ����� ��� ������� �������, ���������� ���� ������
        float radius = baseOrbitRadius + (index * 0.2f); // �������� ����� ����� ��� ������� ���������� �������

        // ��������� ��� ��������� ��� �������
        data.Angle += orbitSpeed * Time.deltaTime; // ������� ��� �� ����� �������� ���������

        // ���������� ���� ������� ������� �� ����� ���� �� ������ 
        float x = transform.position.x + radius * Mathf.Cos(data.Angle * Mathf.Deg2Rad);
        float y = transform.position.y + radius * Mathf.Sin(data.Angle * Mathf.Deg2Rad);

        // ��������� ������� �������
        data.Object.position = new Vector3(x, y, data.Object.position.z);
    }

    // ���� ��� ���������� ���������� ��� ������, �� ����������.
    public class OrbitingObjectData {
        // ��'��� �������, ���� ����������
        public Transform Object { get; private set; }

        // �������� ��� ���������
        public float Angle { get; set; }

        // ����������� ��� ����������� ��'���� ������� �� ���� ����������� ����
        public OrbitingObjectData(Transform obj, float initialAngle) {
            Object = obj;
            Angle = initialAngle;
        }
    }
}
