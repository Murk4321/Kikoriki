using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {
    public enum EnemyType {
        Default
    }

    [Header("Enemy Settings")]
    public EnemyType type;
    [SerializeField] private float speed = 40f; // Movement speed
    [SerializeField] private GameObject destroyParticles; // Particles on destruction

    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        // Find player by tag for robustness
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null) {
            player = playerObject.transform;
        } else {
            Debug.LogError("Player not found! Ensure the player object has the 'Player' tag.");
        }

        // Validate destroyParticles assignment
        if (destroyParticles == null) {
            Debug.LogWarning("DestroyParticles prefab is not assigned.");
        }
    }

    private void FixedUpdate() {
        if (player != null) {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer() {
        // Calculate direction towards the player
        Vector2 direction = ((Vector2)(player.position - transform.position)).normalized;

        // Set velocity directly for consistent movement
        rb.velocity = direction * speed;
    }

    public void Hit() {
        // Instantiate particles if assigned
        if (destroyParticles != null) {
            GameObject particlesInstance = Instantiate(destroyParticles, transform.position, Quaternion.identity);
            ParticleSystem particles = particlesInstance.GetComponent<ParticleSystem>();
            if (particles != null) {
                // Match particle color to enemy sprite
                var main = particles.main;
                main.startColor = sr.color;
            }
        }

        Destroy(gameObject); // Destroy the enemy
    }
}
