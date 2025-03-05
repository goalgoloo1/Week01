using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Canvas_Script canvas;
    public bool isgameover = false;
    public int score;

    void Start()
    {
        player = GameObject.FindFirstObjectByType<Player>();
        canvas = GameObject.FindFirstObjectByType<Canvas_Script>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && isgameover) 
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        canvas.score.GetComponent<TextMeshProUGUI>().text = "score : " + score;
    }

    public void Gameover() 
    {
        isgameover = true;
        canvas.gameOver.SetActive(true);
    }
}
