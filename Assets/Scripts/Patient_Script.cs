using JetBrains.Annotations;
using UnityEngine;

public class Patient_Script : MonoBehaviour
{
    // ȯ���� ���� Ÿ�̸� �ð�, �ִ� Ÿ�̸� �ð�
    public float currentTime = 100f;
    public float maxTime = 100f;
    public float timerDecreaseAmount = 0.1f;

    public GameObject player;
    public float showUIDistance = 1;
    public GameObject PressE_UI;
    public GameObject fill;

    // ȯ�� �Ʒ��� Ÿ�̸Ӹ� �����ִ� ��ũ��Ʈ
    public FloatingTimerBar_Script timerBar;

    private void Awake()
    {
        // ���۽� Ÿ�̸� ������Ʈ �ϱ�
        timerBar.UpdateTimer(currentTime, maxTime);
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        showUIDistance = 10;
    }

    private void Update()
    {
        // �� �����Ӹ��� Ÿ�̸� ����
        TimerDecrease(timerDecreaseAmount);

        // �÷��̾ ��������� e UI����
        float distance = Vector3.Distance(player.transform.position, transform.position);
        if (distance < showUIDistance && player.GetComponent<Player>().isHaveAdkit)
        {
            PressE_UI.SetActive(true);
        }
        else
        {
            PressE_UI.SetActive(false);
        }
    }

    // 'timerDecreaseAmount'��ŭ �ð��� ����, Ÿ�̸� ������Ʈ
    public void TimerDecrease(float _timerDecreaseAmount)
    {
        currentTime -= _timerDecreaseAmount;
        timerBar.UpdateTimer(currentTime, maxTime);

        // ���� �ð��� 0���� ������, ȯ�� �ڱ��ڽ� �ı�.
        if (currentTime < 0)
        {
            GameManager gm = GameManager.FindFirstObjectByType<GameManager>();
            gm.Gameover();
            Destroy(gameObject);
        }
    }
}
