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
        Debug.Log("더 월드!");
        Time.timeScale = 0.0f;
        StartCoroutine(Wait(duration));
    }

    IEnumerator Wait(float duration) 
    {
        waiting = true;
        Debug.Log("1초 경과");
        yield return new WaitForSecondsRealtime(duration);
        Debug.Log("시간은 다시 움직인다!");
        Time.timeScale = 1.0f;
        waiting = false;
    }
}
