using System.Collections.Generic;
using UnityEngine;

public class PickUpSomething : MonoBehaviour
{
    Player player;
    void Start()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        GameObject PressEUI = transform.Find("Canvas/PressE").gameObject;
        PressEUI.transform.rotation = Quaternion.Euler(Vector2.zero);
        if (player.somethings.Count > 0)
        {
            GameObject lastHitObject = player.somethings[player.somethings.Count - 1];
            PressEUI.SetActive(true);

            float minsize = -40f;
            float maxsize = 40f;
            Transform fill = transform.Find("Canvas/PressE/Fill");
            switch (lastHitObject.tag)
            {
                case "Emergency Kit":
                    float EmergencyKitNeedGauge = 1f;
                    fill.parent.gameObject.SetActive(true);
                    if (player.doingGauge >= EmergencyKitNeedGauge && !transform.Find("AidKit_Indicator").gameObject.activeSelf)
                    {
                        Vector2 randomPos = new Vector2(Random.Range(minsize, maxsize), Random.Range(minsize, maxsize));
                        while (Physics2D.OverlapCircle(randomPos, 0.5f, 1 << 4) != null)
                        {
                            randomPos = new Vector2(Random.Range(minsize, maxsize), Random.Range(minsize, maxsize));
                        }
                        lastHitObject.transform.position = randomPos;
                        player.removeState(Player.PlayerState.doing);
                        transform.Find("AidKit_Indicator").gameObject.SetActive(true);
                        fill.parent.gameObject.SetActive(false);
                    }
                    else if (!transform.Find("AidKit_Indicator").gameObject.activeSelf && player.currentState == Player.PlayerState.doing)
                    {
                        Debug.Log("응급키트 줍는 중...");
                        fill.GetComponent<RectTransform>().localScale = new Vector3(0, player.doingGauge, 0);
                    }
                    break;
                case "Weapon":
                    if (player.doingGauge > 0.7f)
                    {
                        player.TakeWeapon(lastHitObject);
                        player.weapon.transform.GetChild(0).gameObject.SetActive(false);
                        player.doingGauge = 0;
                    }
                    break;
                case "Patient":
                    if (player.doingGauge >= lastHitObject.GetComponent<Patient>()?.doingGaugeNeeded)
                    {

                    }
                    break;
            }
        }
        else 
        {
            PressEUI.SetActive(false);
        }
    }

    // 무언가위에 있는지
    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.somethings.Add(collision.gameObject);
        player.doingGauge = 0;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        player.doingGauge = 0;
        player.somethings.Remove(collision.gameObject);
    }
}
