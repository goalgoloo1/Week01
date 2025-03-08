using UnityEngine;

public class DrawLine : MonoBehaviour
{
    public LineRenderer lineRenderer; // LineRenderer ������Ʈ
    public float lineLength = 5f; // ���� ����
    public Vector3 startPoint; // ���� ������

    private void Start()
    {
        // LineRenderer�� �� ����
        lineRenderer.positionCount = 2; // �� ������ ���� ����ϴ�.
        startPoint = transform.position; // ���� ��ġ�� ���������� ����
        DrawLineSegment();
    }

    void DrawLineSegment()
    {
        // �������� ���� ����
        Vector3 endPoint = startPoint + transform.up * lineLength; // �� �������� ���� �߽��ϴ�.
        lineRenderer.SetPosition(0, startPoint); // ������
        lineRenderer.SetPosition(1, endPoint); // ����
    }
}
