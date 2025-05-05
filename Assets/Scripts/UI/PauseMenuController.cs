using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour, IUpdateObserver
{
    /*    // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }*/

    //dodac komponenty do ktorych sie odnosi
    [SerializeField] private PlayerController _player;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject MenuView;
    [SerializeField] private GameObject OptionsView;

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
            else if(_isPaused)
            {
                UnpauseGame();
            }
        }

    }

    private void PauseGame()
    {
        pauseMenuPanel.SetActive(true);

        OptionsView.SetActive(false);
        MenuView.SetActive(true);

        _player.enabled = false;
        Time.timeScale = 0f;
        _isPaused = true;
    }
    public void UnpauseGame()
    {
        pauseMenuPanel.SetActive(false);
        _player.enabled = true;
        Time.timeScale = 10f; //?
        _isPaused = false;
    }

    public void QuitToMainMenu()
    {
        // todo: add quit game session feature
        // e.g. disable muzyki, zapis, etc

        SceneManager.LoadScene(0); // Main Menu
    }

}
