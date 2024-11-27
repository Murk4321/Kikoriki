using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {
    // ���������� ���� ������
    public enum EnemyType {
        Default // ����������� ��� ������
    }

    [Header("Enemy Settings")]
    public EnemyType type; // ��� ������
    [SerializeField] private float speed = 40f; // �������� ���� ������
    [SerializeField] private GameObject destroyParticles; // �������� ��� ������� ������

    private Transform player; // ��������� �� ������
    private Rigidbody2D rb; 
    private SpriteRenderer sr;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        // ��������� ��'��� ������ �� ����� ��� ��������
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null) {
            player = playerObject.transform; // ����'����� ��������� �� ������
        } else {
            // ���� ������� �� ���������, �������� �������
            Debug.LogError("Player not found! Ensure the player object has the 'Player' tag.");
        }

        // �������� �������� ������� �������� ��� ��������
        if (destroyParticles == null) {
            Debug.LogWarning("DestroyParticles prefab is not assigned.");
        }
    }

    private void FixedUpdate() {
        if (player != null) {
            MoveTowardsPlayer(); // ��� ������ �� ������
        }
    }

    // ����� ��� ���� ������ �� ������
    private void MoveTowardsPlayer() {
        // ���������� �������� �� ������
        Vector2 direction = ((Vector2)(player.position - transform.position)).normalized;

        // ������������ �������� ���� ������, ������� �������� �� ��������
        rb.AddForce(direction * speed, ForceMode2D.Force);
    }

    // ����� ��� ������� �������� ������ (���������, ��� ������� �������)
    public void Hit() {
        // ���� � ������ ��������, ������������ �� �� ���� ������
        if (destroyParticles != null) {
            GameObject particlesInstance = Instantiate(destroyParticles, transform.position, Quaternion.identity);
            ParticleSystem particles = particlesInstance.GetComponent<ParticleSystem>();
            if (particles != null) {
                // ������������ ���� �������� �������� �� ������� ������
                var main = particles.main;
                main.startColor = sr.color;
            }
        }

        // ������� ������
        Destroy(gameObject);
    }
}
