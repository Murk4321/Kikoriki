using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private Rigidbody2D rb;

    // �������, ���� �������, � ���� ����� �������� ���� ��������
    [SerializeField] private float forceMult = 4f;

    // ����� ��� ��������� ��������� �� ������, ��������� ��� ���������� ������� ����
    private Camera cam;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        // �������� ������� ������ �����
        cam = Camera.main;
    }

    private void FixedUpdate() {
        MoveTowardsMouse();
    }

    // ����� ��� ���� ������ �� ������� ����.
    private void MoveTowardsMouse() {
        // �������� ������� ���� �� ����� � ���������� �� � ���������� ����
        // ���� ��� ScreenToWorldPoint �� ������������ 0, 0 ��������� ���� ������ ��� ������, 
        // � ���� ��� �� ������� ����, ���� ����� ScreenToWorldPoint 0, 0 ����� ����������� � ����� �������� ����
        Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

        // �������� ������� ����, ��� ���� �� �������� �� ��� ���������� ������� �����.
        mousePosition = LimitMouseToExtendedBounds(mousePosition);

        // ���������� �������� �� ������� ������� ������ �� ������� ����.
        Vector2 moveDirection = (mousePosition - transform.position);

        // ������ ���� ��� ���������� ������ � �������� ����.
        rb.AddForce(moveDirection * forceMult, ForceMode2D.Force);

        // ������ ��������� ��� ������ �� ����� �������� �� ���� (������'������, ����� �����������).
        // ��� ������ x-��������� ��������, ��� ������ ������ ���������.
        float torque = moveDirection.x * 0.1f;
        rb.AddTorque(torque, ForceMode2D.Force);
    }

    // �����, �� ������ ������� ���� � ����� ���������� ������� �����.
    private Vector3 LimitMouseToExtendedBounds(Vector3 mousePosition) {
        // ������������� ����������, ���������� ����� ���������� ������, ��� ��������� �������.
        // ��������� ������� �� ���������� �������� x � y ��� ����, ��� �� �������� �� ��� �����.

        // ̳������� �� ����������� ���������� X ��� ����, ��� ���� �� �������� �� ���.
        float minX = cam.ViewportToWorldPoint(new Vector3(-0.5f, 0, cam.nearClipPlane)).x;
        float maxX = cam.ViewportToWorldPoint(new Vector3(1.5f, 0, cam.nearClipPlane)).x;

        // ̳������� �� ����������� ���������� Y ��� ����, ��� ���� �� �������� �� ���.
        float minY = cam.ViewportToWorldPoint(new Vector3(0, -0.5f, cam.nearClipPlane)).y;
        float maxY = cam.ViewportToWorldPoint(new Vector3(0, 1.5f, cam.nearClipPlane)).y;

        // �������� ���������� ���� �� �������� ������.
        mousePosition.x = Mathf.Clamp(mousePosition.x, minX, maxX); //Clamp �������� ��� ����� ���� � ����� ���������� � �������������
        // ���� ����� �� ����������� - ���� ���� ��������� ������ ���� ����� ����������� ��������, ��� ���� � ���������
        mousePosition.y = Mathf.Clamp(mousePosition.y, minY, maxY);

        // ��������� ���� ������� ����, �������� � �����.
        return mousePosition;
    }
}
