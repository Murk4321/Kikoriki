using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ProjectileCollectingAndOrbiting;

public class ProjectileLauncher : MonoBehaviour {
    private ProjectileCollectingAndOrbiting projectileDatabase; // ������ �� ���� �������, �� ����������� � ����
    private GameObject currentProjectile; // �������� ������, ���� ���� ��������

    private void Awake() {
        projectileDatabase = GetComponent<ProjectileCollectingAndOrbiting>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            LaunchProjectile(); // ������ �������
        }
    }

    // ����� ��� ������� �������
    private void LaunchProjectile() {
        // ����������, �� � ��'����, �� ����������� � ����
        if (projectileDatabase.orbitingObjects.Count == 0) {
            return; // ���� ��'���� ����, �������� � �������
        }

        // �������� ������� ������ � ���� � ��'������ ����
        OrbitingObjectData projectileData = projectileDatabase.orbitingObjects.Last();
        projectileDatabase.RemoveOrbitingObject(projectileData.Object.gameObject);

        currentProjectile = projectileData.Object.gameObject;

        // �������� ���������� Rigidbody2D � ProjectileScript � �������
        Rigidbody2D rb = currentProjectile.GetComponent<Rigidbody2D>();
        ProjectileScript projectileScript = currentProjectile.GetComponent<ProjectileScript>();

        // ���� ���������� �� �������, �������� ������������
        if (rb == null || projectileScript == null) {
            Debug.LogWarning("Projectile is missing required components!");
            return; // ���� ���������� ������, �������� � �������
        }

        // ���������� ������� ��������� �����
        projectileScript.canDealDamage = true;
        LaunchTowardsMouse(rb, projectileScript); // ��������� ������ � �������� ����

        // ������� �������� ������� ����� 10 ������
        Destroy(currentProjectile, 10);

        // ��������� ����-��������� ����� ��� �������
        projectileScript.AfterLaunch();
    }

    // ����� ��� ������� ������� � �������� ����
    private void LaunchTowardsMouse(Rigidbody2D rb, ProjectileScript projectile) {
        // �������� ������� ���� � ���
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // ������� Z � �� ��� �� ���� ���

        // ���������� �������� �� ��'���� �� ������� ����
        Vector3 direction = (mousePosition - transform.position);

        // ����������� ���� �� ������ ��� ���� �������
        rb.AddForce(direction * projectile.launchForce, ForceMode2D.Impulse);
    }
}
