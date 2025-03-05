using JetBrains.Annotations;
using UnityEngine;

public class Patient_ScriptTEMP : MonoBehaviour
{
    // 환자의 현재 타이머 시간, 최대 타이머 시간
    public float currentTime = 100f;
    public float maxTime = 100f;
    public float timerDecreaseAmount = 0.1f;

    // 환자 아래에 타이머를 보여주는 스크립트
    public FloatingTimerBar_Script timerBar;

    private void Awake()
    {
        // 시작시 타이머 업데이트 하기
        timerBar.UpdateTimer(currentTime, maxTime);
    }

    private void Update()
    {
        // 매 프레임마다 타이머 감소
        TimerDecrease(timerDecreaseAmount);
    }

    // 'timerDecreaseAmount'만큼 시간을 빼고, 타이머 업데이트
    public void TimerDecrease(float _timerDecreaseAmount)
    {
        currentTime -= _timerDecreaseAmount;
        timerBar.UpdateTimer(currentTime, maxTime);

        // 남은 시간이 0보다 작으면, 환자 자기자신 파괴.
        if (currentTime < 0)
        {
            Destroy(gameObject);
        }
    }



}
