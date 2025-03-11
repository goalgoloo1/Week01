using UnityEngine;

public class EmergencyKit : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !collision.gameObject.GetComponent<Player>())
        {
            Debug.Log("����ŰƮ ȹ��");
            Transform indicator = collision.transform.Find("AidKit_Indicator");
            indicator.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
