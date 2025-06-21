using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        scenes.Add(new SceneStartData { sceneName = "MainMenu", startPosition = new Vector3(0, 0, 0) });
        scenes.Add(new SceneStartData { sceneName = "Tutotial", startPosition = new Vector3(0, 0, 0) });
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

    public async Task Teleport()
    {
        currentLevelIndex++;

        if (SceneManager.GetActiveScene().name == "Hub 1")
        {
            currentLevelIndex = 0;
            GetRandomLevels();
            
        }
        else if (SceneManager.GetActiveScene().name == "Tutotial")
        {
            await SceneLoader.LoadSceneSingle("Hub 1", new Vector3(0,0,0));
        }
        else if (currentLevelIndex == 3)
        {
            await SceneLoader.LoadSceneSingle("Boss Arena 1", new Vector3(0, 0, 0));
        }
        else
        {
            await SceneLoader.LoadSceneSingle(scenes[randomLevels[currentLevelIndex]].sceneName, scenes[randomLevels[currentLevelIndex]].startPosition);
        }

        RegeneratePlayer();
    }

    private void RegeneratePlayer()
    {
        Player.Instance.healthSystem.RegenerateHealth();
        Player.Instance.staminaSystem.RegenerateStamina();
        Player.Instance.manaSystem.RegenerateMana();
    }

    private void GetRandomLevels()
    {
        int count = 0;
        while (count < 3)
        {
            int random = Random.Range(3, 8); 

            bool alreadyExists = false;
            for (int j = 0; j < count; j++)
            {
                if (randomLevels[j] == random)
                {
                    alreadyExists = true;
                    break;
                }
            }

            if (!alreadyExists)
            {
                randomLevels[count] = random;
                count++;
            }
        }
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
