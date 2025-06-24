using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        _=GameManager.Instance.LoadStartLevel();
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
