using UnityEngine;

public class EmergencyKit : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            Debug.Log("¿¿±ﬁ≈∞∆Æ »πµÊ");
            //Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("¿¿±ﬁ≈∞∆Æ »πµÊ");
            Transform indicator = collision.transform.Find("AidKit_Indicator");
            indicator.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
