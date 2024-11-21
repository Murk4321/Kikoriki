using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private float forceMult = 5; // ������� ����
    private Camera cam; // ������, ��������������� ��� ����������� ���������� �����
     
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main; // Main Camera
    }

    private void FixedUpdate() {
        if (Input.GetMouseButton(0)) { // ���� ��������� ��� ������ �����
            Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition); // ������������� ������� ����� ������� ������

            Vector3 movePosition = mousePosition - transform.position; // ���������� ������� �������� (������� ������� ���� �������)

            Vector2 forceDirection = new Vector2(movePosition.x, movePosition.y) * forceMult; // ³����� Z ���������� � ������� �� ������� 
            rb.AddForce(forceDirection, ForceMode2D.Force); // ������� ���� ��'����, ForceMode2D.Force �������� ������ ������� ���� ��� �����������
        }
    }
}
