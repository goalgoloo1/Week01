using UnityEngine;

public class Patient_Script : MonoBehaviour
{
    // 환자의 현재 타이머 시간, 최대 타이머 시간
    public float currentTime = 100f;
    public float maxTime = 100f;
    public float timerDecreaseAmount = 0.5f;

    public GameObject player;
    public float showUIDistance = 1;
    public GameObject PressE_UI;
    public GameObject fill;

    public int issaving = 1;

    // 환자 아래에 타이머를 보여주는 스크립트
    public FloatingTimerBar_Script timerBar;

    private void Awake()
    {
        // 시작시 타이머 업데이트 하기
        timerBar.UpdateTimer(currentTime, maxTime);
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        showUIDistance = 10;
        issaving = 1;
    }

    private void Update()
    {
        // 매 프레임마다 타이머 감소
        TimerDecrease(timerDecreaseAmount);

        // 플레이어가 가까워지면 e UI띄우기
        if (player) 
        {
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
    }

    // 'timerDecreaseAmount'만큼 시간을 빼고, 타이머 업데이트
    public void TimerDecrease(float _timerDecreaseAmount)
    {
        currentTime -= _timerDecreaseAmount * issaving;
        timerBar.UpdateTimer(currentTime, maxTime);

        // 남은 시간이 0보다 작으면, 환자 자기자신 파괴.
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
}
