using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    public Vector2 minBounds; // 맵 최소 좌표
    public Vector2 maxBounds; // 맵 최대 좌표
    private Vector3 offset = new Vector3(0, 0, -10);

    private Camera cam;
    private float halfHeight;
    private float halfWidth;
    private bool isShaking = false; // 카메라 흔들림 상태 체크

    void Start()
    {
        cam = Camera.main;
        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;
    }

    void LateUpdate()
    {
        if (player == null) return;

        if (!isShaking) // 흔들리는 동안에는 경계 제한 적용 안 함
        {
            Vector3 targetPosition = player.transform.position + offset;

            float clampedX = Mathf.Clamp(targetPosition.x, minBounds.x + halfWidth, maxBounds.x - halfWidth);
            float clampedY = Mathf.Clamp(targetPosition.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);

            transform.position = new Vector3(clampedX, clampedY, targetPosition.z);
        }
    }

    // 📌 카메라 흔들기 효과
    public IEnumerator CameraShake(float duration, float magnitude)
    {
        isShaking = true; // 흔들리는 중

        Vector3 originalPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;
            transform.position = new Vector3(originalPosition.x + offsetX, originalPosition.y + offsetY, originalPosition.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
        isShaking = false; // 흔들림 종료
    }
}
