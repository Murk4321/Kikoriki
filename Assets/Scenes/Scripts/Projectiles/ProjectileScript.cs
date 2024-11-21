using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileScript : MonoBehaviour {
    public enum ProjectileType {
        Standart
    }

    public float launchForce = 10;
    public bool canDealDamage = false;
    public ProjectileType type;
    private SpriteRenderer sr;
    [SerializeField] private GameObject destroyParticles;

    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
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
}
