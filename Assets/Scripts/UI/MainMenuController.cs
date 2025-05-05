using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour //, IUpdateObserver
{
    /*
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {}

    // Update is called once per frame
    void Update() {}
    */

    //dodac komponenty do ktorych sie odnosi
    [SerializeField] private GameObject MainMenuView;
    [SerializeField] private GameObject MainOptionsView;


    //private void OnEnable()
    //{
    //    UpdateManager.AddToList(this);
    //}
    //
    //private void OnDisable()
    //{
    //    UpdateManager.RemoveFromList(this);
    //}
    //
    //public void ObserveUpdate()
    //{
    //    if (Input.GetKeyUp(KeyCode.Escape))
    //    {
    //        ;
    //    }
    //
    //}

    public void PlayGame()
    {
        //int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;

        //SceneManager.LoadScene(nextSceneIndex);

        SceneManager.LoadScene(1); // Hub
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
