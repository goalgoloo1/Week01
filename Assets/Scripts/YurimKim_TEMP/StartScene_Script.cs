using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene_Script : MonoBehaviour
{
    public void StartGame()
    {
        // ���� �� �̸� �ٲ��� ������(��� �ٲ㵵 ��)
        SceneManager.LoadScene("Main");
    }
}
