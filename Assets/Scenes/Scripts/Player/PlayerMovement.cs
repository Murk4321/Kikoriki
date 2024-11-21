using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private Rigidbody2D rb;
    private float forceMult = 4; 
    private Camera cam; 

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main; 
    }

    private void FixedUpdate() {
        MoveTowardsMouse();
    }

    private void MoveTowardsMouse() {
        Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = LimitMouseToCamera(mousePosition);

        Vector3 movePosition = mousePosition - transform.position;

        Vector2 forceDirection = new Vector2(movePosition.x, movePosition.y) * forceMult;
        rb.AddForce(forceDirection, ForceMode2D.Force);
        rb.AddTorque(movePosition.x / 15, ForceMode2D.Force);
    }

    private Vector3 LimitMouseToCamera(Vector3 mousePosition) {
        float minX = cam.ViewportToWorldPoint(new Vector3(-0.5f, 0, cam.nearClipPlane)).x;
        float maxX = cam.ViewportToWorldPoint(new Vector3(1.5f, 0, cam.nearClipPlane)).x;
        float minY = cam.ViewportToWorldPoint(new Vector3(0, -0.5f, cam.nearClipPlane)).y;
        float maxY = cam.ViewportToWorldPoint(new Vector3(0, 1.5f, cam.nearClipPlane)).y;

        mousePosition.x = Mathf.Clamp(mousePosition.x, minX, maxX);
        mousePosition.y = Mathf.Clamp(mousePosition.y, minY, maxY);
        return mousePosition;
    }
}
