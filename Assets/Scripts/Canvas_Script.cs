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
        // blueGunUI�� �ڽ� ������Ʈ���� BlueGunText�� ã�� TextMeshPro ������Ʈ�� �����ɴϴ�. redGunUINum�� ����ϰ� ������ �� �ֽ��ϴ�.
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
