using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollectingAndOrbiting : MonoBehaviour {
    [SerializeField] private float baseOrbitRadius = 2f; // Base radius of the orbit
    [SerializeField] private float orbitSpeed = 400f; // Speed of the orbit
    [SerializeField] private int projectileLimit = 15; // Max number of orbiting objects
    [SerializeField] private int collectedLayer = 3; // Layer for collected projectiles

    public List<OrbitingObjectData> orbitingObjects = new List<OrbitingObjectData>();

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Projectile")) {
            if (orbitingObjects.Count < projectileLimit) {
                CollectProjectile(collision.gameObject);
            }
        }
    }

    private void Update() {
        for (int i = 0; i < orbitingObjects.Count; i++) {
            UpdateOrbit(orbitingObjects[i], i);
        }
    }

    private void CollectProjectile(GameObject projectile) {
        // Set parent to this object and change layer
        projectile.transform.SetParent(transform);
        projectile.gameObject.layer = collectedLayer;

        // Add to the orbiting objects
        float initialAngle = Random.Range(0, 360); // Randomize initial angle
        orbitingObjects.Add(new OrbitingObjectData(projectile.transform, initialAngle));

        // Trigger the projectile's specific pickup behavior
        ProjectileScript projectileScript = projectile.GetComponent<ProjectileScript>();
        if (projectileScript != null) {
            projectileScript.AfterPickup();
        }
    }

    public void RemoveOrbitingObject(GameObject obj) {
        // Remove the object from the list and reset its parent
        orbitingObjects.RemoveAll(o => o.Object == obj.transform);
        obj.transform.SetParent(null);
    }

    private void UpdateOrbit(OrbitingObjectData data, int index) {
        // Optional: Scale radius based on the number of objects
        float radius = baseOrbitRadius + (index * 0.2f);

        // Update the angle for orbiting
        data.Angle += orbitSpeed * Time.deltaTime;

        // Calculate new position
        float x = transform.position.x + radius * Mathf.Cos(data.Angle * Mathf.Deg2Rad);
        float y = transform.position.y + radius * Mathf.Sin(data.Angle * Mathf.Deg2Rad);
        data.Object.position = new Vector3(x, y, data.Object.position.z);
    }

    public class OrbitingObjectData {
        public Transform Object { get; private set; }
        public float Angle { get; set; }

        public OrbitingObjectData(Transform obj, float initialAngle) {
            Object = obj;
            Angle = initialAngle;
        }
    }
}
