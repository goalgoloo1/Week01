using UnityEngine;

public class EmergencyKit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("¿¿±ﬁ≈∞∆Æ »πµÊ");
            Transform indicator = collision.transform.Find("AidKit_Indicator");
            indicator.gameObject.SetActive(true);
            collision.gameObject.GetComponent<Player>().isCanSave = true;
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("¿¿±ﬁ≈∞∆Æ »πµÊ");
            Transform indicator = collision.transform.Find("AidKit_Indicator");
            indicator.gameObject.SetActive(true);
            collision.gameObject.GetComponent<Player>().isCanSave = true;
            Destroy(gameObject);
        }
    }
}
