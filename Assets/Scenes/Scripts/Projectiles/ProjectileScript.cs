using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour {
    public enum ProjectileType {
        Standard,
        Homing
    }

    [Header("Projectile Settings")]
    public float launchForce = 10f;
    public bool canDealDamage = false;
    public ProjectileType type;

    [Header("Homing Settings")]
    [SerializeField] private float homingSpeed = 50f;
    [SerializeField] private float rotationSpeed = 300f;

    [Header("Visuals and Effects")]
    [SerializeField] private GameObject destroyParticles;

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private bool isHoming = false;
    private Transform target;

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        if (destroyParticles == null) {
            Debug.LogWarning("DestroyParticles prefab is not assigned.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (canDealDamage && collision.CompareTag("Enemy")) {
            HitEnemy(collision);
        }
    }

    private void HitEnemy(Collider2D collision) {
        EnemyScript enemy = collision.GetComponent<EnemyScript>();
        if (enemy != null) {
            enemy.Hit();
        }
        DestroyProjectile();
    }

    private void DestroyProjectile() {
        if (destroyParticles != null) {
            // Instantiate particles and match projectile color
            GameObject particlesInstance = Instantiate(destroyParticles, transform.position, Quaternion.identity);
            ParticleSystem particles = particlesInstance.GetComponent<ParticleSystem>();
            if (particles != null) {
                var main = particles.main;
                main.startColor = sr.color;
            }
        }

        Destroy(gameObject);
    }

    public void AfterPickup() {
        // Logic for when the projectile is picked up
    }

    public void AfterLaunch() {
        if (type == ProjectileType.Homing) {
            SetupHomingProjectile();
        }
    }

    private void SetupHomingProjectile() {
        target = FindNearestEnemy();
        if (target != null) {
            isHoming = true;
        }
    }

    private void FixedUpdate() {
        if (isHoming) {
            UpdateHomingMovement();
        }
    }

    private void UpdateHomingMovement() {
        if (target == null) {
            isHoming = false;
            return;
        }

        // Calculate direction and rotate towards the target
        Vector2 direction = ((Vector2)target.position - rb.position).normalized;
        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        rb.angularVelocity = -rotateAmount * rotationSpeed;
        rb.velocity = transform.up * homingSpeed;
    }

    private Transform FindNearestEnemy() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies) {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance) {
                shortestDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }

        return nearestEnemy;
    }
}
