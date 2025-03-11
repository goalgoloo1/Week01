using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using static UnityEngine.UI.Image;
using Unity.VisualScripting;

public class LeftFist : MonoBehaviour
{
    public List<GameObject> list = new List<GameObject>();
    public float power = 0f;
    Vector2 beforePos;
    Vector2 currentPos;
    public bool ispunching = false;

    void Start()
    {
    }

    void Update()
    {
        if (ispunching) 
        {
            currentPos = transform.position;
            beforePos = currentPos;
            currentPos = transform.position;
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.5f, (currentPos - beforePos).normalized, Vector2.Distance(beforePos, currentPos),1<<6);
            if (hit && hit.collider.CompareTag("Enemy"))
            {
                if (list.Contains(hit.collider.gameObject))
                {
                    Debug.Log("이미 한대 맞았음");
                }
                else
                {
                    Debug.Log("처음맞음");
                    hit.collider.GetComponent<CharacterBase>().takeDamage((hit.collider.transform.position - transform.position).normalized, power * 50, 1 + (int)power);
                    list.Add(hit.collider.gameObject);
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        // 원의 범위를 그리기
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f);  // 원 그리기

        // 원이 이동하는 범위를 그리기
        Gizmos.color = Color.red;
        Gizmos.DrawLine(beforePos, currentPos);  // 범위 표시
    }
}
