using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Prefabs")]
    public Player player;
    public Transform playerTransform;
    public Canvas_Script canvas;
    private TextMeshProUGUI timerText;

    [Header("Enemy Manager")]
    public GameObject[] enemies; // 모든 적 오브젝트
    float deleteDistance = 40f; // 비활성화 거리

    [Header("Game System")]
    public bool isgameover = false;
    public bool isGameClear = false;
    public float startingTime = 60f; // 시작시간 초 단위
    private float timeRemaining;

    void Start()
    {
        timeRemaining = startingTime; // 남은 시간 초기화

        player = GameObject.FindFirstObjectByType<Player>();
        canvas = GameObject.FindFirstObjectByType<Canvas_Script>();
        timerText = canvas.timer.GetComponent<TextMeshProUGUI>();
        canvas.GetComponent<Canvas_Script>().gameOver.SetActive(false);
    }

    void Update()
    {
        foreach (var enemy in enemies)
        {
            Enemy enemyController = enemy.GetComponent<Enemy>();
            if (enemyController != null)
            {
                enemyController.NavMeshEnemyOnOff(playerTransform, deleteDistance);
            }
        }

        // 남은 시간이 0보다 큰 경우에만 감소
        if (timeRemaining > 0 && !isgameover)
        {
            timeRemaining -= Time.deltaTime; // 매 프레임 시간 감소
            UpdateTimerDisplay(); // 타이머 표시 업데이트
        }
        else
        {
            Gameover();
            timerText.text = "00:00";
        }

        if (isGameClear)
        {
            SceneManager.LoadScene("ClearMenu");
        }
    }
    public void Gameover()
    {
        Destroy(player.gameObject);
        StartCoroutine(DelayedGameOverActions());
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60); // 분 계산
        int seconds = Mathf.FloorToInt(timeRemaining % 60); // 초 계산

        // "HH:mm" 형식으로 문자열 생성
        timerText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
    }

    IEnumerator DelayedGameOverActions()
    {
        yield return new WaitForSeconds(1f); 

        isgameover = true;
        canvas.gameOver.SetActive(true);
    }

}
