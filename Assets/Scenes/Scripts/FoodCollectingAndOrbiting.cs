using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCollectingAndOrbiting : MonoBehaviour
{
    public float baseOrbitRadius = 2; // ����� �����
    public float orbitSpeed = 400; // �������� ��������� ��'���� �� ����

    private List<OrbitingObjectData> orbitingObjects = new List<OrbitingObjectData>(); // ������ ����� ��� ��� ��'����, �� �����������.
                                                                                       // ����� ��'��� ���������� ����� �� ����������� ��� ���� �������� ��� ���������.

    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.CompareTag("Food")) {
            collision.transform.SetParent(transform); // ����'������� �� ������
            Destroy(collision.GetComponent<BoxCollider2D>()); // ����������� �������� ��'����, ��� �� ����� �� �������� � ���������� + ����������
            AddOrbitingObject(collision.gameObject); // �������� � ������ �����
        }
    }

   
    void Update() {
        for (int i = 0; i < orbitingObjects.Count; i++) {
            UpdateOrbit(orbitingObjects[i], i); // ��������� ������� ��� ��'���� �� ���� ������
        }
    }

    public void AddOrbitingObject(GameObject obj) { // ���� ��'��� �� ������ ���� �� ���������� �����.
        float initialAngle = Random.Range(0, 360); // ����������� ���������� ���������� ��� ���������
        orbitingObjects.Add(new OrbitingObjectData(obj.transform, initialAngle)); // �������� � ������
    }

    public void RemoveOrbitingObject(GameObject obj) {
            orbitingObjects.RemoveAll(o => o.Object == obj.transform); // ��������� ��'���� � ������ ����
        }

    private void UpdateOrbit(OrbitingObjectData data, int index) { // �������� ���� ������� ��'���� �� ���� ����
        float radius = baseOrbitRadius; // ����������� ����� ����� ��� ����� ��'����
        data.Angle += orbitSpeed * Time.deltaTime; // ���������� ��� ��������� ������� �� �������� � ����.

        // ������������ ���������� x � y ��� ��'���� � ���� ������� (���� ���������� �� � + �������� �� � �� �������� ����)
        float x = transform.position.x + radius * Mathf.Cos(data.Angle * Mathf.Deg2Rad);
        float y = transform.position.y + radius * Mathf.Sin(data.Angle * Mathf.Deg2Rad);
        data.Object.position = new Vector3(x, y, data.Object.position.z);
    }

    private class OrbitingObjectData { // ��� ���� ������ ��� ��� ������� ��'����, �� ����������.
        public Transform Object; 
        public float Angle; 

        public OrbitingObjectData(Transform obj, float initialAngle) { // ��� ������� ��'���� ���������� ���� Transform � �������� ��� ���������.
            Object = obj;
            Angle = initialAngle;
        }
    }
}
