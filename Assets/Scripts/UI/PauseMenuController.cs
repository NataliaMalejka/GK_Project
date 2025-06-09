using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour, IUpdateObserver
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private GameObject pauseMenuPanel;
    private bool _isPaused = false;


    public CanvasGroup OptionPanel;

    private void OnEnable()
    {
        UpdateManager.AddToList(this);
    }

    private void OnDisable()
    {
        UpdateManager.RemoveFromList(this);
    }

    public void ObserveUpdate()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if(!_isPaused)
            {
                PauseGame();
            }
            else
            {
                PlayGame();
            }
        }
    }

    private void PauseGame()
    {
        pauseMenuPanel.SetActive(true);

        Time.timeScale = 0f;
        _isPaused = true;
    }

    public void PlayGame()
    {
        pauseMenuPanel.SetActive(false);

        Time.timeScale = 1f;
        _isPaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
