using UnityEngine;

public class FloatingText_Script : MonoBehaviour
{
    public float textMoveRange = 0.5f; // �������� ����
    public float textMoveSpeed = 1.0f;  // �������� �ӵ�

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position; // �ʱ� ��ġ ����
    }

    void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * textMoveSpeed) * textMoveRange;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
