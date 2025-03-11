using System.Collections;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy() 
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
