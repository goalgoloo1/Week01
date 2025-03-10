using UnityEngine;

public class Patient_Script : MonoBehaviour
{
    public GameObject player;
    public float showUIDistance = 1;
    public GameObject PressE_UI;
    public GameObject fill;

    public int issaving = 1;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        showUIDistance = 10;
        issaving = 1;
    }

    private void Update()
    {
        // �� �����Ӹ��� Ÿ�̸� ����
        // TimerDecrease(timerDecreaseAmount);

        // �÷��̾ ��������� e UI����
        if (player) 
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance < showUIDistance)
            {
                PressE_UI.SetActive(true);
            }
            else
            {
                PressE_UI.SetActive(false);
            }
        }
    }

    // 'timerDecreaseAmount'��ŭ �ð��� ����, Ÿ�̸� ������Ʈ
    /*
    public void TimerDecrease(float _timerDecreaseAmount)
    {
        currentTime -= _timerDecreaseAmount * issaving;
        timerBar.UpdateTimer(currentTime, maxTime);

        // ���� �ð��� 0���� ������, ȯ�� �ڱ��ڽ� �ı�.
        if (currentTime < 0)
        {
            GameManager gm = GameManager.FindFirstObjectByType<GameManager>();
            //if (!gm.isgameover)
            //{
            //    gm.Gameover();
            //}
            GameObject tomb = Resources.Load<GameObject>("Prefabs/Tomb");
            Instantiate(tomb, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
    */
}
