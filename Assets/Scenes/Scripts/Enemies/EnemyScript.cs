using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {
    public enum EnemyType {
        Default
    }

    public EnemyType type;
    private Transform player;
    private float speed = 40;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    [SerializeField] private GameObject destroyParticles;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").transform;
        sr = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate() {
        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer() {
        Vector3 direction = (player.position - transform.position).normalized * speed;
        rb.AddForce(direction, ForceMode2D.Force);
    }

    public void Hit() {
        ParticleSystem particles = destroyParticles.GetComponent<ParticleSystem>();
        var main = particles.main;
        main.startColor = sr.color;

        Destroy(gameObject);
        Instantiate(destroyParticles, transform.position, Quaternion.identity);
    }
}
