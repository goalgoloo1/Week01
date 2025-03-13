using System.Collections;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    public static HitStop Instance { get; private set; }
    public static bool waiting;
    public void Stop(float duration) 
    {
        if (waiting) 
        {
            return;
        }
        Debug.Log("�� ����!");
        Time.timeScale = 0.0f;
        StartCoroutine(Wait(duration));
    }

    IEnumerator Wait(float duration) 
    {
        waiting = true;
        Debug.Log("1�� ���");
        yield return new WaitForSecondsRealtime(duration);
        Debug.Log("�ð��� �ٽ� �����δ�!");
        Time.timeScale = 1.0f;
        waiting = false;
    }
}
