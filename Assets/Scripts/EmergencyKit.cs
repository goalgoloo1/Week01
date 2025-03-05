using UnityEngine;

public class EmergencyKit : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !collision.gameObject.GetComponent<Player>().isHaveAdkit)
        {
            Debug.Log("¿¿±ﬁ≈∞∆Æ »πµÊ");
            Transform indicator = collision.transform.Find("AidKit_Indicator");
            indicator.gameObject.SetActive(true);
            collision.gameObject.GetComponent<Player>().isHaveAdkit = true;
            Destroy(gameObject);
        }
    }
}
