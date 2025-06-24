using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : PersistentSingleton<GameManager>
{
    private int pauseCount = 0;

    private List<string> scenes = new List<string>();
    private int[] randomLevels = new int[3];

    private int startLevelIndex = 6;
    private int currentLevelIndex;

    private int[] costItemList = new int[15];

    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;

        scenes.Add("MainMenu");

        scenes.Add("Level 1");
        scenes.Add("Level 2");
        scenes.Add("Level 3");
        scenes.Add("Level 4");
        scenes.Add("Level 5");

        scenes.Add("Tutotial");
        scenes.Add("Hub 1");

        scenes.Add("Boss Arena 1");
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

        RegeneratePlayer();

        if (SceneManager.GetActiveScene().name == "Hub 1")
        {
            currentLevelIndex = 0;
            GetRandomLevels();
            Debug.Log("hub 1");
        }
        else if (SceneManager.GetActiveScene().name == "Tutotial")
        {
            await SceneLoader.LoadSceneSingle("Hub 1");
            Debug.Log("tutotial");
            return;
        }
        else if (currentLevelIndex == 3)
        {
            await SceneLoader.LoadSceneSingle("Boss Arena 1");
            Debug.Log("index 3");
            return;
        }

        await SceneLoader.LoadSceneSingle(scenes[randomLevels[currentLevelIndex]]);

        Debug.Log(randomLevels[0] + " " + randomLevels[1] + " " + randomLevels[2] + " " + currentLevelIndex);
    }

    private void RegeneratePlayer()
    {
        Player.Instance.healthSystem.RegenerateHealth();
        Player.Instance.staminaSystem.RegenerateStamina();
        Player.Instance.manaSystem.RegenerateMana();
    }

    public async Task AfterPlayerDead()
    {
        await SceneLoader.LoadSceneSingle("Hub 1");
        RegeneratePlayer();
    }

    private void GetRandomLevels()
    {
        int count = 0;
        while (count < 3)
        {
            int random = Random.Range(1, 6); 

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

    public void LoadCostList()
    {
        SlotManager.Instance.LoadItemsCost(costItemList);
    }

    public void SaveCostList()
    {
        costItemList = SlotManager.Instance.GetCostLIst();
    }

    public async Task LoadStartLevel()
    {
        var data = SaveSystem.LoadGame();

        if (data != null)
        {
            startLevelIndex = data.startLevelIndex;
            Player.Instance.goldSystem.SetGoldAmound(data.playerGold);
            SlotManager.Instance.SetPlayerWeapon(data.playerWeapon);
            this.costItemList = data.costItemList;
        }

        await SceneLoader.LoadSceneSingle(scenes[startLevelIndex]);

    }

    public void SaveGame()
    {
        SaveData data = new SaveData
        {
            startLevelIndex = this.startLevelIndex,
            playerGold = Player.Instance.goldSystem.GetGoldAmount(),
            playerWeapon = SlotManager.Instance.GetPlayerWeapon(),
            costItemList = this.costItemList
        };
        SaveSystem.SaveGame(data);
    }
}
