using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.localPosition; // ���� ��ġ ����
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            elapsed += Time.deltaTime;
            yield return null; // ���� �����ӱ��� ���
        }

        transform.localPosition = originalPosition; // ���� ��ġ�� ����
    }
}
