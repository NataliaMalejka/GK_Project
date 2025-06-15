using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour, IUpdateObserver
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private GameObject pauseMenuPanel;
    private bool _isPaused = false;


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

        GameManager.Instance.PauseGame();
        _isPaused = true;
    }

    public void PlayGame()
    {
        pauseMenuPanel.SetActive(false);

        GameManager.Instance.PlayGame();
        _isPaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
