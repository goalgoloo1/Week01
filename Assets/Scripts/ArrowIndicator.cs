using System.Collections.Generic;
using UnityEngine;

public class ArrowIndicator : MonoBehaviour
{
    public Transform player; // �÷��̾� ����
    public GameObject arrowPrefab; // ȭ��ǥ ������
    private List<GameObject> arrows = new List<GameObject>(); // ������ ȭ��ǥ ����Ʈ
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        UpdateArrowIndicators();
    }

    void UpdateArrowIndicators()
    {
        // ���� ȭ��ǥ ����
        foreach (var arrow in arrows)
        {
            Destroy(arrow);
        }
        arrows.Clear();

        // ȯ�� ã��
        GameObject[] patients = GameObject.FindGameObjectsWithTag("Patient");

        foreach (GameObject patient in patients)
        {
            Vector3 screenPos = cam.WorldToViewportPoint(patient.transform.position);

            // ȭ�� �ۿ� �ִ� ȯ�ڸ� ȭ��ǥ ǥ��
            if (screenPos.x < 0 || screenPos.x > 1 || screenPos.y < 0 || screenPos.y > 1)
            {
                CreateArrow(patient.transform);
            }
        }
    }

    void CreateArrow(Transform target)
    {
        // ȭ��ǥ ����
        GameObject arrow = Instantiate(arrowPrefab, transform);
        arrows.Add(arrow);

        // �÷��̾�� ��ǥ�� ������ ���� ���
        Vector3 direction = (target.position - player.position).normalized;

        // ȭ��ǥ�� ȭ�� �����ڸ��� ��ġ
        Vector3 arrowPosition = cam.WorldToViewportPoint(player.position + direction * 5f);
        arrowPosition.x = Mathf.Clamp(arrowPosition.x, 0.05f, 0.95f);
        arrowPosition.y = Mathf.Clamp(arrowPosition.y, 0.05f, 0.95f);

        // ����Ʈ ��ǥ -> ���� ��ǥ ��ȯ
        arrow.transform.position = cam.ViewportToWorldPoint(arrowPosition);
        arrow.transform.position = new Vector3(arrow.transform.position.x, arrow.transform.position.y, 0f);

        // ?? ȯ���� ������ ���ϵ��� ȸ��
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.Euler(0, 0, angle - 90f); // -90�� ����(ȭ��ǥ ��翡 �°�)
    }
}
