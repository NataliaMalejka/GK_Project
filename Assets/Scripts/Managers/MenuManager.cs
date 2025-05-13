using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Hub 1");
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
