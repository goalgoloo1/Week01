using UnityEngine;

public class ArrowDirection : MonoBehaviour
{
    public GameObject player;
    Camera cam;
    private void Start()
    {
        cam = Camera.main;
    }
    void Update()
    {
        // 플레이어와 목표물 사이의 방향 계산
        Vector3 direction = (transform.parent.position - player.transform.position).normalized;
        // 화살표의 뷰포트 좌표를 계산
        Vector3 playerCamPosition = cam.WorldToViewportPoint(player.transform.position);
        Vector3 targetCamPosition = cam.WorldToViewportPoint(transform.parent.position);
        Vector3 arrowPosition =  targetCamPosition;

        // 화살표를 화면 가장자리에 배치
        arrowPosition.x = Mathf.Clamp(arrowPosition.x, 0.03f, 0.97f);
        arrowPosition.y = Mathf.Clamp(arrowPosition.y, 0.1f, 0.97f);
        // 뷰포트 좌표 -> 월드 좌표 변환
        transform.position = cam.ViewportToWorldPoint(arrowPosition);

        Vector2 arrowDirection = (player.transform.position - transform.parent.position).normalized;
        float angle = Mathf.Atan2(arrowDirection.y, arrowDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);

        if (transform.parent.tag == "Patient")
        {
            transform.Find("EmergencyKit").gameObject.SetActive(false);
            transform.Find("Patient").gameObject.SetActive(true);
            transform.Find("Patient").rotation = Quaternion.Euler(Vector3.zero);
            SpriteRenderer[] sprites = transform.Find("Arrow").GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sprite in sprites) 
            {
                sprite.color = Color.yellow;
            }
        }
        else if (transform.parent.tag == "Emergency Kit")
        {
            transform.Find("EmergencyKit").gameObject.SetActive(true);
            transform.Find("Patient").gameObject.SetActive(false);
            transform.Find("EmergencyKit").rotation = Quaternion.Euler(Vector3.zero);
            SpriteRenderer[] sprites = transform.Find("Arrow").GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sprite in sprites)
            {
                sprite.color = Color.green;
            }
        }
    }
}
