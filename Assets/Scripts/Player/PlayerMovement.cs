using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private Rigidbody2D rb;

    // Множник, який визначає, з якою силою персонаж буде рухатися
    [SerializeField] private float forceMult = 4f;

    // Змінна для зберігання посилання на камеру, необхідну для визначення позиції миші
    private Camera cam;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        // Отримуємо основну камеру сцени
        cam = Camera.main;
    }

    private void FixedUpdate() {
        MoveTowardsMouse();
    }

    // Метод для руху гравця до позиції миші.
    private void MoveTowardsMouse() {
        // Отримуємо позицію миші на екрані і конвертуємо її в координати світу
        // Якщо без ScreenToWorldPoint то координатами 0, 0 вважається лівий верхній кут екрана, 
        // А воно нам не потрібно наху, тому через ScreenToWorldPoint 0, 0 тепер знаходиться у центрі ігрового вікна
        Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

        // Обмежуємо позицію миші, щоб вона не виходила за межі розширених кордонів сцени.
        mousePosition = LimitMouseToExtendedBounds(mousePosition);

        // Обчислюємо напрямок від поточної позиції гравця до позиції миші.
        Vector2 moveDirection = (mousePosition - transform.position);

        // Додаємо силу для переміщення гравця в напрямку миші.
        rb.AddForce(moveDirection * forceMult, ForceMode2D.Force);

        // Додаємо обертання для гравця на основі напрямку до миші (необов'язково, можна налаштувати).
        // Чим більший x-координат напрямку, тим більший момент обертання.
        float torque = moveDirection.x * 0.1f;
        rb.AddTorque(torque, ForceMode2D.Force);
    }

    // Метод, що обмежує позицію миші в межах розширених кордонів сцени.
    private Vector3 LimitMouseToExtendedBounds(Vector3 mousePosition) {
        // Використовуємо координати, розраховані через кінематичну камеру, для обмеження позиції.
        // Визначаємо мінімальні та максимальні значення x і y для миші, щоб не виходила за межі сцени.

        // Мінімальна та максимальна координата X для миші, щоб вона не виходила за межі.
        float minX = cam.ViewportToWorldPoint(new Vector3(-0.5f, 0, cam.nearClipPlane)).x;
        float maxX = cam.ViewportToWorldPoint(new Vector3(1.5f, 0, cam.nearClipPlane)).x;

        // Мінімальна та максимальна координата Y для миші, щоб вона не виходила за межі.
        float minY = cam.ViewportToWorldPoint(new Vector3(0, -0.5f, cam.nearClipPlane)).y;
        float maxY = cam.ViewportToWorldPoint(new Vector3(0, 1.5f, cam.nearClipPlane)).y;

        // Обмежуємо координати миші за заданими межами.
        mousePosition.x = Mathf.Clamp(mousePosition.x, minX, maxX); //Clamp перевіряє щоб змінна була у межах мінімального і максимального
        // Якщо більше ніж максимальне - вона буде повертати завжди лише стале максимальне значення, так само з мінімальним
        mousePosition.y = Mathf.Clamp(mousePosition.y, minY, maxY);

        // Повертаємо нову позицію миші, обмежену в межах.
        return mousePosition;
    }
}
