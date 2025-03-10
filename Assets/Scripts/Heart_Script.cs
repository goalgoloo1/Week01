using UnityEngine;

public class Heart_Script : MonoBehaviour
{
    public Player player;

    public GameObject[] pinkHearts; // ��ȫ ��Ʈ ������Ʈ �迭
    public GameObject[] grayHearts; // ȸ�� ��Ʈ ������Ʈ �迭

    void Start()
    {
        player = GameObject.FindFirstObjectByType<Player>();
        UpdateHearts(); // ��Ʈ ���� ������Ʈ
    }

    private void Update()
    {
        UpdateHearts();
    }

    // ��Ʈ ���� ������Ʈ �޼���
    private void UpdateHearts()
    {
        for (int i = 0; i < player.maxHp; i++)
        {
            // ��ȫ ��Ʈ�� ȸ�� ��Ʈ�� Ȱ��ȭ ���¸� ����
            if (i < player.hp)
            {
                pinkHearts[i].SetActive(true); // ��ȫ ��Ʈ �ѱ�
                grayHearts[i].SetActive(false); // ȸ�� ��Ʈ ����
            }
            else
            {
                pinkHearts[i].SetActive(false); // ��ȫ ��Ʈ ����
                grayHearts[i].SetActive(true); // ȸ�� ��Ʈ �ѱ�
            }
        }
    }
}
