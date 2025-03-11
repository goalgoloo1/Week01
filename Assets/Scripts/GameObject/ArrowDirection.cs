using UnityEngine;

public class ArrowDirection : MonoBehaviour
{
    GameObject player;
    Camera cam;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
        cam = Camera.main;
    }
    void Update()
    {
        // �÷��̾�� ��ǥ�� ������ ���� ���
        Vector3 direction = (transform.parent.position - player.transform.position).normalized;
        // ȭ��ǥ�� ����Ʈ ��ǥ�� ���
        Vector3 arrowPosition = cam.WorldToViewportPoint(player.transform.position + direction*25);
        // ȭ��ǥ�� ȭ�� �����ڸ��� ��ġ
        arrowPosition.x = Mathf.Clamp(arrowPosition.x, 0.03f, 0.97f);
        arrowPosition.y = Mathf.Clamp(arrowPosition.y, 0.1f, 0.97f);
        // ����Ʈ ��ǥ -> ���� ��ǥ ��ȯ
        transform.position = cam.ViewportToWorldPoint(arrowPosition);

        Vector2 arrowDirection = (player.transform.position - transform.parent.position).normalized;
        float angle = Mathf.Atan2(arrowDirection.y, arrowDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);

        //if (transform.parent.tag == "Patient")
        //{
        //    transform.Find("EmergencyKit").gameObject.SetActive(false);
        //    transform.Find("Patient").gameObject.SetActive(true);
        //    transform.Find("Patient").rotation = Quaternion.Euler(Vector3.zero);
        //}
        //else 
        //{
        //    transform.Find("EmergencyKit").gameObject.SetActive(true);
        //    transform.Find("Patient").gameObject.SetActive(false);
        //    transform.Find("EmergencyKit").rotation = Quaternion.Euler(Vector3.zero);
        //}
    }
}
