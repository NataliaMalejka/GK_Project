using UnityEngine;

public class GameManager : PersistentSingleton<GameManager>
{
    private int pauseCount = 0;

    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
    }

    public void PauseGame()
    {
        pauseCount++;
        Time.timeScale = 0;
    }

    public void PlayGame()
    {
        pauseCount--;

        if(pauseCount == 0)
            Time.timeScale = 1f;
    }
}
