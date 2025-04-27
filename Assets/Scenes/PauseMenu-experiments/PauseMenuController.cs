using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    /*    // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }*/


    public CanvasGroup OptionPanel;

    public void PlayGame()
    {
        //int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;

        //SceneManager.LoadScene(nextSceneIndex);
        SceneManager.LoadScene(1); // Dave hello world
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
