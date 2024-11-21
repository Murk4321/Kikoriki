using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    private float mapSize = 800; 
    private float maxProjectiles = 1000; 
    private float spawnDelay = 0.2f;
    private int projectilesOnStart = 20;

    private List<GameObject> projectileList = new List<GameObject>(); 

    [SerializeField] private GameObject projectile; 
    private Transform projectileCollector; 

    private void Start() {
        projectileCollector = GameObject.Find("ProjectileCollector").transform;

        SpawnOnStart();

        StartCoroutine(SpawnProjectile());
    }

    private void SpawnOnStart() {
        for (int i = 0; i < projectilesOnStart; i++) {
            Spawn();
        }
    }

    IEnumerator SpawnProjectile() {
        while (true) {
            projectileList.RemoveAll(projectile => projectile == null); 

            if (projectileList.Count < maxProjectiles) {
                GameObject projectileInstance = Spawn();
                projectileList.Add(projectileInstance);
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private GameObject Spawn() {
        float randomX = Random.Range(-mapSize, mapSize);
        float randomY = Random.Range(-mapSize, mapSize);

        Vector2 randomPosition = new Vector2(randomX, randomY);
        Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));

        GameObject projectileInstance = Instantiate(projectile, randomPosition, randomRotation, projectileCollector);
        return projectileInstance;
    }

}
