using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ProjectileCollectingAndOrbiting;

public class ProjectileLauncher : MonoBehaviour {
    private ProjectileCollectingAndOrbiting projectileDatabase;
    private GameObject currentProjectile;

    private void Awake() {
        projectileDatabase = GetComponent<ProjectileCollectingAndOrbiting>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            LaunchProjectile();
        }
    }

    private void LaunchProjectile() {
        if (projectileDatabase.orbitingObjects.Count == 0) {
            return;
        }

        // Get the last projectile and detach it
        OrbitingObjectData projectileData = projectileDatabase.orbitingObjects.Last();
        projectileDatabase.RemoveOrbitingObject(projectileData.Object.gameObject);

        currentProjectile = projectileData.Object.gameObject;

        // Get required components
        Rigidbody2D rb = currentProjectile.GetComponent<Rigidbody2D>();
        ProjectileScript projectileScript = currentProjectile.GetComponent<ProjectileScript>();

        if (rb == null || projectileScript == null) {
            Debug.LogWarning("Projectile is missing required components!");
            return;
        }

        // Enable damage and launch
        projectileScript.canDealDamage = true;
        LaunchTowardsMouse(rb, projectileScript);

        // Schedule destruction after a delay
        Destroy(currentProjectile, 10);

        // Trigger post-launch logic
        projectileScript.AfterLaunch();
    }

    private void LaunchTowardsMouse(Rigidbody2D rb, ProjectileScript projectile) {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; // Ensure we are working in 2D space
        Vector3 direction = (mousePosition - transform.position);

        // Apply force based on projectile's launch force
        rb.AddForce(direction * projectile.launchForce, ForceMode2D.Impulse);
    }
}
