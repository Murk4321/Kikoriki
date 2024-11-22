using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileScript : MonoBehaviour {
    public enum ProjectileType {
        Standart,
        Homing
    }

    public float launchForce = 10;
    public bool canDealDamage = false;
    public ProjectileType type;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private bool homing = false;
    private Transform target;
    [SerializeField] private GameObject destroyParticles;

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (canDealDamage) {
            if (collision.CompareTag("Enemy")) {
                HitEnemy(collision);
            }
        }
    }

    private void HitEnemy(Collider2D collision) {
        EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript>();
        enemy.Hit();
        DestroyProjectile();
    }

    private void DestroyProjectile() {
        ParticleSystem particles = destroyParticles.GetComponent<ParticleSystem>();
        var main = particles.main;
        main.startColor = sr.color;

        Destroy(gameObject);
        Instantiate(destroyParticles, transform.position, Quaternion.identity);
    }

    public void AfterPickup() {
        
    }

    public void AfterLaunch() {
       if (type == ProjectileType.Homing) {
            ManageHomingProjectile();
       }
    }

    private void ManageHomingProjectile() {
        target = FindNearestEnemy();
        if (target == null) { 
            return;
        }

        homing = true;
    }

    private void FixedUpdate() {
        if (homing) {
            if (target != null) {
                Vector2 direction = (target.position - transform.position).normalized;
                float rotateAmount = Vector3.Cross(direction, transform.up).z;
                transform.Rotate(0, 0, -rotateAmount * 20000 * Time.deltaTime);

                transform.Translate(Vector3.up * 15 * Time.deltaTime);
            }
        }
    }

    private Transform FindNearestEnemy() {
        GameObject nearestEnemy = null;
        float shortestDistance = 1000;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length > 0) {
            foreach (GameObject enemy in enemies) {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < shortestDistance) {
                    shortestDistance = distance;
                    nearestEnemy = enemy;
                }
            }
            
            if (nearestEnemy != null) {
                return nearestEnemy.transform;
            }
        }

        return null;
    }
}
