using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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
        canvas.GetComponent<Canvas_Script>().gameOver.SetActive(false);
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
        GameObject g = Instantiate(patientPrefab, player.transform.position, Quaternion.identity);

        StartCoroutine(DelayedGameOverActions());
    }

    IEnumerator DelayedGameOverActions()
    {
        yield return new WaitForSeconds(1f); 

        isgameover = true;
        canvas.gameOver.SetActive(true);
    }

}
