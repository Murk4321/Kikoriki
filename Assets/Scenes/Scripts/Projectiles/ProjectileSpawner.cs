using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour {
    [SerializeField] private float mapSize = 800f; // Size of the spawning area
    [SerializeField] private int maxProjectiles = 1000; // Max projectiles allowed
    [SerializeField] private float spawnDelay = 0.2f; // Delay between spawns
    [SerializeField] private int projectilesOnStart = 20; // Initial number of projectiles
    [SerializeField] private GameObject[] projectiles; // Array of projectile prefabs
    [SerializeField] private Transform projectileCollector; // Parent for spawned projectiles

    private readonly List<GameObject> projectileList = new List<GameObject>(); // Active projectiles list

    private void Start() {
        // Validate inputs
        if (projectileCollector == null) {
            Debug.LogError("ProjectileCollector is not assigned in the inspector!");
            return;
        }

        if (projectiles == null || projectiles.Length == 0) {
            Debug.LogError("Projectile prefabs are not assigned!");
            return;
        }

        SpawnOnStart(); // Spawn initial projectiles
        StartCoroutine(SpawnProjectileCoroutine()); // Start spawning loop
    }

    private void SpawnOnStart() {
        for (int i = 0; i < projectilesOnStart; i++) {
            SpawnProjectile();
        }
    }

    private IEnumerator SpawnProjectileCoroutine() {
        while (true) {
            // Remove null references (for projectiles destroyed or collected)
            projectileList.RemoveAll(projectile => projectile == null);

            // Spawn new projectile if below limit
            if (projectileList.Count < maxProjectiles) {
                GameObject newProjectile = SpawnProjectile();
                projectileList.Add(newProjectile);
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private GameObject SpawnProjectile() {
        // Generate random position and rotation
        Vector2 randomPosition = new Vector2(
            Random.Range(-mapSize, mapSize),
            Random.Range(-mapSize, mapSize)
        );
        Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));

        // Choose projectile and instantiate
        GameObject selectedProjectile = ChooseProjectile();
        GameObject newProjectile = Instantiate(selectedProjectile, randomPosition, randomRotation, projectileCollector);

        return newProjectile;
    }

    private GameObject ChooseProjectile() {
        // Ensure the projectile array is valid
        if (projectiles == null || projectiles.Length == 0) {
            Debug.LogError("No projectiles available to spawn!");
            return null;
        }

        // Randomly choose a projectile with weighted probability
        float randomValue = Random.value;
        return randomValue < 0.2f && projectiles.Length > 1
            ? projectiles[1] // 20% chance for second projectile type
            : projectiles[0]; // Default to first projectile type
    }
}
