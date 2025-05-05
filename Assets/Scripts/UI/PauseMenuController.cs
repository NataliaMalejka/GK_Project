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

    [SerializeField] private PlayerController _player;
    [SerializeField] private GameObject pauseMenuPanel;
    private bool _isPaused = false;
    

    //dodac komponenty do ktorych sie odnosi
    public GameObject MenuPanel;
    public GameObject OptionsPanel;

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

        OptionsPanel.SetActive(false);
        MenuPanel.SetActive(true);

        _player.enabled = false;
        Time.timeScale = 0f;
        _isPaused = true;
    }
    public void UnpauseGame()
    {
        pauseMenuPanel.SetActive(false);
        _player.enabled = true;
        Time.timeScale = 10f;
        _isPaused = false;
    }

    public void PlayGame()
    {
        //int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;

        //SceneManager.LoadScene(nextSceneIndex);
        //SceneManager.LoadScene(0); // Hub
    }

    //public void Options()
    //{
    //    OptionPanel.alpha = 1;
    //    OptionPanel.blocksRaycasts = true;
    //}
    //
    //public void Back()
    //{
    //    OptionPanel.alpha = 0;
    //    OptionPanel.blocksRaycasts = false;
    //}

    public void QuitGame()
    {
        Application.Quit();
    }

}
