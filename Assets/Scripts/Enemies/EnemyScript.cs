using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {
    // ѕерел≥ченн€ тип≥в ворог≥в
    public enum EnemyType {
        Default // —тандартний тип ворога
    }

    [Header("Enemy Settings")]
    public EnemyType type; // “ип ворога
    [SerializeField] private float speed = 40f; // Ўвидк≥сть руху ворога
    [SerializeField] private GameObject destroyParticles; // „астинки при знищенн≥ ворога

    private Transform player; // ѕосиланн€ на гравц€
    private Rigidbody2D rb; 
    private SpriteRenderer sr;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        // «находимо об'Їкт гравц€ за тегом дл€ над≥йност≥
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null) {
            player = playerObject.transform; // ѕрив'€зуЇмо посиланн€ на гравц€
        } else {
            // якщо гравець не знайдений, виводимо помилку
            Debug.LogError("Player not found! Ensure the player object has the 'Player' tag.");
        }

        // ѕерев≥рка на€вност≥ префабу частинок дл€ знищенн€
        if (destroyParticles == null) {
            Debug.LogWarning("DestroyParticles prefab is not assigned.");
        }
    }

    private void FixedUpdate() {
        if (player != null) {
            MoveTowardsPlayer(); // –ух ворога до гравц€
        }
    }

    // ћетод дл€ руху ворога до гравц€
    private void MoveTowardsPlayer() {
        // ќбчислюЇмо напр€мок до гравц€
        Vector2 direction = ((Vector2)(player.position - transform.position)).normalized;

        // ¬становлюЇмо швидк≥сть руху ворога, множимо напр€мок на швидк≥сть
        rb.AddForce(direction * speed, ForceMode2D.Force);
    }

    // ћетод дл€ обробки з≥ткненн€ ворога (наприклад, при влучанн≥ снар€да)
    public void Hit() {
        // якщо Ї префаб частинок, ≥нстанц≥юЇмо њх на м≥сц≥ ворога
        if (destroyParticles != null) {
            GameObject particlesInstance = Instantiate(destroyParticles, transform.position, Quaternion.identity);
            ParticleSystem particles = particlesInstance.GetComponent<ParticleSystem>();
            if (particles != null) {
                // ¬становлюЇмо кол≥р частинок в≥дпов≥дно до кольору ворога
                var main = particles.main;
                main.startColor = sr.color;
            }
        }

        // «нищуЇмо ворога
        Destroy(gameObject);
    }
}
