using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private float forceMult = 5; // Множник сили
    private Camera cam; // Камера, використовується для правильного зчитування мишки
     
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main; // Main Camera
    }

    private void FixedUpdate() {
        if (Input.GetMouseButton(0)) { // Якщо натиснута ліва кнопка мишки
            Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition); // Вираховування позиції мишки відносно камери

            Vector3 movePosition = mousePosition - transform.position; // Розрахунок вектора напрямку (цільова позиція мінус поточна)

            Vector2 forceDirection = new Vector2(movePosition.x, movePosition.y) * forceMult; // Відрізаєм Z координату і множимо на множник 
            rb.AddForce(forceDirection, ForceMode2D.Force); // Надання сили об'єкту, ForceMode2D.Force дозволяє давати постійну силу без накопичення
        }
    }
}
