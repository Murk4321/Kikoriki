using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollectingAndOrbiting : MonoBehaviour
{
    public float baseOrbitRadius = 2; 
    public float orbitSpeed = 400;
    private int projectileLimit = 15;

    public List<OrbitingObjectData> orbitingObjects = new List<OrbitingObjectData>(); 

    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.CompareTag("Projectile")) {
            if (orbitingObjects.Count < projectileLimit) {
                collision.transform.SetParent(transform);
                collision.gameObject.layer = 3;
                AddOrbitingObject(collision.gameObject);
            }
        }
    }
   
    void Update() {
        for (int i = 0; i < orbitingObjects.Count; i++) {
            UpdateOrbit(orbitingObjects[i], i); 
        }
    }

    public void AddOrbitingObject(GameObject obj) { 
        float initialAngle = Random.Range(0, 360); 
        orbitingObjects.Add(new OrbitingObjectData(obj.transform, initialAngle)); 
    }

    public void RemoveOrbitingObject(GameObject obj) {
            orbitingObjects.RemoveAll(o => o.Object == obj.transform); 
        }

    private void UpdateOrbit(OrbitingObjectData data, int index) { 
        float radius = baseOrbitRadius; 
        data.Angle += orbitSpeed * Time.deltaTime; 

        float x = transform.position.x + radius * Mathf.Cos(data.Angle * Mathf.Deg2Rad);
        float y = transform.position.y + radius * Mathf.Sin(data.Angle * Mathf.Deg2Rad);
        data.Object.position = new Vector3(x, y, data.Object.position.z);
    }

    public class OrbitingObjectData {
        public Transform Object; 
        public float Angle; 

        public OrbitingObjectData(Transform obj, float initialAngle) { 
            Object = obj;
            Angle = initialAngle;
        }
    }
}
