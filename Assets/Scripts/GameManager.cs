using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject enemyPrefab;
    public GameObject patientPrefab; // Prefab to spawn
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
        canvas.score.GetComponent<TextMeshProUGUI>().text = "구출한 부상자 : " + score;
    }
    public void Gameover()
    {
        Instantiate(patientPrefab, player.transform.position, Quaternion.identity);

        StartCoroutine(DelayedGameOverActions());
    }

    IEnumerator DelayedGameOverActions()
    {
        yield return new WaitForSeconds(1f); 

        isgameover = true;
        canvas.gameOver.SetActive(true);
    }

}
