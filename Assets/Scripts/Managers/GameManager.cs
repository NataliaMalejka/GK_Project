using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SceneStartData
{
    public string sceneName;
    public Vector3 startPosition;
}

public class GameManager : PersistentSingleton<GameManager>
{
    private int pauseCount = 0;

    private List<SceneStartData> scenes = new List<SceneStartData>();
    private int[] randomLevels = new int[3];

    private int startLevelIndex = 1;
    private int currentLevelIndex;

    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
        currentLevelIndex = 1;

        scenes.Add(new SceneStartData { sceneName = "MainMenu", startPosition = new Vector3(0, 0, 0) });
        scenes.Add(new SceneStartData { sceneName = "Tutoria", startPosition = new Vector3(0, 0, 0) });
        scenes.Add(new SceneStartData { sceneName = "Hub 1", startPosition = new Vector3(0, 0, 0) });

        scenes.Add(new SceneStartData { sceneName = "Level 1", startPosition = new Vector3(0, 0, 0) });
        scenes.Add(new SceneStartData { sceneName = "Level 2", startPosition = new Vector3(0, 0, 0) });
        scenes.Add(new SceneStartData { sceneName = "Level 3", startPosition = new Vector3(0, 0, 0) });
        scenes.Add(new SceneStartData { sceneName = "Level 4", startPosition = new Vector3(0, 0, 0) });
        scenes.Add(new SceneStartData { sceneName = "Level 5", startPosition = new Vector3(0, 0, 0) });

        scenes.Add(new SceneStartData { sceneName = "Boss Arena 1", startPosition = new Vector3(0, 0, 0) });
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

    public void Teleport()
    {

    }

    public async Task LoadStartLevel()
    {
        var data = SaveSystem.LoadGame();

        if (data != null)
        {
            startLevelIndex = data.startLevelIndex;
            Player.Instance.goldSystem.SetGoldAmound(data.playerGold);
            SlotManager.Instance.SetPlayerWeapon(data.playerWeapon);
        }

        await SceneLoader.LoadSceneSingle(scenes[startLevelIndex].sceneName, scenes[startLevelIndex].startPosition);

    }

    public void SaveGame()
    {
        SaveData data = new SaveData
        {
            startLevelIndex = this.startLevelIndex,
            playerGold = Player.Instance.goldSystem.GetGoldAmount(),
            playerWeapon = SlotManager.Instance.GetPlayerWeapon()
        };
        SaveSystem.SaveGame(data);
    }
}
