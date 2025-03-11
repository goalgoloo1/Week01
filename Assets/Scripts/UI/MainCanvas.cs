using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCanvas : GameObjectBase
{
    public GameObject lowHp_UI;
    public GameObject gameOver;
    public GameObject score;
    public GameObject staminaBar;
    public GameObject handGunUI;
    public GameObject player;

    private void Start()
    {
        player = GameObject.FindFirstObjectByType<Player>().gameObject;
        gameOver.SetActive(false);
    }
    private void Update()
    {
        if (player)
        {
            ShowMagazin(player.transform.Find("Hand").GetChild(0).GetComponent<Weapon>());
        }
        else 
        {
            gameOver.SetActive(true);
            gameOver.transform.Find("ShowScore").GetComponent<TextMeshProUGUI>().text = score.GetComponent<TextMeshProUGUI>().text;
            if (Input.GetKey(KeyCode.Space)) 
            {
                RestartGame();
            }
        }
    }

    public void RestartGame() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ShowMagazin(Weapon gun)
    {
        TextMeshProUGUI text = handGunUI.transform.Find("Magazin").GetComponent<TextMeshProUGUI>();
        if (text)
        {
            text.text = gun.remainBullet.ToString() + " / " + gun.maxMagazin.ToString();
        }
        else
        {
            Debug.Log("UI¸øÃ£À½");
        }

        Color c = text.color;
        if (gun.remainBullet <= gun.maxMagazin/2)
        {
            c = Color.yellow;
        }
        else
        {
            c = Color.white;
        }

        if (gun.remainBullet == 0)
        {
            c = Color.red;
        }

        text.color = c;
    }

    public void ShowStamina(float maxStamina, float curStamina) 
    {
        float f = staminaBar.transform.localScale.x - curStamina / maxStamina;
        staminaBar.transform.localScale = new Vector3(staminaBar.transform.localScale.x - f/2, 1, 1);
    }
}
