using JetBrains.Annotations;
using UnityEngine;

public class Patient_Script : MonoBehaviour
{
    // ȯ���� ���� Ÿ�̸� �ð�, �ִ� Ÿ�̸� �ð�
    public float currentTime = 100f;
    public float maxTime = 100f;
    public float timerDecreaseAmount = 0.1f;
    public GameManager gameManager;

    // ȯ�� �Ʒ��� Ÿ�̸Ӹ� �����ִ� ��ũ��Ʈ
    public FloatingTimerBar_Script timerBar;

    private void Awake()
    {
        // ���۽� Ÿ�̸� ������Ʈ �ϱ�
        timerBar.UpdateTimer(currentTime, maxTime);
    }

    private void Start()
    {
        gameManager = GameObject.FindFirstObjectByType<GameManager>();
    }

    private void Update()
    {
        // �� �����Ӹ��� Ÿ�̸� ����
        TimerDecrease(timerDecreaseAmount);
    }

    // 'timerDecreaseAmount'��ŭ �ð��� ����, Ÿ�̸� ������Ʈ
    public void TimerDecrease(float _timerDecreaseAmount)
    {
        currentTime -= _timerDecreaseAmount;
        timerBar.UpdateTimer(currentTime, maxTime);

        // ���� �ð��� 0���� ������, ȯ�� �ڱ��ڽ� �ı�.
        if (currentTime < 0)
        {
            gameManager.Gameover();
            Destroy(gameObject);
        }
    }
}
