using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void GotoStartMenu()
    {
        Debug.Log("��ŸƮ�޴��� ��");
        SceneManager.LoadScene("StartMenu");
    }
    public void RestartGame()
    {
        Debug.Log("�ٽ��ϱ� ��");
        SceneManager.LoadScene("Main 2");
    }


    public void QuitGame()
    {
        Application.Quit();
    }


}
