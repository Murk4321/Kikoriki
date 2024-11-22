using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static ProjectileCollectingAndOrbiting;
using UnityEditor;

public class ProjectileLauncher : MonoBehaviour
{
    private ProjectileCollectingAndOrbiting projectileDatabase;
    private GameObject projectileGameObject;

    private void Awake() {
        projectileDatabase = GetComponent<ProjectileCollectingAndOrbiting>();
    }

    private void Update() {
        ManageLaunching();
    }

    private void ManageLaunching() {
        if (Input.GetMouseButtonDown(0)) {
            LaunchProjectile();
        }
    }

    private void LaunchProjectile() {
        List<OrbitingObjectData> projectileList = projectileDatabase.orbitingObjects;

        if (projectileList.Count == 0) {
            return;
        }

        ProjectileScript projectileScript = GetProjectileFromList(projectileList);
        Rigidbody2D projPb = GetRigidbody();

        projectileScript.canDealDamage = true;
        LaunchTowardsMouse(projPb, projectileScript);
        projectileList.Remove(projectileList.Last());
        Destroy(projectileGameObject, 10);

        projectileScript.AfterLaunch();
    }

    private ProjectileScript GetProjectileFromList(List<OrbitingObjectData> projectileList) {
        projectileGameObject = projectileList.Last().Object.gameObject;
        projectileGameObject.transform.parent = null;
        ProjectileScript projectileScript = projectileGameObject.GetComponent<ProjectileScript>();
        return projectileScript;
    }

    private void LaunchTowardsMouse(Rigidbody2D projPb, ProjectileScript projectile) {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 movePosition = mousePosition - transform.position;
        Vector2 forceDirection = new Vector2(movePosition.x, movePosition.y) * projectile.launchForce;

        projPb.AddForce(forceDirection, ForceMode2D.Impulse);
    }

    private Rigidbody2D GetRigidbody() {
        Rigidbody2D projPb = projectileGameObject.GetComponent<Rigidbody2D>();
        return projPb;
    }
}
