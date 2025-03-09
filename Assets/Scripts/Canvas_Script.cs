using UnityEngine;
using TMPro;

public class Canvas_Script : MonoBehaviour
{
    public GameObject lowHp_UI;
    public GameObject gameOver;
    public GameObject score;
    public GameObject staminaBar;
    public GameObject blueGunUI;
    public GameObject redGunUI;
    public TextMeshProUGUI blueGunUINum;
    public TextMeshProUGUI redGunUINum;

    void Start()
    {
        // blueGunUI의 자식 오브젝트에서 BlueGunText를 찾고 TextMeshPro 컴포넌트를 가져옵니다. redGunUINum도 비슷하게 가져올 수 있습니다.
        blueGunUINum = blueGunUI.transform.Find("BlueGunText").GetComponent<TextMeshProUGUI>();
        redGunUINum = redGunUI.transform.Find("RedGunText").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        
    }

    public void TurnOff(GameObject _gameObject)
    {
        _gameObject.SetActive(false);
    }

    public void TurnOn(GameObject _gameObject)
    {
        _gameObject.SetActive(true);
    }

    public void UpdateGunNumber(TextMeshProUGUI _text, int _gunNum)
    {
        _text.text = _gunNum.ToString();
    }
}
