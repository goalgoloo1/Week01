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
    public GameObject[] enemies; // ��� �� ������Ʈ
    float deleteDistance = 40f; // ��Ȱ��ȭ �Ÿ�

    [Header("Game System")]
    public bool isgameover = false;
    public bool isGameClear = false;
    public float startingTime = 60f; // ���۽ð� �� ����
    private float timeRemaining;

    void Start()
    {
        timeRemaining = startingTime; // ���� �ð� �ʱ�ȭ

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

        // ���� �ð��� 0���� ū ��쿡�� ����
        if (timeRemaining > 0 && !isgameover)
        {
            timeRemaining -= Time.deltaTime; // �� ������ �ð� ����
            UpdateTimerDisplay(); // Ÿ�̸� ǥ�� ������Ʈ
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
        int minutes = Mathf.FloorToInt(timeRemaining / 60); // �� ���
        int seconds = Mathf.FloorToInt(timeRemaining % 60); // �� ���

        // "HH:mm" �������� ���ڿ� ����
        timerText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
    }

    IEnumerator DelayedGameOverActions()
    {
        yield return new WaitForSeconds(1f); 

        isgameover = true;
        canvas.gameOver.SetActive(true);
    }

}
